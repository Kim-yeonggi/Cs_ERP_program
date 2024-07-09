using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static erp_franchise.Form_Main;
using System.Drawing;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using System.Data;

namespace erp_franchise
{

    public class Employee
    {
        private static MySqlConnection conn;
        private static string server = "192.168.31.147";
        private static string database = "team2";
        private static string uid = "root";
        private static string password = "0000";
        private static string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";



        private static List<List<string>> _employee_detail = new List<List<string>>();    
        private static List<List<string>> _employee_certificate = new List<List<string>>();
        private static List<List<string>> _employee_language = new List<List<string>>();
        private static List<List<string>> _employee_salary = new List<List<string>>();

        public static List<List<string>> employee_detail_property                   // 세부 인적사항
        {
            get { return _employee_detail; }
            set { _employee_detail = value; }
        }
        public static List<List<string>> employee_certificate_property              // 자격증
        {
            get { return _employee_certificate; }
            set { _employee_certificate = value; }
        }
        public static List<List<string>> employee_language_property                 // 어학
        {
            get { return _employee_language; }
            set { _employee_language = value; }
        }
        public static List<List<string>> employee_salary_property                   // 급여
        {
            get { return _employee_salary; }
            set { _employee_salary = value; }
        }


        //public static List<List<string>> create_employee(string filePath)
        //{
        //    string[] lines = File.ReadAllLines(filePath, Encoding.Default);

        //    List<List<string>> employee_list_temp = new List<List<string>>();

        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        List<string> employeeData = lines[i].Split(',').ToList();
        //        employee_list_temp.Add(employeeData);
        //    }

        //    return employee_list_temp;



        //}

        

        public static List<List<string>> search_employee(ComboBox search_ep_comboBox, TextBox search_ep_textBox, DataGridView dataGridView_employee) 
        {

            List<List<string>> search_result_ep_list = new List<List<string>>();    // 검색 결과 담을 리스트
            int selected_catagory_index;       // 선택된 카테고리의 인덱스

            // csv에서의 선택된 카테고리 인덱스 추출
            List<string> dataGridView_columns = new List<string>();
            foreach (DataGridViewColumn columns in dataGridView_employee.Columns)
            {
               dataGridView_columns.Add(columns.HeaderText);
            }
            selected_catagory_index = Employee.employee_detail_property[0].IndexOf(dataGridView_columns[search_ep_comboBox.SelectedIndex]);       // 콤보박스의 선택된 카테고리 인덱스 저장


            // 첫번째에 임시 리스트 추가
            List<string> temp_header_list = Employee.employee_detail_property[0];     // 카테고리 행 리스트
            search_result_ep_list.Add(temp_header_list);
            for(int el1 = 1; el1 < Employee.employee_detail_property.Count; el1++)      // 검색 텍스트에 일치하는 항목들 리스트에 추가 / 첫번째(index 0)에는 임시 리스트가 있으므로 인덱스  1 부터 시작
            {
                if (Employee.employee_detail_property[el1][selected_catagory_index].IndexOf(search_ep_textBox.Text) != -1)
                {
                    search_result_ep_list.Add(Employee.employee_detail_property[el1]);
                }
            }

            return search_result_ep_list;

        }


        public static void create_DataGridView_employeeList(List<List<string>> employee_list, DataGridView dataGridView_employee)        // 데이터 그리드를 그리는 함수
        {
            dataGridView_employee.Rows.Clear();

            List<List<string>> add_ep_dataGridView_list = new List<List<string>>();
            for (int epl1 = 0; epl1 < employee_list.Count; epl1++)
            {
                List<string> temp_ep_list = new List<string>() {        // 제목행 설정
                    employee_list[epl1][0],                     // 직원코드
                    employee_list[epl1][2],                     // 이름
                    employee_list[epl1][3],                     // 생년월일 
                    employee_list[epl1][6],                     // 이메일
                    employee_list[epl1][11],                    // 소속
                    employee_list[epl1][12],                    // 직급
                    employee_list[epl1][14],                    // 직무
                    employee_list[epl1][13],                    // 입사일
                    employee_list[epl1][17],                    // 상태
                };

                add_ep_dataGridView_list.Add(temp_ep_list);
            }

            for (int j = 0; j < employee_list.Count; j++)
            {
                dataGridView_employee.Rows.Add(add_ep_dataGridView_list[j].ToArray());
            }


            // 첫번째 열, 마지막 행 제거, 셀 선택시 행 선택 처리
            dataGridView_employee.RowHeadersVisible = false;
            dataGridView_employee.AllowUserToAddRows = false;
            dataGridView_employee.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView_employee.AllowUserToResizeColumns = false;
            dataGridView_employee.AllowUserToResizeRows = false;

        }

        public static void remove_employee(List<string> selected_employee_code)
        {
            if (MessageBox.Show($"총 {selected_employee_code.Count}개의 항목을 삭제하시겠습니까?", "알림", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                try
                {
                    conn = new MySqlConnection(connectionString);
                    if (make_connection())
                    {
                        foreach (string sec in selected_employee_code)
                        {
                            string ep_remove_query = $"delete from team2.employee_inform where code='{sec}';";
                            MySqlCommand cmd = new MySqlCommand(ep_remove_query, conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    //MessageBox.Show("전송 성공");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //    // 리스트에서 일치하는지 검사후 삭제
            //    for (int del1 = 0; del1 < temp_employee_list.Count; del1++)
            //    {
            //        for (int del2 = 0; del2 < selected_employee_code.Count; del2++)
            //        {
            //            if (temp_employee_list[del1].IndexOf(selected_employee_code[del2]) != -1)
            //            {
            //                temp_employee_list.Remove(temp_employee_list[del1]);
            //            }                                                                                         
            //        }
            //    }
            //    selected_employee_code.Clear();


            //    using (StreamWriter writer = new StreamWriter("employee_detail.csv", false, Encoding.UTF8))
            //    {

            //        for (int el1 = 0; el1 < temp_employee_list.Count; el1++)
            //        {
            //            for (int el2 = 0; el2 < temp_employee_list[el1].Count; el2++)
            //            {
            //                //temp0_textBox.Text += employee_list[el1][el2];
            //                if (el2 == temp_employee_list[el1].Count - 1)
            //                {
            //                    writer.WriteLine(temp_employee_list[el1][el2].ToString(), false, Encoding.UTF8);     // 마지막일 경우 writeLine -> 줄바꿈
            //                }
            //                else
            //                {
            //                    writer.Write(temp_employee_list[el1][el2].ToString() + ",", false, Encoding.UTF8);   // 그 외 , 작성
            //                }
            //            }
            //        }
            //        writer.Close();
            //    }

                    
            //}

            //return temp_employee_list;
        }

        private static bool make_connection()              // sql 연결함수
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

    }

}
