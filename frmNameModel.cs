using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HAAGONtest
{
    public partial class frmNameModel : Form
    {
        public frmNameModel()
        {
            InitializeComponent();
        }

        private void NameModel_Load(object sender, EventArgs e)
        {

            txt_name.Text = Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text;
        }

        private void btn_entername_Click(object sender, EventArgs e)
        {
            string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/";

            //DataTable dtModel = new DataTable();
            //string CSVModelPath = ModelPath + "/产品型号.csv";

            //if (!System.IO.File.Exists(CSVModelPath))   //文件不存在时，创建新文件，并写入文件标题
            //{
            //    //创建文件流对象，
            //    FileStream fs = new FileStream(CSVModelPath, FileMode.Create, FileAccess.Write);
            //    //创建文件流写入对象，绑定文件流对象
            //    StreamWriter sw = new StreamWriter(fs);
            //    //创建数据对象
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("ID");
            //    //把标题内容写入到文件流中
            //    sw.WriteLine(sb);
            //    sw.Flush();
            //    sw.Close();
            //    fs.Close();
            //}
            //dtModel = Mod_sys.Instance.gCSV_sys.OpenCSV(CSVModelPath);
            //for (int i = dtModel.Rows.Count - 1; i >= 0; i--)
            //{
            //    if (txt_name.Text == dtModel.Rows[i].ItemArray[0].ToString())
            //    {
            //        MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            //        DialogResult dr = MessageBox.Show("设备已存在该型号，是否覆盖？", "创建型号", messButton);
            //        if (dr == DialogResult.OK)//如果点击“确定”按钮
            //        {
            //            dtModel.Rows[i].Delete();
            //            Mod_sys.Instance.gCSV_sys.SaveCSV(dtModel, CSVModelPath);
            //        }
            //        else//如果点击“取消”按钮
            //        {
            //            return;
            //        }
            //    }
            //}
            if (txt_name.Text != "")
            {
                Mod_sys.Instance.gfrmProdChange.txt_CCDCurrentModel.Text = txt_name.Text;
                Mod_sys.Instance.gfrmProdChange.txt_RobCurrentModel.Text = txt_name.Text;
                // Mod_sys.Instance.gCSV_sys.writeCSV(CSVModelPath, txt_name.Text);
                Mod_sys.Instance.gfrmProdChange.txt_M.Text = IniAPI.INIGetStringValue(ModelPath + txt_name.Text + "SizeConfig.ini", "Size", "M", "-1");
                Mod_sys.Instance.gfrmProdChange.txt_LM.Text = IniAPI.INIGetStringValue(ModelPath + txt_name.Text + "SizeConfig.ini", "Size", "LM", "-1");
                Mod_sys.Instance.gfrmProdChange.txt_DM.Text = IniAPI.INIGetStringValue(ModelPath + txt_name.Text + "SizeConfig.ini", "Size", "DM", "-1");
                Mod_sys.Instance.gfrmProdChange.txt_DLM.Text = IniAPI.INIGetStringValue(ModelPath + txt_name.Text + "SizeConfig.ini", "Size", "DLM", "-1");

                this.Hide();
            }
            else
            {
                MessageBox.Show("请输入电芯型号！");
            }



        }

        private void btn_escname_Click(object sender, EventArgs e)
        {
            this.Hide();
            Mod_sys.Instance.gfrmMain.tabControl1.SelectTab(0);
        }

        private void frmNameModel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mod_sys.Instance.gfrmMain.tabControl1.SelectTab(0);
        }

        private void btn_keyboard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        }
    }
}