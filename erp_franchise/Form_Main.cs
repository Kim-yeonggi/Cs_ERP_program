using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams; //NuGet에서 설치

using MySql.Data.MySqlClient;





namespace erp_franchise
{

    public partial class Form_Main : Form
    {
        // SQL 연결
        private MySqlConnection conn;
        private static string server = "192.168.31.147";
        private static string database = "team2";
        private static string uid = "root";
        private static string password = "0000";
        private static string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";


        //public List<List<string>> employee_list;
        // 김영기
        DateTimePicker dateTimePicker_add_ep_ptStart = new DateTimePicker();
        DateTimePicker dateTimePicker_add_ep_ptEnd2 = new DateTimePicker();
        public DateTime dateTime_start = new DateTime(2000, 01, 01, 09, 00, 00);
        public DateTime dateTime_End = new DateTime(2000, 01, 01, 18, 00, 00);

        public List<string> selected_employee_code = new List<string>();




        // 김자연
        private List<string> recommendationList = new List<string>();
        private int saveCount = 0;
        // 필터링 전에 DataGridView에 표시된 데이터를 백업하는 리스트
        List<DataGridViewRow> originalRows = new List<DataGridViewRow>();



        // 김중규
        public List<List<string>> csvData;
        public string csvFilePath = "customInfo.csv"; // 파일 경로

        public List<List<string>> csvOdData;
        public string csvOdFilePath = "orderList.csv"; // 파일 경로

        private Dictionary<string, int> buttonValues = new Dictionary<string, int>();

        public int count = 1;
        public List<List<string>> temp_list = new List<List<string>>();

        public static int serialNumber = 1;



        // 강병헌





        public Form_Main()
        {
            InitializeComponent();

            // ---------------------- 김영기 메인 ------------------------
            label_nowDateTime.Text = DateTime.Now.ToString("F");





            // ----------------------- 김영기 인사 / 급여 관리 ------------------
            comboBox_search_ep_category.SelectedIndex = 0;
            comboBox_add_ep_emailD.SelectedIndex = 0;
            Employee.employee_detail_property = create_db_list("employee_inform");
            //Employee.employee_certificate_property = Employee.create_employee_db("employee_certificate.csv");
            //Employee.employee_language_property = Employee.create_employee_db("employee_language.csv");







            // --------------  김중규 매출관리 ----------------
            // CSV 파일 읽기
            //Customer.Customer_property = CsvHandler.ReadCsvToDlist(csvFilePath);
            Customer.Customer_property = create_db_list("custominfo");
            csvData = Customer.Customer_property;

            // csvData를 수정하고자 하는 작업을 수행합니다. 이 예시에서는 첫 번째 행의 첫 번째 열 값을 "NewValue"로 변경하는 것으로 가정합니다.
            csvData[0][0] = "NewValue"; // 예시: 첫 번째 행의 첫 번째 열 값을 "NewValue"로 변경



            // CSV 파일 읽기
            //Customer.OdList_property = CsvOdHandler.ReadCsvToOdDlist(csvOdFilePath);
            Customer.OdList_property = create_db_list("orderlist");
            csvOdData = Customer.OdList_property;

            // csvData를 수정하고자 하는 작업을 수행합니다. 이 예시에서는 첫 번째 행의 첫 번째 열 값을 "NewValue"로 변경하는 것으로 가정합니다.
            csvOdData[0][0] = "NewValue"; // 예시: 첫 번째 행의 첫 번째 열 값을 "NewValue"로 변경


            // 딕셔너리에 버튼 이름과 값 추가
            buttonValues.Add(아메리카노.Name, 2000); // button1은 아메리카노 2,000원
            buttonValues.Add(아메리카노아이스.Name, 2500); // button2는 아메리카노 아이스 2,500원
            buttonValues.Add(카페라떼.Name, 3500); // button3은 카페라떼 3,500원
            buttonValues.Add(카페라떼아이스.Name, 4000); // button4는 카페라떼 아이스 4,000원

            buttonValues.Add(콜라.Name, 2000); // button21은 콜라 2,000원
            buttonValues.Add(사이다.Name, 2000); // button23은 사이다 2,000원
            buttonValues.Add(닥터페퍼.Name, 2000); // button24는 닥터페퍼 2,000원
            buttonValues.Add(밀키스.Name, 2000); // button25은 밀키스 2,000원

            buttonValues.Add(아이스크림.Name, 3000); // button22은 아이스크림 3,000원
            buttonValues.Add(구슬아이스크림.Name, 4000); // button42는 구슬아이스크림 4,000원
            buttonValues.Add(밀크쉐이크.Name, 3000); // button43은 밀크쉐이크 3,000원
            buttonValues.Add(슬러시.Name, 3000); // button44는 슬러시 3,000원








            // -----------------  김자연 구매, 발주 관리 -------------------
            //Employee.employee_detail_property = Employee.create_employee("employee_detail.csv");

            // DataGridView에 바인딩할 데이터 테이블 초기화
            DataTable dataTable_st_list = new DataTable();
            DataTable dataTable_st_bsk = new DataTable();

            // DataGridView에 데이터 테이블 바인딩
            dataGridView_st_list.DataSource = dataTable_st_list;
            dataGridView_st_bsk.DataSource = dataTable_st_bsk;

            LoadDataToDataGridView();

            textBox_st_list_code.Click += new EventHandler(textBox_st_list_code_Click);
            textBox_st_list_name.Click += new EventHandler(textBox_st_list_name_Click);
            listBox_st_list_code.DoubleClick += new EventHandler(listBox_st_list_code_DoubleClick);
            listBox_st_list_name.DoubleClick += new EventHandler(listBox_st_list_name_DoubleClick);
            listBox_st_apply_mng.DoubleClick += new EventHandler(listBox_st_apply_mng_DoubleClick);

            listBox_st_list_code.Visible = false;
            listBox_st_list_name.Visible = false;
            listBox_st_apply_mng.Visible = false;

            // 폼이 로드될 때 캘린더를 보이지 않도록 설정
            Calendar_st_apply_day.Visible = false;
            Calendar_st_apply_day.DateSelected += Calendar_st_apply_day_DateChanged;

            Calendar_st_apply_delivery.Visible = false;
            Calendar_st_apply_delivery.DateSelected += Calendar_st_apply_delivery_DateChanged;

            Calendar_st_history_day.Visible = false;
            Calendar_st_history_day.DateSelected += Calendar_st_history_day_DateChanged;

            Calendar_st_history_delivery.Visible = false;
            Calendar_st_history_delivery.DateSelected += Calendar_st_history_delivery_DateChanged;

            textBox_st_apply_day.Text = DateTime.Now.ToShortDateString();
            textBox_st_apply_day.TextAlign = HorizontalAlignment.Center;
            textBox_st_apply_day.Location = new System.Drawing.Point(452, 222); // 위치 설정

            textBox_st_apply_delivery.Text = DateTime.Now.ToShortDateString();
            textBox_st_apply_delivery.TextAlign = HorizontalAlignment.Center;
            textBox_st_apply_delivery.Location = new System.Drawing.Point(1278, 222); // 위치 설정

            textBox_st_history_day.Text = DateTime.Now.ToShortDateString();
            textBox_st_history_day.TextAlign = HorizontalAlignment.Center;
            textBox_st_history_day.Location = new System.Drawing.Point(1278, 222); // 위치 설정

            textBox_st_history_day1.Text = DateTime.Now.ToShortDateString();
            textBox_st_history_day1.TextAlign = HorizontalAlignment.Center;
            textBox_st_history_day1.Location = new System.Drawing.Point(1278, 222); // 위치 설정

            dataGridView_st_list.CellValueChanged += dataGridView_st_list_CellValueChanged;
            dataGridView_st_bsk.CellValueChanged += dataGridView_st_bsk_CellValueChanged2;
            dataGridView_st_list.CurrentCellDirtyStateChanged += dataGridView_st_list_StateChanged;
            dataGridView_st_bsk.CurrentCellDirtyStateChanged += dataGridView_st_list_DirtyStateChanged;

            // CellClick 이벤트 핸들러 추가
            dataGridView_history.CellClick += DataGridViewHistory_CellClick;

            // DataGridView2에 CurrentCellDirtyStateChanged 이벤트 핸들러 추가
            dataGridView_st_bsk.CurrentCellDirtyStateChanged += dataGridView_st_bsk_CurrentCellDirtyStateChanged;

            // DataGridView2에 CellValueChanged 이벤트 핸들러 추가
            dataGridView_st_bsk.CellValueChanged += dataGridView_st_bsk_CellValueChanged1;





            // ----------------- 강병헌 운영, 가맹 관리 --------------------
            Equipment.equipment_detail_property = new List<List<string>>();


        }



        // 메인
        // 좌측 상단 현재 시각
        private void Form_Main_Load(object sender, EventArgs e)         // 좌측 상단 날짜,시간 실시간 업데이트
        {
            timer_nowDateTime.Start();
            timer_nowDateTime.Interval = 1000;

            for (int i = 0; i < Employee.employee_detail_property.Count; i++)
            {
                if (Employee.employee_detail_property[i][0] == label_logined_code.Text)
                {
                    label_logined_name.Text = Employee.employee_detail_property[i][2];
                }
                
            }
            
        }

        private void timer_nowDateTime_Tick(object sender, EventArgs e)     // 날짜, 시간 출력 포매팅
        {
            label_nowDateTime.Text = DateTime.Now.ToString("F");
        }







