namespace HAAGONtest
{
    partial class frmNameModel
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
            this.btn_entername = new System.Windows.Forms.Button();
            this.btn_escname = new System.Windows.Forms.Button();
            this.btn_keyboard = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_entername
            // 
            this.btn_entername.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_entername.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_entername.Location = new System.Drawing.Point(24, 96);
            this.btn_entername.Name = "btn_entername";
            this.btn_entername.Size = new System.Drawing.Size(107, 40);
            this.btn_entername.TabIndex = 790;
            this.btn_entername.Text = "确认";
            this.btn_entername.UseVisualStyleBackColor = true;
            this.btn_entername.Click += new System.EventHandler(this.btn_entername_Click);
            // 
            // btn_escname
            // 
            this.btn_escname.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_escname.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_escname.Location = new System.Drawing.Point(179, 96);
            this.btn_escname.Name = "btn_escname";
            this.btn_escname.Size = new System.Drawing.Size(107, 40);
            this.btn_escname.TabIndex = 789;
            this.btn_escname.Text = "取消";
            this.btn_escname.UseVisualStyleBackColor = true;
            this.btn_escname.Click += new System.EventHandler(this.btn_escname_Click);
            // 
            // btn_keyboard
            // 
            this.btn_keyboard.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_keyboard.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_keyboard.Location = new System.Drawing.Point(179, 38);
            this.btn_keyboard.Name = "btn_keyboard";
            this.btn_keyboard.Size = new System.Drawing.Size(107, 40);
            this.btn_keyboard.TabIndex = 791;
            this.btn_keyboard.Text = "屏幕键盘";
            this.btn_keyboard.UseVisualStyleBackColor = true;
            this.btn_keyboard.Click += new System.EventHandler(this.btn_keyboard_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 792;
            this.label1.Text = "创建型号名称：";
            // 
            // txt_name
            // 
            this.txt_name.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_name.Location = new System.Drawing.Point(24, 45);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(107, 29);
            this.txt_name.TabIndex = 793;
            // 
            // frmNameModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 155);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_keyboard);
            this.Controls.Add(this.btn_entername);
            this.Controls.Add(this.btn_escname);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNameModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NameModel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNameModel_FormClosed);
            this.Load += new System.EventHandler(this.NameModel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btn_entername;
        internal System.Windows.Forms.Button btn_escname;
        internal System.Windows.Forms.Button btn_keyboard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_name;
    }
}