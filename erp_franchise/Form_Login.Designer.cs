namespace erp_franchise
{
    partial class Form_Login
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.code_textbox = new System.Windows.Forms.TextBox();
            this.pw_textbox = new System.Windows.Forms.TextBox();
            this.login_button = new System.Windows.Forms.Button();
            this.login_warning = new System.Windows.Forms.Label();
            this.label_user_ID = new System.Windows.Forms.Label();
            this.label_user_PW = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // code_textbox
            // 
            this.code_textbox.Font = new System.Drawing.Font("굴림", 20F);
            this.code_textbox.Location = new System.Drawing.Point(10000, 10000);
            this.code_textbox.Name = "code_textbox";
            this.code_textbox.Size = new System.Drawing.Size(254, 38);
            this.code_textbox.TabIndex = 0;
            this.code_textbox.TextChanged += new System.EventHandler(this.code_textbox_TextChanged);
            this.code_textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.id_pw_textbox_KeyDown);
            // 
            // pw_textbox
            // 
            this.pw_textbox.Font = new System.Drawing.Font("굴림", 20F);
            this.pw_textbox.Location = new System.Drawing.Point(10000, 10000);
            this.pw_textbox.Name = "pw_textbox";
            this.pw_textbox.PasswordChar = '*';
            this.pw_textbox.Size = new System.Drawing.Size(254, 38);
            this.pw_textbox.TabIndex = 1;
            this.pw_textbox.TextChanged += new System.EventHandler(this.pw_textbox_TextChanged);
            this.pw_textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.id_pw_textbox_KeyDown);
            // 
            // login_button
            // 
            this.login_button.Location = new System.Drawing.Point(10000, 10000);
            this.login_button.Name = "login_button";
            this.login_button.Size = new System.Drawing.Size(81, 50);
            this.login_button.TabIndex = 4;
            this.login_button.Text = "LOGIN";
            this.login_button.UseVisualStyleBackColor = true;
            this.login_button.Click += new System.EventHandler(this.login_button_Click);
            // 
            // login_warning
            // 
            this.login_warning.AutoSize = true;
            this.login_warning.BackColor = System.Drawing.Color.Transparent;
            this.login_warning.Font = new System.Drawing.Font("굴림", 20F);
            this.login_warning.ForeColor = System.Drawing.Color.Red;
            this.login_warning.Location = new System.Drawing.Point(656, 670);
            this.login_warning.Name = "login_warning";
            this.login_warning.Size = new System.Drawing.Size(0, 27);
            this.login_warning.TabIndex = 5;
            // 
            // label_user_ID
            // 
            this.label_user_ID.BackColor = System.Drawing.Color.Transparent;
            this.label_user_ID.Font = new System.Drawing.Font("굴림", 30F);
            this.label_user_ID.Location = new System.Drawing.Point(654, 521);
            this.label_user_ID.MaximumSize = new System.Drawing.Size(430, 40);
            this.label_user_ID.Name = "label_user_ID";
            this.label_user_ID.Size = new System.Drawing.Size(430, 40);
            this.label_user_ID.TabIndex = 6;
            this.label_user_ID.Text = "아이디를 입력하세요";
            this.label_user_ID.Click += new System.EventHandler(this.label_user_ID_Click);
            // 
            // label_user_PW
            // 
            this.label_user_PW.BackColor = System.Drawing.Color.Transparent;
            this.label_user_PW.Font = new System.Drawing.Font("굴림", 30F);
            this.label_user_PW.Location = new System.Drawing.Point(654, 602);
            this.label_user_PW.MaximumSize = new System.Drawing.Size(430, 40);
            this.label_user_PW.Name = "label_user_PW";
            this.label_user_PW.Size = new System.Drawing.Size(430, 40);
            this.label_user_PW.TabIndex = 7;
            this.label_user_PW.Text = "비밀번호를 입력하세요";
            this.label_user_PW.Click += new System.EventHandler(this.label_user_PW_Click);
            // 
            // Form_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(238)))), ((int)(((byte)(234)))));
            this.BackgroundImage = global::erp_franchise.Properties.Resources.login3;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.label_user_PW);
            this.Controls.Add(this.label_user_ID);
            this.Controls.Add(this.login_warning);
            this.Controls.Add(this.login_button);
            this.Controls.Add(this.pw_textbox);
            this.Controls.Add(this.code_textbox);
            this.Name = "Form_Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox code_textbox;
        private System.Windows.Forms.TextBox pw_textbox;
        private System.Windows.Forms.Button login_button;
        private System.Windows.Forms.Label login_warning;
        private System.Windows.Forms.Label label_user_ID;
        private System.Windows.Forms.Label label_user_PW;
    }
}

