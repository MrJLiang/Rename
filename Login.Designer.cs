namespace HAAGONtest
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.btn_touchKeyBoard = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_login = new System.Windows.Forms.Button();
            this.checkBox_isShowPassword = new System.Windows.Forms.CheckBox();
            this.btn_resetCode = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.comboBox_loginLevel = new System.Windows.Forms.ComboBox();
            this.txtbox_password = new System.Windows.Forms.TextBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_touchKeyBoard
            // 
            this.btn_touchKeyBoard.AutoSize = true;
            this.btn_touchKeyBoard.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_touchKeyBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_touchKeyBoard.Font = new System.Drawing.Font("宋体", 20F);
            this.btn_touchKeyBoard.Location = new System.Drawing.Point(540, 93);
            this.btn_touchKeyBoard.Name = "btn_touchKeyBoard";
            this.btn_touchKeyBoard.Size = new System.Drawing.Size(150, 71);
            this.btn_touchKeyBoard.TabIndex = 26;
            this.btn_touchKeyBoard.Text = "屏幕键盘";
            this.btn_touchKeyBoard.UseVisualStyleBackColor = false;
            this.btn_touchKeyBoard.Click += new System.EventHandler(this.btn_touchKeyBoard_Click);
            // 
            // btn_close
            // 
            this.btn_close.AutoSize = true;
            this.btn_close.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("宋体", 20F);
            this.btn_close.Location = new System.Drawing.Point(540, 330);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(150, 71);
            this.btn_close.TabIndex = 18;
            this.btn_close.Text = "返回";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_login
            // 
            this.btn_login.AutoSize = true;
            this.btn_login.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_login.Font = new System.Drawing.Font("宋体", 20F);
            this.btn_login.Location = new System.Drawing.Point(540, 250);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(150, 71);
            this.btn_login.TabIndex = 19;
            this.btn_login.Text = "登陆";
            this.btn_login.UseVisualStyleBackColor = false;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // checkBox_isShowPassword
            // 
            this.checkBox_isShowPassword.AutoSize = true;
            this.checkBox_isShowPassword.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_isShowPassword.Font = new System.Drawing.Font("宋体", 25F);
            this.checkBox_isShowPassword.Location = new System.Drawing.Point(402, 374);
            this.checkBox_isShowPassword.Name = "checkBox_isShowPassword";
            this.checkBox_isShowPassword.Size = new System.Drawing.Size(102, 38);
            this.checkBox_isShowPassword.TabIndex = 25;
            this.checkBox_isShowPassword.Text = "显示";
            this.checkBox_isShowPassword.UseVisualStyleBackColor = false;
            this.checkBox_isShowPassword.CheckedChanged += new System.EventHandler(this.checkBox_isShowPassword_CheckedChanged);
            // 
            // btn_resetCode
            // 
            this.btn_resetCode.AutoSize = true;
            this.btn_resetCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_resetCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_resetCode.Font = new System.Drawing.Font("宋体", 20F);
            this.btn_resetCode.Location = new System.Drawing.Point(540, 170);
            this.btn_resetCode.Name = "btn_resetCode";
            this.btn_resetCode.Size = new System.Drawing.Size(150, 71);
            this.btn_resetCode.TabIndex = 24;
            this.btn_resetCode.Text = "修改密码";
            this.btn_resetCode.UseVisualStyleBackColor = false;
            this.btn_resetCode.Click += new System.EventHandler(this.btn_resetCode_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("宋体", 25F);
            this.Label2.Location = new System.Drawing.Point(103, 130);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(151, 34);
            this.Label2.TabIndex = 22;
            this.Label2.Text = "登录类型";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("宋体", 25F);
            this.Label1.Location = new System.Drawing.Point(103, 329);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(151, 34);
            this.Label1.TabIndex = 23;
            this.Label1.Text = "登录密码";
            // 
            // comboBox_loginLevel
            // 
            this.comboBox_loginLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.comboBox_loginLevel.Font = new System.Drawing.Font("宋体", 25F);
            this.comboBox_loginLevel.FormattingEnabled = true;
            this.comboBox_loginLevel.Items.AddRange(new object[] {
            "操作人员",
            "管理人员",
            "调试人员"});
            this.comboBox_loginLevel.Location = new System.Drawing.Point(103, 169);
            this.comboBox_loginLevel.Name = "comboBox_loginLevel";
            this.comboBox_loginLevel.Size = new System.Drawing.Size(286, 173);
            this.comboBox_loginLevel.TabIndex = 21;
            this.comboBox_loginLevel.Text = "操作人员";
            this.comboBox_loginLevel.TextChanged += new System.EventHandler(this.comboBox_loginLevel_TextChanged);
            // 
            // txtbox_password
            // 
            this.txtbox_password.Font = new System.Drawing.Font("宋体", 25F);
            this.txtbox_password.Location = new System.Drawing.Point(103, 370);
            this.txtbox_password.Name = "txtbox_password";
            this.txtbox_password.PasswordChar = '*';
            this.txtbox_password.Size = new System.Drawing.Size(286, 46);
            this.txtbox_password.TabIndex = 20;
            this.txtbox_password.TextChanged += new System.EventHandler(this.txtbox_password_TextChanged);
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(71, 12);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(342, 109);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 27;
            this.PictureBox1.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(763, 435);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.btn_touchKeyBoard);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.checkBox_isShowPassword);
            this.Controls.Add(this.btn_resetCode);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.comboBox_loginLevel);
            this.Controls.Add(this.txtbox_password);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btn_touchKeyBoard;
        internal System.Windows.Forms.Button btn_close;
        internal System.Windows.Forms.Button btn_login;
        internal System.Windows.Forms.CheckBox checkBox_isShowPassword;
        internal System.Windows.Forms.Button btn_resetCode;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox comboBox_loginLevel;
        internal System.Windows.Forms.TextBox txtbox_password;
        internal System.Windows.Forms.PictureBox PictureBox1;
    }
}