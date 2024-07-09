using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace erp_franchise
{
    public partial class Form_Login : Form
    {
        //private string logined_ID;

        //public static string logined_ID_property
        //{
        //    get { return logined_ID_property; }
        //    set { logined_ID_property = value;}
        //}

        public Form_Login()
        {
            InitializeComponent();
        }

        public (bool, string) login()         // 로그인
        {
            string user_ID = code_textbox.Text;
            string user_pw = pw_textbox.Text;

            List<List<string>> employee_idpw_list = employee_idpw();

            bool login_tf = false;

            if (user_ID.Equals(""))
            {
                login_warning.Text = "ID를 입력해주세요";
                pw_textbox.Text = "";
                this.ActiveControl = code_textbox;
            }
            else if (user_pw.Equals(""))
            {
                login_warning.Text = "PASSWORD를 입력해주세요";
                pw_textbox.Text = "";
                this.ActiveControl = pw_textbox;
            }
            else
            {
                for (int i = 0; i < employee_idpw_list.Count; i++)
                {
                    if (user_ID == employee_idpw_list[i][0] && user_pw == employee_idpw_list[i][1])
                    {
                        login_tf = true;
                        break;
                    }

                }
                if (!login_tf)
                {
                    MessageBox.Show("ID 혹은 PASSWORD가 일치하지 않습니다.", "경고");
                    pw_textbox.Text = "";
                }

            }

            return (login_tf, user_ID);
        }

        private void id_pw_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login_button_Click(sender, e);
            }
        }

        private void login_button_Click(object sender, EventArgs e)
        {

            var (login_tf, user_ID) = login();
            if (login_tf)
            {
                this.Hide();
                Form_Main form_main = new Form_Main();
                form_main.Set_ID(user_ID);      // 로그인한 아이디 메인폼에 전달
                form_main.ShowDialog();
                this.Close();
            }
        }

        public List<List<string>> employee_idpw()
        {
            //string filePath = "employee_detail.csv";
            //string[] lines = File.ReadAllLines(filePath, Encoding.Default);

            Form_Main form_Main = new Form_Main();
            List<List<string>> employee_list_temp = form_Main.create_db_list("employee_inform");

            //List<List<string>> employee_list_temp = new List<List<string>>();
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    List<string> employeeData = new List<string> { lines[i].Split(',').ToList()[0], lines[i].Split(',').ToList()[1] };
            //    employee_list_temp.Add(employeeData);
            //}

            
            return employee_list_temp;
        }

        private void label_user_ID_Click(object sender, EventArgs e)
        {
            label_user_ID.Text = code_textbox.Text;
            this.ActiveControl = code_textbox;
        }

        private void code_textbox_TextChanged(object sender, EventArgs e)
        {
            label_user_ID.Text = code_textbox.Text;
        }

        private void label_user_PW_Click(object sender, EventArgs e)
        {
            label_user_PW.Text = pw_textbox.Text;
            this.ActiveControl = pw_textbox;
        }

        private void pw_textbox_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != null)
            {
                label_user_PW.Text = "";
            }
            else if (this.ActiveControl == null && pw_textbox.Text == "")
            {
                label_user_PW.Text = "비밀번호를 입력해주세요";
            }

            label_user_PW.Text = "";
            for (int text = 0; text < pw_textbox.Text.Length; text++)
            {
                label_user_PW.Text += '*';
            }
        }
    }
}