        public void Set_ID(string logined_id)
        {
            label_logined_code.Text = logined_id;

            foreach (List<string> list in Employee.employee_detail_property)
            {
                try
                {
                    if (list.IndexOf(logined_id) != -1)
                    {
                        pictureBox_logined_user.Image = System.Drawing.Image.FromFile(list[8]);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }


        private void logout_button_Click(object sender, EventArgs e)
        {
            Form_Login f1 = new Form_Login();

            var logout_confirm = MessageBox.Show("로그아웃 하시겠습니까?", "로그아웃", MessageBoxButtons.YesNo);
            if (logout_confirm == DialogResult.Yes)
            {
                this.Hide();
                f1.ShowDialog();
                this.Close();
            }
        }

        private void side_mng_btn_Click(object sender, EventArgs e)
        {
            panel_mng_side.Visible = true;
            panel_hr_side.Visible = false;
            panel_pch_side.Visible = false;
            panel_sales_side.Visible = false;
            panel_fc_side.Visible = false;
        }

        // 대분류 버튼
        private void side_hr_btn_Click(object sender, EventArgs e)
        {
            panel_mng_side.Visible = false;
            panel_hr_side.Visible = true;
            panel_pch_side.Visible = false;
            panel_sales_side.Visible = false;
            panel_fc_side.Visible = false;
        }

        private void side_pch_btn_Click(object sender, EventArgs e)
        {
            panel_mng_side.Visible = false;
            panel_hr_side.Visible = false;
            panel_pch_side.Visible = true;
            panel_sales_side.Visible = false;
            panel_fc_side.Visible = false;
        }

        private void side_sales_btn_Click(object sender, EventArgs e)
        {
            panel_mng_side.Visible = false;
            panel_hr_side.Visible = false;
            panel_pch_side.Visible = false;
            panel_sales_side.Visible = true;
            panel_fc_side.Visible = false;
        }

        private void side_fc_btn_Click(object sender, EventArgs e)
        {
            panel_mng_side.Visible = false;
            panel_hr_side.Visible = false;
            panel_pch_side.Visible = false;
            panel_sales_side.Visible = false;
            panel_fc_side.Visible = true;
        }



        // 소분류 패널 open 버튼
        private void button_open_mng1_Click(object sender, EventArgs e) // 장비관리
        {
            equ_panel.Visible = true;
            as_panel.Visible = false;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = false;

            panell_st.Visible = false;

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = false;

            label_top_menu.Text = "운영>장비관리";
        }

        private void button_open_mng2_Click(object sender, EventArgs e) // A/S 접수
        {
            equ_panel.Visible = false;
            as_panel.Visible = true;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = false;

            //  구매관리 패널 false

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = false;

            label_top_menu.Text = "운영>A/S 접수";

        }




        private void button_open_hr1_Click(object sender, EventArgs e)  // 인사관리
        {
            equ_panel.Visible = false;
            as_panel.Visible = false;

            panel_main_hr1.Visible = true;
            panel_main_hr2.Visible = false;

            panell_st.Visible = false;

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = false;

            label_top_menu.Text = "인사>직원관리";



            button_ep_inquiry_Click(sender, e);         // 조회 버튼 함수(데이터 그리드 그리기 함수) 호출




            dateTimePicker_add_ep_ptStart.Location = new System.Drawing.Point(3, 4);



            // 근무시간 dateTimePicker 생성
            dateTimePicker_add_ep_ptStart.Value = dateTime_start;
            dateTimePicker_add_ep_ptStart.Location = new System.Drawing.Point(3, 7);
            dateTimePicker_add_ep_ptStart.Format = DateTimePickerFormat.Custom;
            dateTimePicker_add_ep_ptStart.Size = new Size(122, 21);
            dateTimePicker_add_ep_ptStart.ShowUpDown = true;
            dateTimePicker_add_ep_ptStart.CustomFormat = "HH:mm";
            dateTimePicker_add_ep_ptStart.TabIndex = 541;
            this.panel_ep_temp_worktime.Controls.Add(dateTimePicker_add_ep_ptStart);



            dateTimePicker_add_ep_ptEnd2.Value = dateTime_End;
            dateTimePicker_add_ep_ptEnd2.Location = new System.Drawing.Point(143, 7);
            dateTimePicker_add_ep_ptEnd2.Format = DateTimePickerFormat.Custom;
            dateTimePicker_add_ep_ptEnd2.Size = new Size(122, 21);
            dateTimePicker_add_ep_ptEnd2.ShowUpDown = true;
            dateTimePicker_add_ep_ptEnd2.CustomFormat = "HH:mm";
            dateTimePicker_add_ep_ptEnd2.TabIndex = 543;
            this.panel_ep_temp_worktime.Controls.Add(dateTimePicker_add_ep_ptEnd2);
        }

        private void button_open_hr2_Click(object sender, EventArgs e)
        {
            equ_panel.Visible = false;
            as_panel.Visible = false;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = true;

            panell_st.Visible = false;

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = false;

            label_top_menu.Text = "인사>직원 통계";

        }

        private void button_open_pch1_Click(object sender, EventArgs e)
        {
            equ_panel.Visible = false;
            as_panel.Visible = false;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = false;

            panell_st.Visible = true;

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = false;

            label_top_menu.Text = "구매>재고관리";

        }

        private void button_open_sales1_Click(object sender, EventArgs e)
        {
            equ_panel.Visible = false;
            as_panel.Visible = false;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = false;

            panell_st.Visible = false;

            tableLayoutPanel_order.Visible = true;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = false;

            label_top_menu.Text = "매출>주문관리";

        }

        private void button_open_sales2_Click(object sender, EventArgs e)
        {
            equ_panel.Visible = false;
            as_panel.Visible = false;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = false;

            panell_st.Visible = false;

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = true;

            store_panel.Visible = false;

            label_top_menu.Text = "매출>고객관리";

        }

        private void button_open_fr1_Click(object sender, EventArgs e)
        {

            equ_panel.Visible = false;
            as_panel.Visible = false;

            panel_main_hr1.Visible = false;
            panel_main_hr2.Visible = false;

            panell_st.Visible = false;

            tableLayoutPanel_order.Visible = false;
            tableLayoutPanel_customer.Visible = false;

            store_panel.Visible = true;

            label_top_menu.Text = "가맹>가맹관리";


        }







        // 인사관리 기능
        private void button_ep_inquiry_Click(object sender, EventArgs e)        // 출력을 위한 임시 버튼
        {
            Employee.employee_detail_property = create_db_list("employee_inform");
            Employee.create_DataGridView_employeeList(Employee.employee_detail_property, dataGridView_employee); // 데이터그리드 그리기

            //// 체크박스 선택한 상태에서 조회 버튼 누를 경우
            //foreach (string sec in selected_employee_code)
            //{
            //    for (int i = 0; i < Employee.employee_detail_property.Count; i++)
            //    {
            //        if (Employee.employee_detail_property[i].IndexOf(sec) != -1)
            //        {
            //            dataGridView_employee.Rows[i-1].Cells["ep_dataGrid_checkBox"].Value = true;
            //        }
            //    }
            //}
            selected_employee_code.Clear();
        }




        private void dataGridView_employee_CellValueChanged(object sender, DataGridViewCellEventArgs e)     // 체크박스 선택 함수
        {
            if (e.RowIndex >= 0 && dataGridView_employee.Columns[e.ColumnIndex].Name == "ep_dataGrid_checkBox")     // 선택한 셀의 헤더 이름이 ep_dataGrid_checkBox 인지 검사
            {
                // 체크박스의 값이 True 인지 검사, 이미 선택되어 있던 내용인지 검사
                if (Convert.ToBoolean(dataGridView_employee.Rows[e.RowIndex].Cells["ep_dataGrid_checkBox"].Value))  
                {
                        dataGridView_employee.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(200,200,200);
                        selected_employee_code.Add(dataGridView_employee.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
                else
                {
                    dataGridView_employee.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    selected_employee_code.Remove(dataGridView_employee.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
            }
        }
        private void dataGridView_employee_CurrentCellDirtyStateChanged(object sender, EventArgs e)     // 즉각적으로 반응
        {
            dataGridView_employee.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        // List<string>
        private void button_remove_employee_Click(object sender, EventArgs e)       // 삭제 버튼
        {
            if (selected_employee_code.Count == 0)
            {
                MessageBox.Show("선택된 항목이 없습니다.", "알림");
            }
            else
            {
                //List<List<string>> employee_list = Employee.remove_employee(selected_employee_code);
                Employee.remove_employee(selected_employee_code);
                //Employee.create_DataGridView_employeeList(employee_list, dataGridView_employee);
                Employee.create_DataGridView_employeeList(create_db_list("employee_inform"), dataGridView_employee);
            }

        }

        // 검색 기능
        private void button_search_employee_Click(object sender, EventArgs e)
        {


            Employee.employee_detail_property = create_db_list("employee_inform");
            Employee.create_DataGridView_employeeList(Employee.search_employee(comboBox_search_ep_category, textBox_search_ep_text, dataGridView_employee), dataGridView_employee);
            selected_employee_code.Clear();

        }










        // 직원 탭
        private List<string> selected_employee_informs = new List<string>();
        private void dataGridView_employee_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string selected_employee_code = dataGridView_employee.Rows[e.RowIndex].Cells[0].Value.ToString();
            List<List<string>> employee_list = Employee.employee_detail_property;

            for (int el = 0; el < employee_list.Count; el++)
            {
                if (employee_list[el][0] == selected_employee_code)
                {
                    selected_employee_informs = employee_list[el];
                    break;
                }
            }

            try
            {
                textBox_add_ep_code.Text = selected_employee_informs[0];
                textBox_add_ep_pw.Text = selected_employee_informs[1];
                textBox_add_ep_name.Text = selected_employee_informs[2];
                textBox_add_ep_rrn.Text = selected_employee_informs[3];
                textBox_add_ep_rrn3.Text = selected_employee_informs[4];
                textBox_add_ep_pn.Text = selected_employee_informs[5];
                textBox_add_ep_emailF.Text = selected_employee_informs[6].Substring(0, selected_employee_informs[6].IndexOf('@'));
                textBox_add_ep_emailD.Text = selected_employee_informs[6].Substring(selected_employee_informs[6].IndexOf('@')+1, selected_employee_informs[6].Length - selected_employee_informs[6].IndexOf('@')-1);
                textBox_add_ep_address.Text = selected_employee_informs[7];
                pictureBox_add_ep_profile.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox_add_ep_profile.Image = System.Drawing.Image.FromFile(selected_employee_informs[8]);
                comboBox_add_ep_education.Text = selected_employee_informs[9];
                comboBox_add_ep_military.Text = selected_employee_informs[10];
                comboBox_add_ep_div.Text = selected_employee_informs[11];
                comboBox_add_ep_pos.Text = selected_employee_informs[12];
                dateTimePicker_add_ep_jac.Text = selected_employee_informs[13];
                textBox_add_ep_work.Text = selected_employee_informs[14];
                dateTimePicker_add_ep_ptStart.Value = DateTime.Parse(selected_employee_informs[15]);
                dateTimePicker_add_ep_ptEnd2.Value = DateTime.Parse(selected_employee_informs[16]);
                comboBox_add_ep_off.Text = selected_employee_informs[17];
                textBox_add_ep_contFile.Text = selected_employee_informs[18];
                //dateTimePicker_add_ep_retire.Text = selected_employee_informs[19];
                textBox_add_ep_retire_reason.Text = selected_employee_informs[20];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }











        // 직원 추가 탭
        private void button_add_ep_add_Click(object sender, EventArgs e)       // 직원 정보 추가 버튼
        {
            ////bool input = true;
            //string filePath_employee_detail = "employee_detail.csv";
            //string filePath_employee_certificate = "employee_certificate.csv";
            //string filePath_employee_language = "employee_language.csv";

            // 필수 항목 리스트
            List<string> essential = new List<string>()
            {
                textBox_add_ep_code.Text,
                textBox_add_ep_pw.Text,
                textBox_add_ep_name.Text,
                textBox_add_ep_pn.Text,
                comboBox_add_ep_div.Text,
                comboBox_add_ep_pos.Text,
                textBox_add_ep_emailF.Text,
                textBox_add_ep_emailD.Text,
                textBox_add_ep_rrn.Text,
                textBox_add_ep_rrn3.Text,
                textBox_add_ep_address.Text,
                comboBox_add_ep_div.Text,
                comboBox_add_ep_pos.Text
            };


            //필수 정보 입력
            foreach (string es in essential)
            {
                if (es == "")
                {
                    MessageBox.Show("비어있는 필수 정보 입력칸이 있습니다.", "경고");
                    return;
                }
            }
            if (pictureBox_add_ep_profile.Image == null)
            {
                MessageBox.Show("비어있는 필수 정보 입력칸이 있습니다.", "경고");
                return;
            }


            // 중복체크
            foreach (List<string> edl in Employee.employee_detail_property)
            {
                if (edl[0] == textBox_add_ep_code.Text)
                {
                    MessageBox.Show("이미 동일한 CODE가 존재합니다.", "경고");
                    return;
                }
            }


            // csv에 넣을 리스트들 선언
            List<string> addToCsv_employee_detail = new List<string>()     
            {
                textBox_add_ep_code.Text,                                                                       // 0: 직원코드(id)
                textBox_add_ep_pw.Text,                                                                         // 1: 비밀번호
                textBox_add_ep_name.Text,                                                                       // 2: 이름
                textBox_add_ep_rrn.Text,                                                                        // 3: 생년월일
                textBox_add_ep_rrn3.Text,                                                                       // 4: 주민등록번호 뒤
                textBox_add_ep_pn.Text.Replace("-", String.Empty),                                              // 5: 연락처
                textBox_add_ep_emailF.Text + '@' + textBox_add_ep_emailD.Text,                                  // 6. 이메일 아이디
                textBox_add_ep_address.Text,                                                                    // 7: 주소
                textBox_add_ep_proFile.Text.Replace("\\", "\\\\"),                                                                    // 8: 증명사진
                comboBox_add_ep_education.Text,                                                                 // 9: 최종학력
                comboBox_add_ep_military.Text,                                                                  // 10: 병역 
                comboBox_add_ep_div.Text,                                                                       // 11: 소속 
                comboBox_add_ep_pos.Text,                                                                       // 12: 직급 
                dateTimePicker_add_ep_jac.Text.Replace("-", String.Empty),                                      // 13: 입사일
                textBox_add_ep_work.Text,                                                                       // 14: 직무
                dateTimePicker_add_ep_ptStart.Text,                                                             // 15: 근무 시작시간
                dateTimePicker_add_ep_ptEnd2.Text,                                                              // 16: 근무 종료시간
                comboBox_add_ep_off.Text,                                                                       // 17: 상태
                textBox_add_ep_contFile.Text.Replace("\\", "\\\\"),                                                                   // 18: 계약서
                dateTimePicker_add_ep_retire.Text.Replace("-", String.Empty),                                   // 19: 퇴사일
                textBox_add_ep_retire_reason.Text                                                               // 20: 퇴직사유
            };

            foreach(string str in addToCsv_employee_detail)
            {
                if (str.IndexOf('\\') != -1)
                {
                    str.Replace("\\", "\\\\");
                }
            }




            if (MessageBox.Show("직원 정보를 추가하시겠습니까?", "알림", MessageBoxButtons.OKCancel) == DialogResult.OK) 
            {
                try
                {
                    conn = new MySqlConnection(connectionString);

                    if (make_connection())
                    {
                        string ep_inform_add_query = "INSERT INTO team2.employee_inform VALUES(";
                        foreach (string aed in addToCsv_employee_detail)
                        {
                            int except = addToCsv_employee_detail.IndexOf(aed);
                            if (except != 13 && except != 19)
                            {
                                ep_inform_add_query += $"'{aed}',";
                            }
                            else
                            {
                                ep_inform_add_query += $"{aed},";
                            }
                        }
                        ep_inform_add_query = ep_inform_add_query.Remove(ep_inform_add_query.Length - 1);
                        ep_inform_add_query += ");";

                        textBox_add_ep_retire_reason.Text = ep_inform_add_query;

                        MySqlCommand cmd = new MySqlCommand(ep_inform_add_query, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        //MessageBox.Show("쿼리 전송 성공");
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //MessageBox.Show("쿼리 전송 실패");
                }

                Employee.create_DataGridView_employeeList(Employee.employee_detail_property, dataGridView_employee);




                // 초기화 작업
                ResetControls(tableLayoutPanel_ep_idpw);
                ResetControls(tableLayoutPanel_detail1);
                ResetControls(panel_tabelLayoutPanel_email);
                pictureBox_add_ep_profile.Image = null;

                ResetControls(tableLayoutPanel_add_ep_baisc);
                ResetControls(panel_ep_temp_cont);
                ResetControls(panel_ep_temp_rrn);

                ResetControls(tableLayoutPanel_add_ep_certificate);
                ResetControls(panel_add_ep_temp_certificate);
                ResetControls(tableLayoutPanel_add_ep_language);
                ResetControls(panel_add_ep_temp_language);

                dateTimePicker_add_ep_certificate_date.Value = DateTime.Now;
                dateTimePicker_add_ep_jac.Value = DateTime.Now;
                dateTimePicker_add_ep_ptStart.Value = dateTime_start;
                dateTimePicker_add_ep_ptEnd2.Value = dateTime_End;

                dataGridView_add_ep_certificate.Rows.Clear();
                dataGridView_add_ep_language.Rows.Clear();

                addToCsv_employee_certificate.Clear();
                addToCsv_employee_language.Clear();

                MessageBox.Show("추가/수정이 완료되었습니다.", "알림");

                button_ep_inquiry_Click(sender, e);
            }
            
        }

        
        
        private void ResetControls(Control con) // 초기화 함수
        {
            foreach (Control control in con.Controls)
            {
                //컨트롤 속성으로 찾는 방법
                if ((control is System.Windows.Forms.TextBox || control is System.Windows.Forms.ComboBox) || (control is Label && control.Name.IndexOf("File") != -1))      // 텍스트박스, 콤보박스 내용 초기화
                {
                    if (control.Name == "comboBox_add_ep_emailD")
                    {
                        comboBox_add_ep_emailD.SelectedIndex = 0;
                    }
                    else
                    {
                        control.Text = "";
                    }
                }

            }

        }

        private void button_add_ep_loadAccount_Click(object sender, EventArgs e)    // 통장사본 파일 불러오기 버튼
        {
            //textBox_add_ep_accFile.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string accountFile;

            openFileDialog.InitialDirectory = "..\\..\\..\\res\\account";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                accountFile = openFileDialog.FileName;
                //label_add_ep_accFile.Text = accountFile;
                //textBox_add_ep_accFile.Text = accountFile.Split('\\')[accountFile.Split('\\').Length-1];
            }
        }

        private void button_add_ep_loadProfile_Click(object sender, EventArgs e)        // 증명사진 불러오기
        {
            textBox_add_ep_proFile.Text = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string profileFile;

            openFileDialog.InitialDirectory = "..\\..\\..\\res\\profile";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                profileFile = openFileDialog.FileName;
                textBox_add_ep_proFile.Text = profileFile;

                pictureBox_add_ep_profile.Load($"{profileFile}");
                pictureBox_add_ep_profile.SizeMode = PictureBoxSizeMode.Zoom;
            }

        }

        private void button_add_ep_loadContract_Click(object sender, EventArgs e)   // 계약서 불러오기
        {
            textBox_add_ep_contFile.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string contractFile;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                contractFile = openFileDialog.FileName;
                textBox_add_ep_contFile.Text = contractFile;
            }
        }



        private void comboBox_add_ep_emailD_SelectedValueChanged(object sender, EventArgs e)        // 이메일 도메인 선택한 경우 텍스트박스에 값 전달
        {
            if (comboBox_add_ep_emailD.SelectedIndex == 0)
            {
                textBox_add_ep_emailD.ReadOnly = false;
                textBox_add_ep_emailD.Text = "";

            }
            else
            {
                textBox_add_ep_emailD.Text = comboBox_add_ep_emailD.Text;
                textBox_add_ep_emailD.ReadOnly = true;
            }
        }



        private void pictureBox_logined_user_Paint(object sender, PaintEventArgs e)
        {
            // GraphicsPath를 사용하여 원형의 경로를 생성합니다.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox_logined_user.Width, pictureBox_logined_user.Height);

            // PictureBox의 Region 속성에 경로를 할당하여 원형 모양을 설정합니다.
            pictureBox_logined_user.Region = new Region(path);
        }

        private void button_add_ep_reset_Click(object sender, EventArgs e)
        {
            textBox_add_ep_code.Text = "";
            textBox_add_ep_pw.Text = "";
            textBox_add_ep_name.Text = "";
            textBox_add_ep_rrn.Text = "";
            textBox_add_ep_rrn3.Text = "";
            textBox_add_ep_emailF.Text = "";
            textBox_add_ep_emailD.Text = "";
            textBox_add_ep_address.Text = "";
            pictureBox_add_ep_profile.Image = null;
            comboBox_add_ep_education.Text = "";
            comboBox_add_ep_military.Text = "";
            comboBox_add_ep_div.Text = "";
            comboBox_add_ep_pos.Text = "";
            dateTimePicker_add_ep_jac.Value = DateTime.Now;
            textBox_add_ep_work.Text = "";
            dateTimePicker_add_ep_ptStart.Text = "09:00";
            dateTimePicker_add_ep_ptEnd2.Text = "18:00";
            comboBox_add_ep_off.Text = "";
            textBox_add_ep_contFile.Text = "";
            //dateTimePicker_add_ep_retire.Text = selected_employee_informs[16];
            textBox_add_ep_retire_reason.Text = "";

            dataGridView_add_ep_certificate.Rows.Clear();
            dataGridView_add_ep_language.Rows.Clear();
        }


        // 자격, 어학 추가
        private List<List<string>> addToCsv_employee_certificate = new List<List<string>>();

        private void button_add_ep_certificate_Click(object sender, EventArgs e)
        {
            List<string> certificate = new List<string>()
            {
                textBox_add_ep_code.Text,
                textBox_add_ep_name.Text,
                textBox_add_ep_certificate_name.Text,
                dateTimePicker_add_ep_certificate_date.Text,
                textBox_add_ep_certificate_agency.Text,
                textBox_add_ep_certFile.Text + "\n"
            };

            List<string> dgv_certificate = new List<string>()
            {
                textBox_add_ep_certificate_name.Text,
                dateTimePicker_add_ep_certificate_date.Text,
                textBox_add_ep_certificate_agency.Text,
            };
            dataGridView_add_ep_certificate.Rows.Add(dgv_certificate.ToArray());
            addToCsv_employee_certificate.Add(certificate);
        }

        private List<List<string>> addToCsv_employee_language = new List<List<string>>();
        private void button_add_ep_language_Click(object sender, EventArgs e)
        {
            List<string> language = new List<string>()
            {
                textBox_add_ep_code.Text,
                textBox_add_ep_name.Text,
                textBox_add_ep_language_name.Text,
                textBox_add_ep_language_score.Text,
                dateTimePicker_add_ep_language.Text,
                textBox_add_ep_language_agency.Text,
                textBox_add_ep_langFile.Text + "\n"
            };

            List<string> dgv_language = new List<string>()
            {
                textBox_add_ep_language_name.Text,
                textBox_add_ep_language_score.Text,
                dateTimePicker_add_ep_language.Text,
                textBox_add_ep_language_agency.Text + "\n",
            };
            dataGridView_add_ep_language.Rows.Add(dgv_language.ToArray());
            addToCsv_employee_language.Add(language);
        }


        // 차트 그리기
        private void paint_chart()
        {



        }























        // 김중규
        private void InitializeDataGrid()
        {
            dataGridViewCSV.Rows.Clear();
        }


        public static class CsvHandler
        {
            public static List<List<string>> ReadCsvToDlist(string filePath)
            {
                List<List<string>> DList = new List<List<string>>(); // 이중 리스트 초기화 선언

                string[] inputlines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (string line in inputlines) // 각 라인에 대해 반복
                {
                    List<string> customData = line.Split(',').ToList(); // 쉼표를 기준으로 문자열을 나누고 리스트로 변환
                    DList.Add(customData); // 이중리스트에 추가
                }

                return DList; // 이중리스트 반환
            }

            public static void WriteDlistToCsv(string filePath, List<List<string>> DList)
            {
                List<string> lines = new List<string>();

                foreach (List<string> row in DList) // 각 행에 대해 반복
                {
                    string line = string.Join(",", row); // 각 행을 문자열로 결합하여 쉼표로 구분된 형태로 만듭니다.
                    lines.Add(line); // 각 행을 리스트에 추가
                }

                File.WriteAllLines(filePath, lines, Encoding.UTF8); // 파일에 리스트의 내용을 씁니다.
            }
        }


        private void btnSearchCustomRg_Click(object sender, EventArgs e)
        {

            InitializeDataGrid();   // 데이터그리드 초기화

            string searchText = TbSearchRg.Text.ToLower();
            dataGridViewCSV.Rows.Clear();

            bool searchResultExists = false;
            //foreach (List<string> row in CsvHandler.ReadCsvToDlist(csvFilePath).Skip(1))
            Customer.Customer_property = create_db_list("custominfo");
            foreach (List<string> row in Customer.Customer_property)
            {
                if (row.Any(cellValue => cellValue.ToLower().Contains(searchText)))
                {
                    dataGridViewCSV.Rows.Add(row.ToArray());
                    searchResultExists = true;
                }
            }

            if (!searchResultExists)
            {
                MessageBox.Show("검색된 결과가 없습니다.");
            }

            //행 전체 선택 할 수 있도록
            dataGridViewCSV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // 그리드뷰 컬럼폭 채우기
            dataGridViewCSV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewCSV.Rows.Count) // 선택된 행이 유효한지 확인합니다.
            {
                DataGridViewRow selectedRow = dataGridViewCSV.Rows[e.RowIndex]; // 선택된 행을 가져옵니다.

                // 각 셀의 값을 각각의 textBox에 할당합니다.
                tbCustomIDRg.Text = selectedRow.Cells[0].Value?.ToString() ?? "";
                tbNameRg.Text = selectedRow.Cells[1].Value?.ToString() ?? "";
                tbMobileRg.Text = selectedRow.Cells[2].Value?.ToString() ?? "";
                tbBirthRg.Text = selectedRow.Cells[3].Value?.ToString() ?? "";
                tbGenderRg.Text = selectedRow.Cells[4].Value?.ToString() ?? "";
                tbCatgRg.Text = selectedRow.Cells[5].Value?.ToString() ?? "";
                tbMemoRg.Text = selectedRow.Cells[6].Value?.ToString() ?? "";
                //tbCouponRg.Text = selectedRow.Cells[7].Value.ToString();
                //tbStackNumRg.Text = selectedRow.Cells[8].Value.ToString();
                //tbAccPointRg.Text = selectedRow.Cells[9].Value.ToString();
                tbPointRg.Text = e.RowIndex.ToString(); // 선택된 행의 인덱스를 출력합니다.
            }

        }


        private void tbMobileRg_TextChanged(object sender, EventArgs e)
        {

            // 0는 반드시 숫자 입력 요
            // 9은 숫자나 공란
            //tbMobileRg.Mask = "(999)000-0000";

        }

        // 여러 개의 TextBox에 입력된 데이터를 CSV 파일에 추가
        // 여러 개의 TextBox와 Button 컨트롤을 추가
        // 버튼의 Click 이벤트에 btnAddToCSV_Click 메서드를 연결
        private void btnCIReg_Click(object sender, EventArgs e)
        {
            // 텍스트 상자에서 값을 가져옵니다.
            string id = tbCustomIDRg.Text;
            string name = tbNameRg.Text;
            string mobile = tbMobileRg.Text;
            string birth = tbBirthRg.Text;
            string gender = tbGenderRg.Text;
            string catg = tbCatgRg.Text;
            string memo = tbMemoRg.Text;

            string createTableQuery = $"INSERT INTO customInfo (id, name, mobile, birth, gender, catg, memo) VALUES('{id}', '{name}', '{mobile}', '{birth}', '{gender}', '{catg}', '{memo}');";
            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    MySqlCommand cmd = new MySqlCommand(createTableQuery, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("쿼리 전송 성공");
                }
                else
                {
                    MessageBox.Show("쿼리 전송 실패");
                }

                // 성공 메시지 표시
                MessageBox.Show("등록이 완료 되었습니다.");

                btnSearchCustomRg.PerformClick();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



            //// TextBox에서 데이터 가져오기
            //string[] dataToAdd = {tbCustomIDRg.Text,
            //    tbNameRg.Text,
            //    tbMobileRg.Text,
            //    tbBirthRg.Text,
            //    tbGenderRg.Text,
            //    tbCatgRg.Text,
            //    tbMemoRg.Text 
            //    // 추가할 데이터 
            //};
            //using (StreamWriter sw = new StreamWriter(csvFilePath, true, Encoding.UTF8))
            //{
            //    // 각 TextBox의 데이터를 CSV 형식으로 한 줄에 추가
            //    sw.WriteLine(string.Join(",", dataToAdd), Encoding.UTF8);
            //}
            //// 성공 메시지 표시
            //MessageBox.Show("등록이 완료 되었습니다.");

            //btnSearchCustomRg.PerformClick();
            //// DataGridView의 DataSource를 변경한 후 Refresh 메서드를 호출하여 변경 사항을 적용합니다.
            ////dataGridViewCSV.Rows.Add = sw;
            ////dataGridViewCSV.Refresh();
        }

        private void btnCIDel_Click(object sender, EventArgs e)
        {

            //// 선택된 행을 식별하고 DataGridViewRow 객체로 가져옵니다.
            //DataGridViewRow selectedRow = dataGridViewCSV.SelectedRows[0];

            //// 선택된 행의 인덱스를 가져옵니다.
            //int selectedIndex = selectedRow.Index;

            //// DataGridView에서 선택된 행을 제거합니다.
            //dataGridViewCSV.Rows.RemoveAt(selectedIndex);

            //// CSV 파일에서 선택된 행을 제외한 나머지 행을 가져옵니다.
            //List<List<string>> csvData = CsvHandler.ReadCsvToDlist(csvFilePath);
            //csvData.RemoveAt(selectedIndex + 1);

            //// 수정된 CSV 데이터를 CSV 파일에 다시 씁니다.
            //CsvHandler.WriteDlistToCsv(csvFilePath, csvData);


            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    DeleteStoreData("custominfo", "id", tbCustomIDRg.Text);
                }
                btnSearchCustomRg_Click(sender, e);
                MessageBox.Show("삭제가 완료되었습니다.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void btnCIModify_Click(object sender, EventArgs e)
        {

            // DataGridView에서 선택된 행이 있는지 확인합니다.
            if (dataGridViewCSV.SelectedRows.Count > 0)
            {
                // 선택된 행을 식별하고 DataGridViewRow 객체로 가져옵니다.
                DataGridViewRow selectedRow = dataGridViewCSV.SelectedRows[0];

                // 선택된 행의 인덱스를 가져옵니다.
                int selectedIndex = selectedRow.Index;

                // 선택된 행의 데이터를 수정합니다.
                selectedRow.Cells[0].Value = tbCustomIDRg.Text;
                selectedRow.Cells[1].Value = tbNameRg.Text;
                selectedRow.Cells[2].Value = tbMobileRg.Text;
                selectedRow.Cells[3].Value = tbBirthRg.Text;
                selectedRow.Cells[4].Value = tbGenderRg.Text;
                selectedRow.Cells[5].Value = tbCatgRg.Text;
                selectedRow.Cells[6].Value = tbMemoRg.Text;
                // 필요한 경우 나머지 열에 대해서도 수정합니다.

                // 수정된 DataGridView의 데이터를 CSV 파일에 반영합니다.
                List<List<string>> csvData = CsvHandler.ReadCsvToDlist(csvFilePath);
                csvData[selectedIndex + 1] = new List<string>
                {
                    tbCustomIDRg.Text,
                    tbNameRg.Text,
                    tbMobileRg.Text,
                    tbBirthRg.Text,
                    tbGenderRg.Text,
                    tbCatgRg.Text,
                    tbMemoRg.Text
                    // 필요한 경우 나머지 열에 대해서도 추가합니다.
                };


                CsvHandler.WriteDlistToCsv(csvFilePath, csvData);
            }
            else
            {
                MessageBox.Show("수정할 행을 선택해주세요.");
            }
        }

        private void customReg_Load(object sender, EventArgs e)
        {
            btnSearchCustomRg.PerformClick();

        }

        private void button_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button clickedButton = sender as System.Windows.Forms.Button;
            int quantity = 1;
            string m = "";

            //if (clickedButton != null)      // 버튼이 눌러졌는지 검사
            //{
            // 클릭된 버튼의 이름을 가져와서 해당 버튼의 값을 딕셔너리에서 찾아옵니다.
            string buttonName = clickedButton.Name;     // 클릭된 버튼의 이름
            if (buttonValues.ContainsKey(buttonName))   // 버튼에 값(가격)이 있는지
            {
                int buttonValue = buttonValues[buttonName];


                // 버튼의 값에 대한 처리를 수행합니다.
                //MessageBox.Show($"버튼 '{buttonName}'의 값은 {buttonValue:C0} 입니다.");


                // button Name : 항목 이름 / buttonValue : 가격
                List<string> temp = new List<string>() { count.ToString(), buttonName, $"{quantity}", buttonValue.ToString(), (quantity * buttonValue).ToString(), m };
                temp_list.Add(temp);

                dataGridViewOdList.Rows.Clear();
                foreach (List<string> tl in temp_list)
                {
                    dataGridViewOdList.Rows.Add(tl.ToArray());
                }
            }
            //}

            count++;


        }

        private void InitializeDataGrid0()
        {
            // DataGrid 초기화
            //dataGridViewCSV.Columns.Clear();
            dataGridViewOdList.Rows.Clear();
        }
        public static class CsvOdHandler
        {
            public static List<List<string>> ReadCsvToOdDlist(string filePath)
            {
                List<List<string>> OdDList = new List<List<string>>(); // 이중 리스트 초기화 선언

                string[] inputlines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (string line in inputlines) // 각 라인에 대해 반복
                {
                    List<string> customData = line.Split(',').ToList(); // 쉼표를 기준으로 문자열을 나누고 리스트로 변환
                    OdDList.Add(customData); // 이중리스트에 추가
                }

                return OdDList; // 이중리스트 반환
            }

            public static void WriteOdDlistToCsv(string filePath, List<List<string>> OdDList)
            {
                List<string> lines = new List<string>();

                foreach (List<string> row in OdDList) // 각 행에 대해 반복
                {
                    string line = string.Join(",", row); // 각 행을 문자열로 결합하여 쉼표로 구분된 형태로 만듭니다.
                    lines.Add(line); // 각 행을 리스트에 추가
                }

                File.WriteAllLines(filePath, lines, Encoding.UTF8); // 파일에 리스트의 내용을 씁니다.
            }
        }



        private void btnSearchCustomOd_Click(object sender, EventArgs e)
        {
            InitializeDataGrid0();   // 데이터그리드 초기화

            string searchText = tbSearchNameOd.Text.ToLower();
            dataGridViewOdList.Rows.Clear();

            bool searchResultExists = false;
            foreach (List<string> row in CsvOdHandler.ReadCsvToOdDlist(csvOdFilePath).Skip(1))
            {
                if (row.Any(cellValue => cellValue.ToLower().Contains(searchText)))
                {
                    dataGridViewOdList.Rows.Add(row.ToArray());
                    searchResultExists = true;
                }
            }

            if (!searchResultExists)
            {
                MessageBox.Show("검색된 결과가 없습니다.");
            }

            //행 전체 선택 할 수 있도록
            dataGridViewOdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // 그리드뷰 컬럼폭 채우기
            dataGridViewOdList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }



        static string OdNumber()
        {

            // 현재 날짜를 가져와서 "yyyyMMdd" 형식으로 변환

            string currentDate = DateTime.Now.ToString("yyMM");

            // 일련번호 생성 (예: 1부터 시작)

            // 날짜와 일련번호를 결합하여 일련번호 문자열 생성
            string OdNum = "Od" + currentDate + serialNumber.ToString("D4");

            return OdNum;
        }


        private void orderReg_Load(object sender, EventArgs e)
        {
            label_OdNum.Text = OdNumber();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            serialNumber++;
            label_OdNum.Text = OdNumber();
            dataGridViewOdList.Rows.Clear();

        }









        // 강병헌
        private void button0_Click(object sender, EventArgs e)
        {
            // 임의의 이중 리스트 (지역)변수에 _employee_detail 에 저장되어 있는 값 전달
            List<List<string>> list = Equipment.equipment_detail_property;
        }


        private void equ_search_button_Click(object sender, EventArgs e)
        {
            string searchText = equ_search_num_textBox.Text.ToLower();
            string searchText1 = equ_search_mnum_textBox.Text.ToLower();
            string searchText2 = equ_search_name_textBox.Text.ToLower();
            string searchText3 = equ_search_mname_textBox.Text.ToLower();
            string searchText4 = equ_search_amount_textBox.Text.ToLower();
            string searchText5 = equ_search_my_textbox.Text.ToLower();
            string searchText6 = equ_search_insdate_textbox.Text.ToLower();
            string searchText7 = equ_search_date_textBox.Text.ToLower();

            equ_dataGridView.Rows.Clear();

            List<List<string>> equipment_list = create_equipment();

            foreach (List<string> row in equipment_list)
            {
                if (row[0].ToLower().Contains(searchText) &&
                    row[1].ToLower().Contains(searchText1) &&
                    row[2].ToLower().Contains(searchText2) &&
                    row[3].ToLower().Contains(searchText3) &&
                    row[4].ToLower().Contains(searchText4) &&
                    row[5].ToLower().Contains(searchText5) &&
                    row[6].ToLower().Contains(searchText6) &&
                    row[7].ToLower().Contains(searchText7))
                {
                    equ_dataGridView.Rows.Add(row.ToArray());
                }
            }

            if (equ_dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("검색된 결과가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        } //장비 검색 버튼





        private void as_search_button_Click(object sender, EventArgs e)//A/S 검색 버튼
        {
            string searchText = as_search_textBox.Text.ToLower();
            as_dataGridView.Rows.Clear();

            List<List<string>> as_list = create_as();
            List<string> as_dataGrid_headers = as_list[0];

            foreach (List<string> row in as_list)
            {
                if (row.Any(cellValue => cellValue.ToLower().Contains(searchText)))
                {
                    as_dataGridView.Rows.Add(row.ToArray());
                }
            }
            if (as_dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("검색된 결과가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void store_search_button_Click(object sender, EventArgs e)
        {
            string searchText = store_search_textBox.Text.ToLower();
            store_dataGridView.Rows.Clear();

            List<List<string>> store_list = create_store();
            List<string> store_dataGrid_headers = store_list[0];

            foreach (List<string> row in store_list)
            {
                if (row.Any(cellValue => cellValue.ToLower().Contains(searchText)))
                {
                    store_dataGridView.Rows.Add(row.ToArray());
                }
            }
            if (store_dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("검색된 결과가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InitializeDataGrid_equ()
        {
            // DataGrid 초기화
            equ_dataGridView.Rows.Clear();
            as_dataGridView.Rows.Clear();
            store_dataGridView.Rows.Clear();

        }


        private void create_DataGridView(List<List<string>> equipment_list)        // 장비 데이터 그리드를 그리는 함수
        {
            for (int j = 0; j < equipment_list.Count; j++)
            {
                equ_dataGridView.Rows.Add(equipment_list[j].ToArray());
            }

        }

        private void equ_view_button_Click(object sender, EventArgs e)      //장비 데이터파일 불러오기
        {
            InitializeDataGrid_equ();   // 데이터그리드 초기화

            List<List<string>> equipment_list = create_db_list("Equipment");      // 이중 리스트를 선언하여 함수 리턴값 저장

            create_DataGridView(equipment_list); // 데이터그리드 그리기
        }



        private void as_view_button_Click(object sender, EventArgs e)       //A/S데이터파일 불러오기
        {
            InitializeDataGrid_equ();   // 데이터그리드 초기화

            List<List<string>> as_list = create_db_list("AStable");
            create_as_DataGridView(as_list); // 데이터그리드 그리기
        }
        private void create_as_DataGridView(List<List<string>> as_list)        // 데이터 그리드를 그리는 함수
        {
            for (int j = 0; j < as_list.Count; j++)
            {
                as_dataGridView.Rows.Add(as_list[j].ToArray());
            }

        }
        private void store_view_button_Click(object sender, EventArgs e)
        {
            InitializeDataGrid_equ();   // 데이터그리드 초기화

            List<List<string>> store_list = create_db_list("store");      // 이중 리스트를 선언하여 함수 리턴값 저장

            create_store_DataGridView(store_list); // 데이터그리드 그리기  
        }


        private void equ_add_button_Click(object sender, EventArgs e)
        {
            server = "192.168.31.147";
            //server = "127.0.0.1";
            database = "team2";
            uid = "root";
            password = "0000";

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connectionString); //커넥션 객체 만들기

            if (string.IsNullOrWhiteSpace(equ_add_textBox_num.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_mnum.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_branch.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_mname.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_amount.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_my.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_insdate.Text) ||
                string.IsNullOrWhiteSpace(equ_add_textBox_date.Text))
            {
                MessageBox.Show("비어있는 정보가 있습니다.\n모든 정보를 입력해주세요.", "경고");
                return;
            }

            try
            {
                string insertQuery = $"INSERT INTO Equipment (number, enum, branch, mname, amount, my, insdate, date, name) " +
                     $"VALUES ('{equ_add_textBox_num.Text}', '{equ_add_textBox_mnum.Text}', " +
                     $"'{equ_add_textBox_branch.Text}', '{equ_add_textBox_mname.Text}', " +
                     $"'{equ_add_textBox_amount.Text}', '{equ_add_textBox_my.Text}', " +
                     $"'{equ_add_textBox_insdate.Text}', '{equ_add_textBox_date.Text}', 'Unknown')";




                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                // Clear input fields
                equ_add_textBox_num.Text = "";
                equ_add_textBox_mnum.Text = "";
                equ_add_textBox_branch.Text = "";
                equ_add_textBox_mname.Text = "";
                equ_add_textBox_amount.Text = "";
                equ_add_textBox_my.Text = "";
                equ_add_textBox_insdate.Text = "";
                equ_add_textBox_date.Text = "";

                MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"저장 중 오류가 발생했습니다: {ex.Message}");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }



        private void as_add_button_Click(object sender, EventArgs e)
        {
            server = "192.168.31.147";
            //server = "127.0.0.1";
            database = "team2";
            uid = "root";
            password = "0000";

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connectionString); //커넥션 객체 만들기

            if (string.IsNullOrWhiteSpace(as_num_textBox.Text) ||
                string.IsNullOrWhiteSpace(as_ename_textBox.Text) ||
                string.IsNullOrWhiteSpace(as_enum_textBox.Text) ||
                string.IsNullOrWhiteSpace(as_branch_textBox.Text) ||
                string.IsNullOrWhiteSpace(as_date_textBox.Text) ||
                string.IsNullOrWhiteSpace(as_significant_textBox.Text))

            {
                MessageBox.Show("비어있는 정보가 있습니다.\n모든 정보를 입력해주세요.", "경고");
                return;
            }

            try
            {
                string insertQuery = $"INSERT INTO AStable (number, ename, enum, branch, reason, date, significant) " +
                     $"VALUES ('{as_num_textBox.Text}', '{as_ename_textBox.Text}', " +
                     $"'{as_enum_textBox.Text}', '{as_branch_textBox.Text}', " +
                     $"'{as_reason_textBox.Text}'," +
                     $"'{as_date_textBox.Text}'," +
                     $"'{as_significant_textBox.Text}')";


                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                // Clear input fields
                as_num_textBox.Text = "";
                as_ename_textBox.Text = "";
                as_enum_textBox.Text = "";
                as_branch_textBox.Text = "";
                as_date_textBox.Text = "";
                as_significant_textBox.Text = "";
                as_reason_textBox.Text = "";

                MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"저장 중 오류가 발생했습니다: {ex.Message}");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


        private void create_store_DataGridView(List<List<string>> store_list)        // 데이터 그리드를 그리는 함수
        {
            for (int j = 0; j < store_list.Count; j++)
            {
                store_dataGridView.Rows.Add(store_list[j].ToArray());
            }

        }


        private void store_add_button_Click(object sender, EventArgs e)
        {
            server = "192.168.31.147";
            //server = "127.0.0.1";
            database = "team2";
            uid = "root";
            password = "0000";

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connectionString); //커넥션 객체 만들기

            if (string.IsNullOrWhiteSpace(store_num_textBox.Text) ||
                string.IsNullOrWhiteSpace(store_branch_textBox.Text) ||
                string.IsNullOrWhiteSpace(store_loc_textBox.Text) ||
                string.IsNullOrWhiteSpace(store_name_textBox.Text) ||
                string.IsNullOrWhiteSpace(store_phone_textBox.Text) ||
                string.IsNullOrWhiteSpace(store_hour_textBox.Text))

            {
                MessageBox.Show("비어있는 정보가 있습니다.\n모든 정보를 입력해주세요.", "경고");
                return;
            }

            try
            {
                string insertQuery = $"INSERT INTO store (number, branch, location, name, phone, hour) " +
                     $"VALUES ('{store_num_textBox.Text}', '{store_branch_textBox.Text}', " +
                     $"'{store_branch_textBox.Text}', '{store_name_textBox.Text}', " +
                     $"'{store_phone_textBox.Text}', '{store_hour_textBox.Text}')";


                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                // Clear input fields
                store_num_textBox.Text = "";
                store_branch_textBox.Text = "";
                store_branch_textBox.Text = "";
                store_name_textBox.Text = "";
                store_phone_textBox.Text = "";
                store_hour_textBox.Text = "";

                MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"저장 중 오류가 발생했습니다: {ex.Message}");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public List<string> equ_selected_code = new List<string>();

        public List<string> as_selected_code = new List<string>();

        public List<string> store_selected_code = new List<string>();

        private void equ_dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            equ_dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private static List<List<string>> equipment_list = new List<List<string>>();

        public static List<List<string>> equipment_list_property
        {
            get { return equipment_list; }
            set { equipment_list = value; }
        }

        private void as_dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            as_dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private static List<List<string>> as_list = new List<List<string>>();

        public static List<List<string>> as_list_property
        {
            get { return as_list; }
            set { as_list = value; }
        }

        private void store_dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            store_dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private static List<List<string>> store_list = new List<List<string>>();

        public static List<List<string>> store_list_property
        {
            get { return store_list; }
            set { store_list = value; }
        }

        public static List<List<string>> create_equipment()
        {
            string filePath = "equ_list.csv";
            string[] lines = File.ReadAllLines(filePath, Encoding.Default);

            List<List<string>> equipment_list_temp = new List<List<string>>();

            foreach (string line in lines)
            {
                List<string> equipmentData = line.Split(',').ToList();
                equipment_list_temp.Add(equipmentData);
            }

            return equipment_list_temp;
        }

        public static List<List<string>> create_as()
        {

            string filePath = "as_list.csv";      // 수정 필요
            string[] lines = File.ReadAllLines(filePath, Encoding.Default);

            List<List<string>> as_list_temp = new List<List<string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                List<string> asData = lines[i].Split(',').ToList();
                as_list_temp.Add(asData);
            }

            return as_list_temp;
        }
        public static List<List<string>> create_store()
        {

            string filePath = "store_list.csv";      // 수정 필요
            string[] lines = File.ReadAllLines(filePath, Encoding.Default);

            List<List<string>> store_list_temp = new List<List<string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                List<string> storeData = lines[i].Split(',').ToList();
                store_list_temp.Add(storeData);
            }

            return store_list_temp;
        }


        //public List<string> selected_equipment = new List<string>();
        //public List<string> selected_as = new List<string>();
        //public List<string> selected_store = new List<string>();

        private void equ_dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && equ_dataGridView.Columns[e.ColumnIndex].Name == "equ_dataGrid_checkBox")     // 선택한 셀의 헤더 이름이 equ_dataGrid_checkBox 라면
            {
                if (Convert.ToBoolean(equ_dataGridView.Rows[e.RowIndex].Cells["equ_dataGrid_checkBox"].Value))  // 체크박스의 값이 True 인지 검사
                {
                    equ_dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    equ_selected_code.Add(equ_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
                else
                {
                    equ_dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    equ_selected_code.Remove(equ_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
            }
        }
        private void as_dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && as_dataGridView.Columns[e.ColumnIndex].Name == "as_datagrid_checkbox")     // 선택한 셀의 헤더 이름이 store_dataGrid_checkBox 라면
            {
                if (Convert.ToBoolean(as_dataGridView.Rows[e.RowIndex].Cells["as_datagrid_checkbox"].Value))  // 체크박스의 값이 True 인지 검사
                {
                    as_dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    as_selected_code.Add(as_dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
                else
                {
                    as_dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    as_selected_code.Remove(as_dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
            }
        }
        private void store_dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && store_dataGridView.Columns[e.ColumnIndex].Name == "store_dataGridView_checkBox")     // 선택한 셀의 헤더 이름이 store_dataGrid_checkBox 라면
            {
                if (Convert.ToBoolean(store_dataGridView.Rows[e.RowIndex].Cells["store_dataGridView_checkBox"].Value))  // 체크박스의 값이 True 인지 검사
                {
                    store_dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    store_selected_code.Add(store_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
                else
                {
                    store_dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    store_selected_code.Remove(store_dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
            }
        }


        public static void create_equ_DataGridView(List<List<string>> equipment_list, DataGridView equ_dataGridView)        // 데이터 그리드를 그리는 함수
        {
            equ_dataGridView.Rows.Clear();

            List<List<string>> add_equ_dataGridView_list = new List<List<string>>();
            for (int equ1 = 0; equ1 < equipment_list.Count; equ1++)
            {
                List<string> temp_equ_list = new List<string>() {        // 제목행 설정
                    equipment_list[equ1][0],                     // 번호
                    equipment_list[equ1][1],                     // 장비명
                    equipment_list[equ1][2],                     // 갯수
                    equipment_list[equ1][3],                     // 최근 점검일
                    equipment_list[equ1][4],                     // 들여온 날짜
                    equipment_list[equ1][5],                     // 연식
                };

                add_equ_dataGridView_list.Add(temp_equ_list);
            }

            for (int j = 1; j < equipment_list.Count; j++)
            {
                equ_dataGridView.Rows.Add(add_equ_dataGridView_list[j].ToArray());
            }


            // 첫번째 열, 마지막 행 제거
            equ_dataGridView.RowHeadersVisible = false; //
            equ_dataGridView.AllowUserToAddRows = false;
            equ_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //선택 되는 함수

            equ_dataGridView.AllowUserToResizeColumns = false;
            equ_dataGridView.AllowUserToResizeRows = false;

        }

        public static void create_as_DataGridView(List<List<string>> as_list, DataGridView as_dataGridView)        // 데이터 그리드를 그리는 함수
        {
            as_dataGridView.Rows.Clear();

            List<List<string>> add_as_dataGridView_list = new List<List<string>>();
            for (int as1 = 0; as1 < equipment_list.Count; as1++)
            {
                List<string> temp_as_list = new List<string>() {        // 제목행 설정
                    as_list[as1][0],                     // 번호
                    as_list[as1][1],                     // 장비명
                    as_list[as1][2],                     // 사유
                    as_list[as1][3],                     // 등록일
                };

                add_as_dataGridView_list.Add(temp_as_list);
            }

            for (int j = 1; j < as_list.Count; j++)
            {
                as_dataGridView.Rows.Add(add_as_dataGridView_list[j].ToArray());
            }


            // 첫번째 열, 마지막 행 제거
            as_dataGridView.RowHeadersVisible = false; //
            as_dataGridView.AllowUserToAddRows = false;
            as_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //선택 되는 함수

            as_dataGridView.AllowUserToResizeColumns = false;
            as_dataGridView.AllowUserToResizeRows = false;

        }



        private void as_add_reset_Click(object sender, EventArgs e)
        {
            ResetControls(as_num_textbox_panel);
            ResetControls(as_ename_textbox_panel);
            ResetControls(as_sname_textbox_panel);
            ResetControls(as_name_textbox_panel);
            ResetControls(as_reason_textbox_panel);
            ResetControls(as_date_textbox_panel);
            ResetControls(as_significant_textbox_panel);
        }

        private void equ_search_reset_button_Click(object sender, EventArgs e)
        {
            ResetControls(equ_search_num_panel2);
            ResetControls(equ_search_mnum_panel2);
            ResetControls(equ_search_amount_panel2);
            ResetControls(equ_search_date_panel2);
            ResetControls(equ_search_insdate_panel2);
            ResetControls(equ_search_mname_panel2);
            ResetControls(equ_search_my_panel2);
            ResetControls(equ_search_name_panel2);
        }

        private void store_add_reset_button_Click(object sender, EventArgs e)
        {
            ResetControls(store_table);
        }

        private void as_firstq_button_Click(object sender, EventArgs e)
        {
            FilterByQuarter(1);

        }

        private void as_secondq_button_Click(object sender, EventArgs e)
        {
            FilterByQuarter(4);

        }

        private void as_thirdq_button_Click(object sender, EventArgs e)
        {
            FilterByQuarter(7);

        }

        private void as_fourthq_button_Click(object sender, EventArgs e)
        {
            FilterByQuarter(10);

        }
        private void FilterByQuarter(int startMonth)
        {
            as_dataGridView.Rows.Clear();

            List<List<string>> as_list = create_as();

            foreach (List<string> row in as_list)
            {
                string[] dateParts = row[5].Split('-'); // 등록일이 6번째 열에 있다고 가정
                if (dateParts.Length >= 2) // 등록일이 "YYYY-MM-DD" 형식이라는 가정하에 월 정보만 추출
                {
                    int month = int.Parse(dateParts[1]);

                    // 해당 분기에 속하는지 확인
                    if (month >= startMonth && month <= startMonth + 2)
                    {
                        as_dataGridView.Rows.Add(row.ToArray());
                    }
                }
            }

            if (as_dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("해당 분기에 등록된 데이터가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void as_up_button_Click(object sender, EventArgs e)
        {
            FilterByHalfYear(true);
        }

        private void as_down_button_Click(object sender, EventArgs e)
        {
            FilterByHalfYear(false);
        }
        private void FilterByHalfYear(bool isFirstHalf)
        {
            as_dataGridView.Rows.Clear();

            List<List<string>> as_list = create_as();

            foreach (List<string> row in as_list)
            {
                string[] dateParts = row[5].Split('-'); // 등록일이 6번째 열에 있다고 가정
                if (dateParts.Length >= 2) // 등록일이 "YYYY-MM-DD" 형식이라는 가정하에 월 정보만 추출
                {
                    int month = int.Parse(dateParts[1]);

                    // 상반기인 경우 1월부터 6월까지, 하반기인 경우 7월부터 12월까지 행을 표시합니다.
                    if (isFirstHalf && month >= 1 && month <= 6)
                    {
                        as_dataGridView.Rows.Add(row.ToArray());
                    }
                    else if (!isFirstHalf && month >= 7 && month <= 12)
                    {
                        as_dataGridView.Rows.Add(row.ToArray());
                    }
                }
            }

            if (as_dataGridView.Rows.Count == 0)
            {
                MessageBox.Show(isFirstHalf ? "상반기에 등록된 데이터가 없습니다." : "하반기에 등록된 데이터가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void store_delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    foreach (string ssc in store_selected_code)
                    {
                        DeleteStoreData("store", "branch", ssc);
                    }
                    conn.Close();
                    store_view_button_Click(sender, e);
                    store_selected_code.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void equ_delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    foreach (string ssc in equ_selected_code)
                    {
                        DeleteStoreData("equipment", "enum", ssc);
                    }
                    conn.Close();
                    equ_view_button_Click(sender, e);
                    equ_selected_code.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //foreach (DataGridViewRow row in equ_dataGridView.Rows)
            //{
            //    try
            //    {
            //        conn = new MySqlConnection(connectionString);
            //        if (make_connection())
            //        {
            //            foreach (string ssc in store_selected_code)
            //            {
            //                DeleteStoreData("store", "branch", ssc);
            //            }
            //            conn.Close();
            //            store_view_button_Click(sender, e);
            //            store_selected_code.Clear();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());
            //    }
            //}
        }

        private void as_delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    foreach (string ssc in as_selected_code)
                    {
                        DeleteStoreData("astable", "number", ssc);
                    }
                    conn.Close();
                    as_view_button_Click(sender, e);
                    as_selected_code.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //foreach (DataGridViewRow row in as_dataGridView.Rows)
            //{
            //    try
            //    {
            //        if (row.Cells["as_datagrid_checkbox"].Value != null && (bool)row.Cells["as_datagrid_checkbox"].Value)
            //        {
            //            // 선택된 행의 데이터를 파일에서도 삭제
            //            //MessageBox.Show(row.Cells[1].Value.ToString());
            //            DeleteStoreData("astable", "number", row.Cells[0].Value.ToString()); // 선택된 행의 첫 번째 열의 데이터를 삭제 함수에 전달
            //            as_dataGridView.Rows.Remove(row);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());
            //    }
            //}
        }


        private void DeleteStoreData(string table_name, string pk, string branch)
        {

            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    string remove_store_query = $"delete from team2.{table_name} where {pk}='{branch}'";
                    MySqlCommand cmd = new MySqlCommand(remove_store_query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                //MessageBox.Show("전송 성공");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //private void DeleteASData(string code)
        //{
        //    string filePath = "as_list.csv";
        //    List<string> lines = File.ReadAllLines(filePath, Encoding.Default).ToList();

        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        if (lines[i].Split(',')[0] == code) // 선택된 코드와 일치하는 행을 찾으면
        //        {
        //            lines.RemoveAt(i); // 해당 행을 제거
        //            break;
        //        }
        //    }

        //    // 파일을 다시 쓰기 모드로 열고 삭제된 데이터를 기록
        //    File.WriteAllLines(filePath, lines.ToArray(), Encoding.Default);
        //}


        //private void DeleteEquipmentData(string code)
        //{
        //    string filePath = " equ_list.csv";
        //    List<string> lines = File.ReadAllLines(filePath, Encoding.Default).ToList();

        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        if (lines[i].Split(',')[0] == code) // 선택된 코드와 일치하는 행을 찾으면
        //        {
        //            lines.RemoveAt(i); // 해당 행을 제거
        //            break;
        //        }
        //    }

        //    // 파일을 다시 쓰기 모드로 열고 삭제된 데이터를 기록
        //    File.WriteAllLines(filePath, lines.ToArray(), Encoding.Default);
        //}





















        // 김자연
        private void SetReadOnlyColumns()
        {
            for (int i = 0; i < 9; i++)
            {
                dataGridView_st_list.Columns[i].ReadOnly = true;
            }
            for (int i = 0; i < 9; i++)
            {
                if (i != 3)
                {
                    dataGridView_st_bsk.Columns[i].ReadOnly = true;
                }

            }
        }

        // List<List<string>> 형태의 데이터를 textBox에 표시하는 메서드
        //private void ShowDataInTextBox(List<List<string>> data, System.Windows.Forms.TextBox textBox)
        //{
        //    // 텍스트 박스 내용을 초기화합니다.
        //    textBox.Clear();

        //    // 데이터를 텍스트 박스에 추가합니다.
        //    foreach (List<string> rowData in data)
        //    {
        //        foreach (string cellData in rowData)
        //        {
        //            textBox.AppendText(cellData + "\t"); // 탭으로 구분하여 표시합니다.
        //        }
        //        textBox.AppendText(Environment.NewLine); // 새로운 줄로 이동합니다.
        //    }
        //}
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ToggleCalendarVisibility(System.Windows.Forms.MonthCalendar calendar, System.Windows.Forms.TextBox textBox)
        {
            if (!calendar.Visible)
            {
                calendar.Visible = true;
                calendar.BringToFront(); // 달력을 폼의 가장 앞으로 가져옴
            }
            else
            {
                calendar.Visible = false;
            }
        }

        private void button_st_apply_day_Click(object sender, EventArgs e)
        {
            ToggleCalendarVisibility(Calendar_st_apply_day, textBox_st_apply_day);
        }
        private void button_st_apply_delivery_Click(object sender, EventArgs e)
        {
            ToggleCalendarVisibility(Calendar_st_apply_delivery, textBox_st_apply_delivery);
        }
        private void buttonl_st_history_day_Click(object sender, EventArgs e)
        {
            ToggleCalendarVisibility(Calendar_st_history_day, textBox_st_history_day);
        }

        private void button_st_history_delivery_Click(object sender, EventArgs e)
        {
            ToggleCalendarVisibility(Calendar_st_history_delivery, textBox_st_history_day1);
        }
        private void UpdateTextBoxFromCalendar(DateRangeEventArgs e, System.Windows.Forms.TextBox textBox, MonthCalendar calendar)
        {
            Console.WriteLine("Selected Date: " + e.Start.ToShortDateString());
            textBox.Text = e.Start.ToShortDateString(); // 선택된 날짜를 textBox에 입력

            calendar.Visible = false; // Calendar를 숨김
        }

        private void Calendar_st_apply_day_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateTextBoxFromCalendar(e, textBox_st_apply_day, Calendar_st_apply_day);
        }

        private void Calendar_st_apply_delivery_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateTextBoxFromCalendar(e, textBox_st_apply_delivery, Calendar_st_apply_delivery);
        }

        private void Calendar_st_history_day_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateTextBoxFromCalendar(e, textBox_st_history_day, Calendar_st_history_day);
        }

        private void Calendar_st_history_delivery_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateTextBoxFromCalendar(e, textBox_st_history_day1, Calendar_st_history_delivery);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //데이터그리드뷰1
        private void LoadDataToDataGridView()
        {
            string filePath = "stockdata.csv"; // CSV 파일 경로           
            string[] lines = File.ReadAllLines(filePath, Encoding.Default);

            dataGridView_st_list.DataSource = null; // DataGridView 초기화
            dataGridView_st_list.Columns.Clear();
            dataGridView_st_bsk.DataSource = null;
            dataGridView_st_bsk.Columns.Clear();



            if (lines.Length > 0) // 첫 번째 줄을 헤더로 사용하여 열을 추가
            {
                string[] headers = lines[0].Split(',');
                foreach (string header in headers)
                {
                    dataGridView_st_list.Columns.Add(header, header);
                    dataGridView_st_bsk.Columns.Add(header, header);  // dataGridView2에 열 추가

                }
            }



            try
            {
                List<List<string>> stock_list = create_db_list("stock_data");
                foreach(List<string> st_list in stock_list)
                {
                    string[] temp = new string[9];
                    for (int i=0; i < st_list.Count; i++)
                    {
                        temp[i] = st_list[i];
                    }
                    dataGridView_st_list.Rows.Add(temp);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //for (int i = 1; i < lines.Length; i++) // 나머지 줄을 데이터로 사용하여 행을 추가
            //{
            //    string[] data = lines[i].Split(',');
            //    dataGridView_st_list.Rows.Add(data);
            //}





            DataGridViewCheckBoxColumn approvalColumn = new DataGridViewCheckBoxColumn();
            approvalColumn.HeaderText = "승인";
            approvalColumn.Name = "Approval";
            dataGridView_st_list.Columns.Add(approvalColumn);

            DataGridViewCheckBoxColumn approvalColumn2 = new DataGridViewCheckBoxColumn();
            approvalColumn2.HeaderText = "승인";
            approvalColumn2.Name = "Approval_bsk";
            dataGridView_st_bsk.Columns.Add(approvalColumn2);

            dataGridView_st_list.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders; // 행 헤더의 너비 설정

            int columnCount = dataGridView_st_list.Columns.Count; // 열의 개수
            int totalColumnWidth = dataGridView_st_list.ClientSize.Width - 44; // 열의 너비 비율
            int columnWidth = totalColumnWidth / columnCount;

            foreach (DataGridViewColumn column in dataGridView_st_list.Columns) // 각 열의 너비 설정
            {
                column.Width = columnWidth;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 각 열을 가운데 정렬로 설정
                if (column.Name == "Approval") // "Approval" 열에 대해서만 추가 설정
                {
                    DataGridViewCheckBoxColumn chkColumn = column as DataGridViewCheckBoxColumn;
                    if (chkColumn != null)
                    {
                        chkColumn.TrueValue = true;
                        chkColumn.FalseValue = false;
                    }
                }
            }
            foreach (DataGridViewColumn column in dataGridView_st_bsk.Columns)
            {
                column.Width = columnWidth;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 각 열을 가운데 정렬로 설정
            }

            // 승인 열 너비 설정
            SetColumnWidth(dataGridView_st_list.Columns["Approval"]);
            SetColumnWidth(dataGridView_st_bsk.Columns["Approval_bsk"]);
        }

        private void dataGridView_st_list_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 변경된 셀이 "Approval" 열인지 확인하고, 체크된 상태인지 확인tobox
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView_st_list.Columns[e.ColumnIndex].Name == "Approval")
            {
                DataGridViewCheckBoxCell chk = dataGridView_st_list.Rows[e.RowIndex].Cells["Approval"] as DataGridViewCheckBoxCell;

                if (chk != null && chk.Value != null && (bool)chk.Value)
                {
                    // 체크된 행의 데이터를 데이터그리드뷰2에 추가
                    AddRowToBasket(dataGridView_st_list.Rows[e.RowIndex]);
                }
                else
                {
                    // 체크가 해제된 경우, DataGridVie
                    //
                    // w2에서 해당 행 삭제
                    RemoveRowFromBasket(dataGridView_st_bsk, dataGridView_st_list, dataGridView_st_list.Rows[e.RowIndex]);
                }
            }
        }

        private void dataGridView_st_bsk_CellValueChanged2(object sender, DataGridViewCellEventArgs e)
        {
            // 변경된 셀이 "Approval_bsk" 열인지 확인하고, 체크된 상태인지 확인
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView_st_bsk.Columns[e.ColumnIndex].Name == "Approval_bsk")
            {
                DataGridViewCheckBoxCell chk = dataGridView_st_bsk.Rows[e.RowIndex].Cells["Approval_bsk"] as DataGridViewCheckBoxCell;

                if (chk != null && chk.Value != null && !(bool)chk.Value)
                {
                    // 체크가 해제된 경우, DataGridView2에서 해당 행 삭제
                    RemoveRowFromBasket(dataGridView_st_bsk, dataGridView_st_list, dataGridView_st_list.Rows[e.RowIndex]);
                }
            }
        }

        private void dataGridView_st_list_StateChanged(object sender, EventArgs e)
        {
            // 현재 셀이 체크 박스 셀이고 Dirty한지 확인
            if (dataGridView_st_list.IsCurrentCellDirty)
            {
                // 변경 사항을 즉시 커밋하여 셀 값이 변경되도록 함
                dataGridView_st_list.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void dataGridView_st_bsk_StateChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 변경된 셀이 "Approval_bsk" 열인지 확인하고, 체크된 상태인지 확인
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView_st_bsk.Columns[e.ColumnIndex].Name == "Approval_bsk")
            {
                DataGridViewCheckBoxCell chk = dataGridView_st_bsk.Rows[e.RowIndex].Cells["Approval_bsk"] as DataGridViewCheckBoxCell;

                if (chk != null && chk.Value != null && !(bool)chk.Value)
                {
                    // 체크가 해제된 경우, DataGridView2에서 해당 행 삭제
                    RemoveRowFromBasket(dataGridView_st_bsk, dataGridView_st_list, dataGridView_st_list.Rows[e.RowIndex]);
                }
            }
        }

        private void dataGridView_st_list_DirtyStateChanged(object sender, EventArgs e)
        {
            // 현재 셀이 체크 박스 셀인지 확인
            if (dataGridView_st_list.IsCurrentCellDirty)
            {
                // 변경 사항을 즉시 커밋하여 셀 값이 변경되도록 함
                dataGridView_st_list.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void AddRowToBasket(DataGridViewRow row)
        {
            // 체크된 행의 데이터를 데이터그리드뷰2에 추가
            DataGridViewRow newRow = (DataGridViewRow)row.Clone(); // 새로운 행 생성
            newRow.Cells[0].Value = row.Cells[0].Value; // 첫 번째 열의 값을 복사

            // 나머지 열에 데이터 복사
            for (int i = 1; i < row.Cells.Count; i++)
            {
                newRow.Cells[i].Value = row.Cells[i].Value;
            }

            // 승인 상태 업데이트
            DataGridViewCheckBoxCell chk = row.Cells["Approval"] as DataGridViewCheckBoxCell;
            if (chk != null && chk.Value != null && (bool)chk.Value)
            {
                // "Approval_bsk" 열의 셀을 찾아서 값을 설정합니다.
                int index = dataGridView_st_bsk.Columns["Approval_bsk"].Index;
                newRow.Cells[index].Value = true;
            }

            // 새로운 행을 DataGridView2에 추가
            dataGridView_st_bsk.Rows.Add(newRow);
        }

        private void RemoveRowFromBasket(DataGridView dataGridView_st_bsk, DataGridView dataGridView_st_list, DataGridViewRow row)
        {/*
            // DataGridView2에서 해당 행 삭제
            foreach (DataGridViewRow basketRow in dataGridView_st_bsk.Rows)
            {
                // "Approval_bsk" 열에 체크박스가 있는지 확인
                if (basketRow.Cells["Approval_bsk"] is DataGridViewCheckBoxCell)
                {
                    if (basketRow.Cells["Approval_bsk"].Value != null && row.Cells["Approval_bsk"].Value != null)
                    {
                        if (basketRow.Cells["Approval_bsk"].Value.ToString() == row.Cells["Approval_bsk"].Value.ToString())
                        {
                            dataGridView_st_bsk.Rows.Remove(basketRow);

                            // dataGridView_st_list에서 해당 행의 체크 박스 상태를 해제
                            dataGridView_st_list.Rows[row.Index].Cells["Approval"].Value = false;

                            break;
                        }
                    }
                }
            }
            */
        }

        private void SetColumnWidth(DataGridViewColumn column)
        {
            int totalColumnWidth = dataGridView_st_list.ClientSize.Width - SystemInformation.VerticalScrollBarWidth; // 승인 열의 너비 비율 계산
            int columnWidth = totalColumnWidth / dataGridView_st_list.Columns.Count;

            column.Width = columnWidth; // 승인 열의 너비 설정
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //품목 등록
        private void button_st_register_Click(object sender, EventArgs e)
        {
            //try
            //{

            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            // 유효성 검사를 수행합니다.
            if (!ValidateInputs())
            {
                return;
            }

            // 유효성 검사를 통과한 경우 데이터를 추가합니다.
            string data1 = textBox_st_apply2_class.Text;
            string data2 = textBox_st_apply2_code.Text;
            string data3 = textBox_st_apply2_name.Text;
            int data4 = int.Parse(textBox_st_apply2_amt.Text);
            string data5 = textBox_st_apply2_unit.Text;
            int data6 = int.Parse(textBox_st_apply2_price.Text);
            float data7 = data4 * data6;
            string data8 = textBox_st_apply2_acc.Text;
            string data9 = comboBox_st_apply2_status.SelectedItem?.ToString() ?? "";


            // 데이터를 로드하여 데이터 그리드 뷰에 표시합니다.
            LoadDataToDataGridView();

            // 텍스트박스를 초기화합니다.
            ClearTextBoxes();
        }


        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "검색 결과 없음. 검색어를 바르게 입력했는지 다시 확인하세요.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearTextBoxes()
        {
            // 텍스트박스를 초기화합니다.
            textBox_st_apply2_class.Text = "";
            textBox_st_apply2_code.Text = "";
            textBox_st_apply2_name.Text = "";
            textBox_st_apply2_amt.Text = "";
            textBox_st_apply2_unit.Text = "";
            textBox_st_apply2_price.Text = "";
            textBox_st_apply2_acc.Text = "";
        }
        private bool ValidateInputs()
        {
            /*
            List<string> errorMessages = new List<string>(); // 오류 메시지를 저장할 리스트 생성

            int data4;
            if (!int.TryParse(textBox_st_apply2_amt.Text, out data4))
            {
                textBox_st_apply2_amt.BackColor = Color.Yellow;
                errorMessages.Add("<재고 수량>에서 숫자를 입력하세요."); // 오류 메시지 추가
            }
            else
            {
                textBox_st_apply2_amt.BackColor = SystemColors.Window; // 배경색 원래대로 돌리기
            }

            int data6;
            if (!int.TryParse(textBox_st_apply2_price.Text, out data6))
            {
                textBox_st_apply2_price.BackColor = Color.Yellow;
                errorMessages.Add("<단가>에서 숫자를 입력하세요."); // 오류 메시지 추가
            }
            else
            {
                textBox_st_apply2_price.BackColor = SystemColors.Window; // 배경색 원래대로 돌리기
            }

            if (comboBox_st_apply2_status.SelectedItem == null)
            {
                errorMessages.Add("<등록상태>에서 항목을 선택하세요."); // 오류 메시지 추가
            }

            if (errorMessages.Count > 0)
            {

                MessageBox.Show(string.Join(Environment.NewLine, errorMessages)); // 모든 오류 메시지를 한 번에 표시
                return false; // 유효성 검사 실패
            }

            return true; // 유효성 검사 성공
            */
            // 텍스트박스에 입력된 데이터의 유효성을 검사합니다.
            List<string> errorMessages = new List<string>();

            // 각 텍스트박스의 데이터를 유효성 검사합니다.
            if (!int.TryParse(textBox_st_apply2_amt.Text, out int data4))
            {
                textBox_st_apply2_amt.BackColor = Color.Yellow;
                errorMessages.Add("<재고 수량>에서 숫자를 입력하세요.");
            }
            else
            {
                textBox_st_apply2_amt.BackColor = SystemColors.Window;
            }

            if (!int.TryParse(textBox_st_apply2_price.Text, out int data6))
            {
                textBox_st_apply2_price.BackColor = Color.Yellow;
                errorMessages.Add("<단가>에서 숫자를 입력하세요.");
            }
            else
            {
                textBox_st_apply2_price.BackColor = SystemColors.Window;
            }

            if (comboBox_st_apply2_status.SelectedItem == null)
            {
                errorMessages.Add("<등록상태>에서 항목을 선택하세요.");
            }


            // 유효성 검사 결과에 따라 메시지를 표시합니다.
            if (errorMessages.Count > 0)
            {
                ShowErrorMessage(string.Join(Environment.NewLine, errorMessages));
                return false;
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //항목 검색
        private void button_st_list_class_Click(object sender, EventArgs e)
        {
            string searchText = textBox_st_list_class.Text.Trim(); // 텍스트박스에서 입력된 텍스트 가져오기
            if (string.IsNullOrWhiteSpace(searchText)) // 입력된 텍스트가 없으면 함수 종료
            {
                return;
            }

            // DataGridView1의 모든 행을 검사하여 "분류" 열의 값이 searchText와 일치하는 행을 찾음
            List<DataGridViewRow> matchedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dataGridView_st_list.Rows)
            {
                if (row.Cells["분류"].Value != null && row.Cells["분류"].Value.ToString().Equals(searchText))
                {
                    matchedRows.Add(row); // 일치하는 행을 리스트에 추가
                }
            }

            // 검색 결과가 없으면 메시지를 표시하고 함수 종료
            if (matchedRows.Count == 0)
            {
                MessageBox.Show("분류 열에서 '" + searchText + "'를 찾을 수 없습니다.");
                return;
            }

            // 검색 결과가 있으면 DataGridView1의 순서를 변경하여 일치하는 행들을 위로 정렬
            // 동일한 분류명이 여러 개일 경우, 실제 데이터 순서에 맞는 첫 번째 행을 찾음
            DataGridViewRow firstMatchedRow = null;
            int minRowIndex = int.MaxValue;
            foreach (DataGridViewRow row in matchedRows)
            {
                if (row.Index < minRowIndex)
                {
                    minRowIndex = row.Index;
                    firstMatchedRow = row;
                }
            }

            // 검색 결과의 첫 번째 행을 선택표시
            if (firstMatchedRow != null)
            {
                dataGridView_st_list.ClearSelection(); // 선택 해제
                firstMatchedRow.Selected = true; // 첫 번째 일치하는 행 선택표시
                dataGridView_st_list.FirstDisplayedScrollingRowIndex = firstMatchedRow.Index; // 선택된 행이 보여지도록 스크롤 조정
            }
        }
        private void textBox_st_list_code_TextChanged(object sender, EventArgs e)
        {

        }

        private void FilterDataGridView(string searchText, string columnName, DataGridView dataGridView)
        {
            // 데이터 그리드 뷰의 데이터를 필터링합니다.
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView.DataSource;
            bs.Filter = string.Format("{0} LIKE '%{1}%'", columnName, searchText);
            dataGridView.DataSource = bs;
        }

        private void textBox_st_list_name_TextChanged(object sender, EventArgs e)
        {

        }
        private void ReloadDataToDataGridView()
        {
            string filePath = "stockdata.csv"; // CSV 파일 경로           
            string[] lines = File.ReadAllLines(filePath, Encoding.Default);

            dataGridView_st_list.DataSource = null; // DataGridView 초기화
            dataGridView_st_list.Columns.Clear();

            if (lines.Length > 0) // 첫 번째 줄을 헤더로 사용하여 열을 추가
            {
                string[] headers = lines[0].Split(',');
                foreach (string header in headers)
                {
                    dataGridView_st_list.Columns.Add(header, header);

                }
            }

            for (int i = 1; i < lines.Length; i++) // 나머지 줄을 데이터로 사용하여 행을 추가
            {
                string[] data = lines[i].Split(',');
                dataGridView_st_list.Rows.Add(data);
            }

            DataGridViewCheckBoxColumn approvalColumn = new DataGridViewCheckBoxColumn();
            approvalColumn.HeaderText = "승인";
            approvalColumn.Name = "Approval";
            dataGridView_st_list.Columns.Add(approvalColumn);

            dataGridView_st_list.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders; // 행 헤더의 너비 설정

            int columnCount = dataGridView_st_list.Columns.Count; // 열의 개수
            int totalColumnWidth = dataGridView_st_list.ClientSize.Width - 44; // 열의 너비 비율
            int columnWidth = totalColumnWidth / columnCount;

            foreach (DataGridViewColumn column in dataGridView_st_list.Columns) // 각 열의 너비 설정
            {
                column.Width = columnWidth;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 각 열을 가운데 정렬로 설정
                if (column.Name == "Approval") // "Approval" 열에 대해서만 추가 설정
                {
                    DataGridViewCheckBoxColumn chkColumn = column as DataGridViewCheckBoxColumn;
                    if (chkColumn != null)
                    {
                        chkColumn.TrueValue = true;
                        chkColumn.FalseValue = false;
                    }
                }
            }

            // 승인 열 너비 설정
            SetColumnWidth(dataGridView_st_list.Columns["Approval"]);

        }
        private void textBox_st_apply_mng_TextChanged(object sender, EventArgs e)
        {
            // 검색어를 가져옴
            string searchText = textBox_st_apply_mng.Text.Trim();

            // 검색 결과를 얻기 위해 search_employee 메서드를 호출
            List<List<string>> search_result_ep_list = Employee.employee_detail_property;

            // 추출된 데이터를 이용하여 추천 목록 생성
            List<string> recommendationList = new List<string>();


            for (int i = 1; i < search_result_ep_list.Count; i++)
            {
                List<string> el = search_result_ep_list[i];
                string recommendation = $"{el[10]} - {el[2]}"; // 이름 - 소속 형식으로 추천 항목 생성
                recommendationList.Add(recommendation);
            }

            // 추천 목록을 사용자에게 표시
            ShowRecommendationList(recommendationList, listBox_st_apply_mng);
        }


        private void listBox_st_list_code_DoubleClick(object sender, EventArgs e)
        {
            // 리스트박스에서 더블 클릭된 항목을 텍스트박스에 반영
            if (listBox_st_list_code.SelectedItem != null)
            {
                textBox_st_list_code.Text = listBox_st_list_code.SelectedItem.ToString();
                listBox_st_list_code.Visible = false;
            }
        }

        private void listBox_st_list_name_DoubleClick(object sender, EventArgs e)
        {
            // 리스트박스에서 더블 클릭된 항목을 텍스트박스에 반영
            if (listBox_st_list_name.SelectedItem != null)
            {
                textBox_st_list_name.Text = listBox_st_list_name.SelectedItem.ToString();
                listBox_st_list_name.Visible = false;
            }
        }
        private void listBox_st_apply_mng_DoubleClick(object sender, EventArgs e)
        {
            // 리스트박스에서 더블 클릭된 항목을 텍스트박스에 반영
            if (listBox_st_apply_mng.SelectedItem != null)
            {
                textBox_st_apply_mng.Text = listBox_st_apply_mng.SelectedItem.ToString();
            }
        }

        private void textBox_st_list_toggle(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(listBox_st_list_code, true); //리스트박스 표시하기
        }
        private void textBox_st_list_name_toggle(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(listBox_st_list_name, true); //리스트박스 표시하기
        }
        private void textBox_st_list_code_Click(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(listBox_st_list_code, true); //리스트박스 표시하기

        }
        private void textBox_st_list_name_Click(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(listBox_st_list_name, true); //리스트박스 표시하기
        }
        private void textBox_st_apply_mng_toggle(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(listBox_st_apply_mng, true); //리스트박스 표시하기
            listBox_st_apply_mng.BringToFront();
        }
        private void textBox_st_apply_mng_Click(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(listBox_st_apply_mng, true); //리스트박스 표시하기
            listBox_st_apply_mng.BringToFront();
        }

        private void button_st_apply_mng_Click(object sender, EventArgs e)
        {
            // ListBox에 새로운 데이터 소스 설정
            listBox_st_apply_mng.DataSource = null; // 이전 데이터 소스 제거

            // 새로운 데이터 소스 설정 (형식: "이름 - 소속")
            List<string> recommendationList = new List<string>();
            foreach (List<string> el in Employee.employee_detail_property)
            {
                if (el[2] != "이름")
                {
                    string recommendation = $"{el[10]} - {el[2]}";
                    recommendationList.Add(recommendation);
                }
            }
            listBox_st_apply_mng.DataSource = recommendationList; // 새로운 데이터 소스 설정
        }

        private void ShowRecommendationList(List<string> recommendationList, ListBox listBox)
        {
            // ListBox에 새로운 데이터 소스 설정
            listBox.DataSource = null; // 이전 데이터 소스 제거
            listBox.DataSource = recommendationList; // 새로운 데이터 소스 설정

            listBox.Visible = true;
        }

        // 텍스트박스 클릭 이벤트 핸들러
        public static void TextBoxClickHandler(System.Windows.Forms.TextBox textBox, ListBox listBox)
        {
            // 텍스트박스 클릭 시 리스트박스가 보이도록 설정
            listBox.Visible = true;
        }

        // 리스트박스 더블클릭 이벤트 핸들러
        public static void ListBoxDoubleClickHandler(ListBox listBox, System.Windows.Forms.TextBox textBox)
        {
            // 리스트박스에서 더블 클릭된 항목을 텍스트박스에 반영
            if (listBox.SelectedItem != null)
            {
                textBox.Text = listBox.SelectedItem.ToString();
            }

            // 리스트박스를 다시 숨김
            listBox.Visible = false;
        }

        // 버튼 클릭 이벤트 핸들러
        public static void ButtonClickHandler(ListBox listBox, System.Windows.Forms.TextBox textBox)
        {
            // 선택한 항목이 있는 경우에만 처리
            if (listBox.SelectedItem != null)
            {
                // 리스트박스에서 선택된 항목을 텍스트박스에 반영
                textBox.Text = listBox.SelectedItem.ToString();

                // 리스트박스를 다시 숨김
                listBox.Visible = false;
            }
        }

        private void textBox_st_apply2_class_Click(object sender, EventArgs e)
        {
            // 텍스트박스 클릭 시 추천 항목을 보이도록 설정
            TextBoxClickHandler(textBox_st_apply2_class, listBox_st_apply2_class);

            // 추천 항목을 얻어와서 리스트박스에 표시
            List<string> recommendationList = new List<string>();// 추천 항목을 얻는 함수 호출

            // 추천 항목을 얻는 코드 작성
            recommendationList.Add("");
            recommendationList.Add("추천 항목 2");
            recommendationList.Add("추천 항목 3");
            ShowRecommendationList(recommendationList, listBox_st_apply2_class);
        }

        // listBox_st_apply_mng 더블클릭 이벤트 핸들러
        private void listBox_st_apply2_class_DoubleClick(object sender, EventArgs e)
        {
            // 리스트박스에서 더블 클릭된 항목을 텍스트박스에 반영
            ListBoxDoubleClickHandler(listBox_st_apply2_class, textBox_st_apply2_class);
        }

        // button_st_apply_mng 클릭 이벤트 핸들러
        private void button_st_apply2_class_Click(object sender, EventArgs e)
        {
            // 버튼 클릭 시 리스트박스에서 선택된 항목을 텍스트박스에 반영
            ButtonClickHandler(listBox_st_apply2_class, textBox_st_apply2_class);
        }




        private void MainForm_Click(object sender, EventArgs e)
        {
            // 클릭된 위치가 리스트박스 내부가 아니라면 리스트박스를 숨깁니다.
            if (!listBox_st_list_code.Bounds.Contains(this.PointToClient(MousePosition)))
            {
                ToggleListBoxVisibility(listBox_st_list_code, false); // 리스트박스 숨기기
            }
            else if (!listBox_st_list_name.Bounds.Contains(this.PointToClient(MousePosition)))
            {
                ToggleListBoxVisibility(listBox_st_list_name, false); // 리스트박스 숨기기
            }
        }
        private void ToggleListBoxVisibility(ListBox listBox, bool visibility)
        {
            listBox.Visible = visibility;
        }
        private void FilterData()
        {
            // 1. 품목 코드와 품목명 검색어를 받기
            string searchTextCode = textBox_st_list_code.Text.Trim().ToLower();
            string searchTextName = textBox_st_list_name.Text.Trim().ToLower();

            // 필터링된 결과를 저장할 리스트 초기화
            List<DataGridViewRow> filteredRows = new List<DataGridViewRow>();

            // 2. 품목 코드 검색어와 품목명 검색어가 모두 입력된 경우
            if (!string.IsNullOrEmpty(searchTextCode) && !string.IsNullOrEmpty(searchTextName))
            {
                foreach (DataGridViewRow row in dataGridView_st_list.Rows)
                {
                    string itemClass = row.Cells["품목 코드"].Value?.ToString().ToLower();
                    string itemName = row.Cells["품목명"].Value?.ToString().ToLower();

                    if (itemClass.Contains(searchTextCode) && itemName.Contains(searchTextName))
                    {
                        filteredRows.Add(row);
                    }
                }
            }
            // 3. 품목 코드 또는 품목명 검색어가 입력된 경우
            else if (!string.IsNullOrEmpty(searchTextCode) || !string.IsNullOrEmpty(searchTextName))
            {
                foreach (DataGridViewRow row in dataGridView_st_list.Rows)
                {
                    string itemClass = row.Cells["품목 코드"].Value?.ToString().ToLower();
                    string itemName = row.Cells["품목명"].Value?.ToString().ToLower();

                    // 품목 코드 검색어가 입력된 경우
                    if (!string.IsNullOrEmpty(searchTextCode) && itemClass.Contains(searchTextCode))
                    {
                        if (!string.IsNullOrEmpty(searchTextName) && !itemName.Contains(searchTextName))
                        {
                            ShowErrorMessage("검색 필터링 결과, 일치하는 값이 없습니다.");
                            return;
                        }

                        filteredRows.Add(row);
                    }
                    // 품목명 검색어가 입력된 경우
                    else if (!string.IsNullOrEmpty(searchTextName) && itemName.Contains(searchTextName))
                    {
                        filteredRows.Add(row);
                    }
                }
            }
            else
            {
                // 3. 텍스트 박스가 모두 비어있는 경우, 모든 데이터를 필터링된 결과로 설정
                if (string.IsNullOrEmpty(searchTextCode) && string.IsNullOrEmpty(searchTextName))
                {
                    filteredRows.AddRange(originalRows); // 초기상태의 데이터를 필터링된 결과로 설정
                }
            }

            // 4. 필터링된 결과를 DataGridView에 반영
            UpdateDataGridView(filteredRows);
        }

        private void button_st_list_code_Click(object sender, EventArgs e)
        {
            FilterData();
        }

        private void button_st_list_name_Click(object sender, EventArgs e)
        {
            FilterData();
        }

        private void BackupOriginalData()
        {
            originalRows.Clear(); // 기존 데이터를 모두 제거
            foreach (DataGridViewRow row in dataGridView_st_list.Rows)
            {
                originalRows.Add(row); // 모든 행을 백업 리스트에 추가
            }
        }

        private void UpdateDataGridView(List<DataGridViewRow> filteredRows)
        {
            // DataGridView에 필터링된 결과 반영
            dataGridView_st_list.Rows.Clear();
            dataGridView_st_list.Rows.AddRange(filteredRows.ToArray());
        }
        private List<DataGridViewRow> FilterDataGridViewRows(string searchTextCode, string searchTextName)
        {
            // 필터링된 결과를 저장할 리스트 초기화
            List<DataGridViewRow> filteredRows = new List<DataGridViewRow>();

            // 각 행의 품목 코드와 검색어를 비교하여 해당하지 않는 행을 필터링하고, 필터링된 결과를 리스트에 저장
            foreach (DataGridViewRow row in dataGridView_st_list.Rows)
            {
                string itemClass = row.Cells["품목 코드"].Value?.ToString().ToLower();
                string itemName = row.Cells["품목명"].Value?.ToString().ToLower();

                if (!string.IsNullOrEmpty(searchTextCode) && itemClass.Contains(searchTextCode))
                {
                    if (!string.IsNullOrEmpty(searchTextName) && !itemName.Contains(searchTextName))
                    {
                        ShowErrorMessage("검색 필터링 결과, 일치하는 값이 없습니다.");
                        return null; // 필터링된 결과가 없으면 null 반환
                    }

                    filteredRows.Add(row);
                }
            }

            return filteredRows;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // 데이터그리드뷰2 
        private void AddDataToCSVFile(string data1, string data2, string data3, int data4, string data5, int data6, float data7, string data8, string data9)
        {

            // CSV 파일에 데이터를 추가합니다.
            string filePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "stockdata.csv");
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine($"{data1},{data2},{data3},{data4},{data5},{data6},{data7},{data8},{data9}");
            }
        }

        private void SaveDataGridViewToCSV(string fileName, DataGridView dataGridView)
        {
            // "신청 일자", "담당자", "거래처", "납기 일자" 데이터 가져오기
            string applyDate = textBox_st_apply_day.Text;
            string manager = textBox_st_apply_mng.Text;
            string customer = textBox_st_apply_acc.Text;
            string deliveryDate = textBox_st_apply_delivery.Text;

            // 열 헤더 생성
            var headers = dataGridView.Columns.Cast<DataGridViewColumn>()
                                              .Select(column => column.HeaderText)
                                              .ToArray();

            // CSV 파일로 저장할 문자열 초기화
            StringBuilder csvContentBuilder = new StringBuilder();

            // "신청 일자", "담당자", "거래처", "납기 일자" 데이터 추가
            csvContentBuilder.AppendLine($"{applyDate},{manager},{customer},{deliveryDate}");

            // DataGridView의 열 헤더 추가
            csvContentBuilder.AppendLine(string.Join(",", headers));

            // DataGridView의 데이터를 CSV 형식으로 변환합니다.
            string csvContent = string.Join(Environment.NewLine,
                dataGridView.Rows.Cast<DataGridViewRow>()
                    .Select(row => string.Join(",", row.Cells.Cast<DataGridViewCell>()
                        .Where(cell => cell.OwningColumn.Name != "Approval")
                        .Select(cell => cell.Value)))
            );

            // CSV 파일로 저장합니다.
            File.WriteAllText(fileName, csvContentBuilder.ToString() + csvContent, Encoding.Default);

            // 저장된 파일을 열도록 유도합니다.
            MessageBox.Show($"데이터가 성공적으로 저장되었습니다.\n파일명: {fileName}");
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일을 열 수 없습니다: {ex.Message}");
            }
        }

        private List<string[]> GetDataFromCSV(string fileName)
        {
            List<string[]> csvData = new List<string[]>();

            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        csvData.Add(values);
                    }
                }
            }
            catch (IOException ex)
            {
                // 파일 읽기 오류 처리
                MessageBox.Show($"파일을 읽을 수 없습니다: {ex.Message}");
            }

            return csvData;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //기타 버튼

        private void button_st_bsk_save_Click(object sender, EventArgs e)
        {
            // 저장 날짜를 가져옵니다.
            string currentDate = DateTime.Now.ToString("yyyyMMdd");

            // 새로운 파일명을 만듭니다.
            string fileName = $"{currentDate}_dataGridView_st_bsk_{saveCount + 1}.csv";

            // CSV 파일에 데이터를 저장합니다.
            SaveDataGridViewToCSV(fileName, dataGridView_st_bsk);

            // 저장 횟수를 증가시킵니다.
            saveCount++;

            AddDataToDataGridView_history(fileName);

        }

        private void button_st_bsk_Reset_Click(object sender, EventArgs e)
        {
            dataGridView_st_bsk.Rows.Clear();
        }
        private void ExportToExcel(DataGridView dataGridView, string fileName)
        {
            {
                // 파일 경로 설정
                string filePath = Path.Combine(System.Windows.Forms.Application.StartupPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    // XLWorkbook을 FileStream을 사용하여 생성
                    var workbook = new XLWorkbook(fileStream);

                    // 기존 워크시트 가져오기
                    var worksheet = workbook.Worksheet("송장");

                    // 기존 데이터의 마지막 행 번호 확인
                    int lastRow = 8;

                    // 새로운 데이터를 기존 표에 추가합니다.
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        int columnCount = 3; // 엑셀 파일의 C열부터 시작
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            worksheet.Cell(lastRow, columnCount).Value = cell.Value?.ToString();
                            columnCount++;
                        }
                        lastRow++; // 다음 행으로 이동
                    }

                    // 변경된 내용을 저장합니다.
                    workbook.Save();
                }

                // 엑셀 파일 열기
                //Process.Start(filePath);

                /*
                 // 데이터 그리드 뷰의 데이터를 엑셀 파일로 내보냅니다.
                 string filePath = Path.Combine(Application.StartupPath, fileName);
                 var workbook = new XLWorkbook();
                 var worksheet = workbook.Worksheets.Add("데이터");

                 // 엑셀 파일에 헤더를 추가합니다.
                 int headerIndex = 1;
                 foreach (DataGridViewColumn column in dataGridView.Columns)
                 {
                     worksheet.Cell(1, headerIndex).Value = column.HeaderText;
                     headerIndex++;
                 }

                 // 엑셀 파일에 데이터를 추가합니다.
                 int rowIndex = 2;
                 foreach (DataGridViewRow row in dataGridView.Rows)
                 {
                     int columnIndex = 1;
                     foreach (DataGridViewCell cell in row.Cells)
                     {
                         worksheet.Cell(rowIndex, columnIndex).Value = cell.Value?.ToString();
                         columnIndex++;
                     }
                     rowIndex++;
                 }

                 workbook.SaveAs(filePath);
                 Process.Start(filePath);
                 */
            }
        }
        private void AddDataToDataGridView_history(string fileName)
        {
            // "신청 일자", "담당자", "거래처", "납기 일자" 데이터 가져오기
            string applyDate = textBox_st_apply_day.Text;
            string manager = textBox_st_apply_mng.Text;
            string customer = textBox_st_apply_acc.Text;
            string deliveryDate = textBox_st_apply_delivery.Text;

            // "발주 명세서" 파일 링크
            string specificationLink = "stock_Specification123";

            // CSV 파일의 데이터 가져오기
            List<string[]> csvData = GetDataFromCSV(fileName);

            // DataGridView1에 열 추가
            if (dataGridView_history.Columns.Count < 5)
            {
                dataGridView_history.Columns.AddRange(
                    new DataGridViewTextBoxColumn()
                    {
                        Name = "ApplyDate",
                        HeaderText = "신청 일자"
                    },
                    new DataGridViewTextBoxColumn()
                    {
                        Name = "Manager",
                        HeaderText = "담당자"
                    },
                    new DataGridViewTextBoxColumn()
                    {
                        Name = "Customer",
                        HeaderText = "거래처"
                    },
                    new DataGridViewTextBoxColumn()
                    {
                        Name = "DeliveryDate",
                        HeaderText = "납기 일자"
                    },
                    new DataGridViewTextBoxColumn()
                    {
                        Name = "Specification",
                        HeaderText = "발주 명세서"
                    });
                // "Specification" 열의 너비 설정
                dataGridView_history.Columns["Specification"].Width = 150; // 원하는 너비로 변경하세요
            }

            // DataGridView1에 데이터 추가
            if (csvData.Count > 0)
            {
                string[] firstRow = csvData[0]; // 첫 번째 행 가져오기
                dataGridView_history.Rows.Add(applyDate, manager, customer, deliveryDate, specificationLink); // 첫 번째 행 추가

            }
        }

        private void button_st_history_search_Click(object sender, EventArgs e)
        {
            // 시작 및 종료 날짜 가져오기
            DateTime startDate = DateTime.Parse(textBox_st_history_day.Text);
            DateTime endDate = DateTime.Parse(textBox_st_history_day1.Text);

            // DataGridView_history의 데이터 필터링
            foreach (DataGridViewRow row in dataGridView_history.Rows)
            {
                // 첫 번째 열의 데이터 가져오기
                string dateStr = row.Cells[0].Value.ToString();
                DateTime date = DateTime.Parse(dateStr);

                // 날짜가 시작 날짜와 종료 날짜 사이에 있는지 확인
                if (date >= startDate && date <= endDate)
                {
                    // 범위에 해당하는 데이터는 보이게 설정
                    row.Visible = true;
                }
                else
                {
                    // 범위에 해당하지 않는 데이터는 숨기게 설정
                    row.Visible = false;
                }
            }
        }

        private void DataGridViewHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_history.Columns["Specification"].Index && e.RowIndex >= 0)
            {
                // 발주 명세서 열을 클릭했을 때 파일을 열도록 구현
                string specificationLink = dataGridView_history.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (!string.IsNullOrEmpty(specificationLink))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(specificationLink);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"파일을 열 수 없습니다: {ex.Message}");
                    }
                }
            }
        }

        private List<string[]> GetDataFromExcel(string fileName)
        {
            List<string[]> excelData = new List<string[]>();

            // 엑셀 파일에서 데이터 읽어오기
            using (var workbook = new XLWorkbook(fileName))
            {
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    foreach (var row in worksheet.RowsUsed())
                    {
                        excelData.Add(row.Cells().Select(cell => cell.Value.ToString()).ToArray());
                    }
                }
            }

            return excelData;
        }

        private void button_st_Specification_Click(object sender, EventArgs e)
        {
            // DataGridView의 데이터를 엑셀 파일로 내보내기
            ExportToExcel(dataGridView_st_bsk, "stock_Specification123.xlsx");
        }

        private void button_bsk_approval_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // DataGridView 생성
            DataGridView dataGridView = new DataGridView();

            // DataGridView에 엑셀 파일의 내용을 로드한다고 가정
            // LoadExcelData(dataGridView);

            // DataGridView를 printPreviewControl에 추가
            printPreviewControl_st.Controls.Clear(); // 이미 추가된 컨트롤 제거
            printPreviewControl_st.Controls.Add(dataGridView);

            // DataGridView를 printPreviewControl에 맞게 크기 조정
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // DataGridView를 printPreviewControl에 맞게 위치 조정
            dataGridView.Dock = DockStyle.Fill;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ShowPreviewForm()
        {
            // 미리보기 창을 생성하고 표시
            Form previewForm = new Form();
            previewForm.Text = "File Preview";
            previewForm.Size = new System.Drawing.Size(400, 300);

            // RichTextBox를 미리보기 창에 추가
            RichTextBox richTextBoxPreview = new RichTextBox();
            richTextBoxPreview.Dock = DockStyle.Fill;
            previewForm.Controls.Add(richTextBoxPreview);

            // 미리보기 창 표시
            previewForm.ShowDialog();
        }

        private void listBox_st_apply2_class_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void dataGridView_st_bsk_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            // 현재 셀이 수정되었는지 확인
            if (dataGridView.IsCurrentCellDirty)
            {
                // 변경된 셀이 3열인지 확인
                if (dataGridView.CurrentCell.ColumnIndex == 3) // 3열은 인덱스로 2를 사용합니다. (0부터 시작)
                {
                    // 변경된 셀을 즉시 반영하도록 함
                    dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void dataGridView_st_bsk_CellValueChanged1(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            // 변경된 셀이 3열인지 확인
            if (columnIndex == 3) // 3열은 인덱스로 2를 사용합니다. (0부터 시작)
            {
                // 3열의 데이터 값 가져오기
                object cellValueObj = dataGridView_st_bsk.Rows[rowIndex].Cells[3].Value;

                // 셀 값이 null 또는 비어 있는지 확인
                string cellValue = cellValueObj != null ? cellValueObj.ToString() : "0";

                // 3열의 데이터 값이 숫자인지 확인
                if (int.TryParse(cellValue, out int value3))
                {
                    // 5열의 데이터 값 가져오기
                    object value5Obj = dataGridView_st_bsk.Rows[rowIndex].Cells[5].Value;

                    // 5열의 데이터 값이 null 또는 비어 있는지 확인하고 값을 설정
                    int value5Int = 0;
                    if (value5Obj != null)
                    {
                        string value5 = value5Obj.ToString();
                        if (int.TryParse(value5, out value5Int))
                        {
                            // 3열과 5열의 값을 곱해서 6열에 반영
                            dataGridView_st_bsk.Rows[rowIndex].Cells[6].Value = (value3 * value5Int).ToString();
                        }
                    }
                }
            }
        }

        private void dataGridView_st_bsk_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }





        private bool make_connection()              // sql 연결함수
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public List<List<string>> create_db_list(string table_name)
        {
            List<List<string>> table_list_temp = new List<List<string>>();
            try
            {
                conn = new MySqlConnection(connectionString);
                if (make_connection())
                {
                    string sql = $"SELECT * FROM team2.{table_name}";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        List<string> table = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader[i].GetType() == typeof(DateTime))
                            {
                                table.Add(reader[i].ToString().Substring(0, 10));
                            }
                            else
                            {
                                table.Add(reader[i].ToString());
                            }
                            //MessageBox.Show(reader[i].ToString());
                        }
                        table_list_temp.Add(table);

                    }

                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return table_list_temp;
        }


    }
}

