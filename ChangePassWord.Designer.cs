namespace HAAGONtest
{
    partial class ChangePassWord
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
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Cbx_CodeShow = new System.Windows.Forms.CheckBox();
            this.Btn_exit = new System.Windows.Forms.Button();
            this.Cbx_ResetType = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Txbox_newCode_comfirm = new System.Windows.Forms.TextBox();
            this.Txbox_newCode = new System.Windows.Forms.TextBox();
            this.Txbox_Code = new System.Windows.Forms.TextBox();
            this.Btn_reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label6
            // 
            this.Label6.BackColor = System.Drawing.Color.Crimson;
            this.Label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label6.Font = new System.Drawing.Font("宋体", 25F);
            this.Label6.ForeColor = System.Drawing.Color.Black;
            this.Label6.Location = new System.Drawing.Point(0, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(795, 70);
            this.Label6.TabIndex = 13;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.BackColor = System.Drawing.Color.Crimson;
            this.Label5.Font = new System.Drawing.Font("宋体", 25F);
            this.Label5.Location = new System.Drawing.Point(12, 19);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(219, 34);
            this.Label5.TabIndex = 18;
            this.Label5.Text = "登录密码修改";
            // 
            // Cbx_CodeShow
            // 
            this.Cbx_CodeShow.AutoSize = true;
            this.Cbx_CodeShow.BackColor = System.Drawing.Color.Transparent;
            this.Cbx_CodeShow.Font = new System.Drawing.Font("宋体", 25F);
            this.Cbx_CodeShow.Location = new System.Drawing.Point(601, 222);
            this.Cbx_CodeShow.Name = "Cbx_CodeShow";
            this.Cbx_CodeShow.Size = new System.Drawing.Size(102, 38);
            this.Cbx_CodeShow.TabIndex = 29;
            this.Cbx_CodeShow.Text = "显示";
            this.Cbx_CodeShow.UseVisualStyleBackColor = false;
            this.Cbx_CodeShow.CheckedChanged += new System.EventHandler(this.Cbx_CodeShow_CheckedChanged);
            // 
            // Btn_exit
            // 
            this.Btn_exit.BackColor = System.Drawing.Color.Transparent;
            this.Btn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_exit.Font = new System.Drawing.Font("宋体", 20F);
            this.Btn_exit.Location = new System.Drawing.Point(617, 360);
            this.Btn_exit.Name = "Btn_exit";
            this.Btn_exit.Size = new System.Drawing.Size(150, 71);
            this.Btn_exit.TabIndex = 28;
            this.Btn_exit.Text = "退出";
            this.Btn_exit.UseVisualStyleBackColor = false;
            this.Btn_exit.Click += new System.EventHandler(this.Btn_exit_Click);
            // 
            // Cbx_ResetType
            // 
            this.Cbx_ResetType.Font = new System.Drawing.Font("宋体", 25F);
            this.Cbx_ResetType.FormattingEnabled = true;
            this.Cbx_ResetType.Items.AddRange(new object[] {
            "操作人员",
            "管理人员"});
            this.Cbx_ResetType.Location = new System.Drawing.Point(311, 86);
            this.Cbx_ResetType.Name = "Cbx_ResetType";
            this.Cbx_ResetType.Size = new System.Drawing.Size(272, 41);
            this.Cbx_ResetType.TabIndex = 27;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("宋体", 25F);
            this.Label3.ForeColor = System.Drawing.Color.Black;
            this.Label3.Location = new System.Drawing.Point(120, 287);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(185, 34);
            this.Label3.TabIndex = 25;
            this.Label3.Text = "确认新密码";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("宋体", 25F);
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(188, 222);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(117, 34);
            this.Label2.TabIndex = 26;
            this.Label2.Text = "新密码";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Font = new System.Drawing.Font("宋体", 25F);
            this.Label4.ForeColor = System.Drawing.Color.Black;
            this.Label4.Location = new System.Drawing.Point(52, 92);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(253, 34);
            this.Label4.TabIndex = 24;
            this.Label4.Text = "修改密码的类型";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("宋体", 25F);
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(18, 157);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(287, 34);
            this.Label1.TabIndex = 23;
            this.Label1.Text = "输入管理人员密码";
            // 
            // Txbox_newCode_comfirm
            // 
            this.Txbox_newCode_comfirm.Font = new System.Drawing.Font("宋体", 25F);
            this.Txbox_newCode_comfirm.Location = new System.Drawing.Point(311, 278);
            this.Txbox_newCode_comfirm.Name = "Txbox_newCode_comfirm";
            this.Txbox_newCode_comfirm.PasswordChar = '*';
            this.Txbox_newCode_comfirm.Size = new System.Drawing.Size(272, 46);
            this.Txbox_newCode_comfirm.TabIndex = 20;
            // 
            // Txbox_newCode
            // 
            this.Txbox_newCode.Font = new System.Drawing.Font("宋体", 25F);
            this.Txbox_newCode.Location = new System.Drawing.Point(311, 213);
            this.Txbox_newCode.Name = "Txbox_newCode";
            this.Txbox_newCode.PasswordChar = '*';
            this.Txbox_newCode.Size = new System.Drawing.Size(272, 46);
            this.Txbox_newCode.TabIndex = 21;
            // 
            // Txbox_Code
            // 
            this.Txbox_Code.Font = new System.Drawing.Font("宋体", 25F);
            this.Txbox_Code.Location = new System.Drawing.Point(311, 148);
            this.Txbox_Code.Name = "Txbox_Code";
            this.Txbox_Code.PasswordChar = '*';
            this.Txbox_Code.Size = new System.Drawing.Size(272, 46);
            this.Txbox_Code.TabIndex = 22;
            // 
            // Btn_reset
            // 
            this.Btn_reset.BackColor = System.Drawing.Color.Transparent;
            this.Btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_reset.Font = new System.Drawing.Font("宋体", 20F);
            this.Btn_reset.Location = new System.Drawing.Point(396, 360);
            this.Btn_reset.Name = "Btn_reset";
            this.Btn_reset.Size = new System.Drawing.Size(150, 71);
            this.Btn_reset.TabIndex = 19;
            this.Btn_reset.Text = "确认修改";
            this.Btn_reset.UseVisualStyleBackColor = false;
            this.Btn_reset.Click += new System.EventHandler(this.Btn_reset_Click);
            // 
            // Change
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(795, 448);
            this.Controls.Add(this.Cbx_CodeShow);
            this.Controls.Add(this.Btn_exit);
            this.Controls.Add(this.Cbx_ResetType);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Txbox_newCode_comfirm);
            this.Controls.Add(this.Txbox_newCode);
            this.Controls.Add(this.Txbox_Code);
            this.Controls.Add(this.Btn_reset);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Change";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change";
            this.Load += new System.EventHandler(this.Change_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.CheckBox Cbx_CodeShow;
        internal System.Windows.Forms.Button Btn_exit;
        internal System.Windows.Forms.ComboBox Cbx_ResetType;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox Txbox_newCode_comfirm;
        internal System.Windows.Forms.TextBox Txbox_newCode;
        internal System.Windows.Forms.TextBox Txbox_Code;
        internal System.Windows.Forms.Button Btn_reset;
    }
}