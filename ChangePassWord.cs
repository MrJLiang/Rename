using System;
using System.Windows.Forms;

namespace HAAGONtest
{
    public partial class ChangePassWord : Form
    {
        private string _Path = Application.StartupPath + "\\PassConfig.ini";

        public ChangePassWord()
        {
            InitializeComponent();
        }

        private void Btn_exit_Click(object sender, EventArgs e)
        {
            Login gLogin = new Login();
            gLogin.Show();
            this.Hide();
        }

        private void Change_Load(object sender, EventArgs e)
        {
        }

        private void Btn_reset_Click(object sender, EventArgs e)
        {
            if (Cbx_ResetType.Text == "")
            {
                MessageBox.Show("请选择修改密码的类型!");
                return;
            }
            if (Txbox_Code.Text == "")
            {
                MessageBox.Show("请输入管理人员密码!");
                return;
            }
            if (Txbox_newCode.Text == "")
            {
                MessageBox.Show("请输入新密码!");
                return;
            }
            if (Txbox_newCode_comfirm.Text == "")
            {
                MessageBox.Show("请输入确认密码!");
                return;
            }

            if (Txbox_Code.Text == readIni(this._Path, "管理人员"))
            {
            }
            else
            {
                MessageBox.Show("管理人员密码错误!");
                return;
            }
            if (Txbox_newCode.Text == Txbox_newCode_comfirm.Text)
            {
                writeIni(this._Path, Cbx_ResetType.Text, Txbox_newCode.Text);
                MessageBox.Show("修改成功!");
                Login gLogin = new Login();
                gLogin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("新密码和确认密码不一致!");
            }
        }

        private string readIni(string path, string message)
        {
            string mData = IniAPI.INIGetStringValue(path, "Value", message, "-1");
            return mData;
        }

        private void writeIni(string path, string message, string mData)
        {
            IniAPI.INIWriteValue(path, "Value", message, mData);
        }

        private void Cbx_CodeShow_CheckedChanged(object sender, EventArgs e)
        {
            if (Cbx_CodeShow.Checked)
            {
                Txbox_newCode.PasswordChar = '\0';
                Txbox_newCode_comfirm.PasswordChar = '\0';
            }
            else
            {
                Txbox_newCode.PasswordChar = '*';
                Txbox_newCode_comfirm.PasswordChar = '*';
            }
        }
    }
}