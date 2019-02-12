using System;
using System.Windows.Forms;

namespace HAAGONtest
{
    public partial class Login : Form
    {
        private string _iniPath = Application.StartupPath + "\\PassConfig.ini";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {


            txtbox_password.TabIndex = 0;
            txtbox_password.Focus();

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btn_resetCode_Click(object sender, EventArgs e)
        {
            Mod_sys.Instance.gChangePassWord.Show();
            this.Hide();
        }

        private void checkBox_isShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_isShowPassword.Checked) txtbox_password.PasswordChar = '\0';
            else txtbox_password.PasswordChar = '*';
        }



        private void btn_login_Click(object sender, EventArgs e)
        {
            if (comboBox_loginLevel.Text == "")
            {
                MessageBox.Show("请选择登录类型!");
                return;
            }
            else if (txtbox_password.Text == "")
            {
                MessageBox.Show("请输入登录密码!");
                return;
            }
            try
            {
                if (txtbox_password.Text == readIni(this._iniPath, comboBox_loginLevel.Text))
                {
                    Mod_sys.Instance.gfrmAutoRun.txtbox_userName.Text = comboBox_loginLevel.Text;
                }
                else
                {
                    MessageBox.Show("密码错误!");
                }
            }
            catch
            {
                MessageBox.Show("请选择登录类型!");
            }
        }

        private string readIni(string path, string message)
        {
            string mData = IniAPI.INIGetStringValue(path, "Value", message, "-1");
            return mData;
        }

        private void btn_touchKeyBoard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        }

        private void txtbox_password_TextChanged(object sender, EventArgs e)
        {
            if (txtbox_password.Text == readIni(this._iniPath, comboBox_loginLevel.Text))
            {
                Mod_sys.Instance.gfrmAutoRun.txtbox_userName.Text = comboBox_loginLevel.Text;
                this.Hide();
            }
        }

        private void comboBox_loginLevel_TextChanged(object sender, EventArgs e)
        {
            txtbox_password.Focus();
        }
    }
}