using System;
using System.Windows.Forms;

namespace HAAGONtest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Text == "暂停")
            //{
            //    //if (MessageBox.Show("请确保机台停止!", "关闭窗口", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
            //    //{
            //    //    e.Cancel = true;
            //    //}
            //    //else
            //    //{
            //    //    e.Cancel = true;
            //    //}

            //    MessageBox.Show("请确保机台停止运行!", "关闭窗口");
            //    e.Cancel = true;
            //}
            //else
            //{
            //    Mod_sys.Instance.gHipotTest.HipotExcuteState = false;
            //    
            //    Mod_sys.Instance.gCCD_sys.CCDClose();

            //}

            Mod_sys.Instance.gHipotTest.HipotExcuteState = false;
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
         
            SendRobotData();

            Mod_sys.Instance.gHipotTest.PLCInit();

            tabControl1.SelectTab(0);
            Mod_sys.Instance.gfrmAutoRun.MdiParent = this;
            Mod_sys.Instance.gfrmAutoRun.Parent = this.tabP_AutoRun;
            //Mod_sys.gfrmAutoRun.Dock = DockStyle.Fill;
            Mod_sys.Instance.gfrmAutoRun.Show();
        }

        private string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/";

        private void SendRobotData()
        {
            string[] RunMode = new string[2];
            try
            {
                if (Mod_sys.Instance.gfrmAutoRun.rad_noload.Checked)
                {
                    Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE = false;
                    RunMode[0] = ((int)Enum_sys.DateMode.发送状态).ToString();
                    RunMode[1] = ((int)Enum_sys.MoveMode.半程空跑).ToString();
                }
                if (Mod_sys.Instance.gfrmAutoRun.rad_halfloadAfter.Checked)
                {
                    Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE = false;
                    RunMode[0] = ((int)Enum_sys.DateMode.发送状态).ToString();
                    RunMode[1] = ((int)Enum_sys.MoveMode.后半程带料).ToString();
                }
                if (Mod_sys.Instance.gfrmAutoRun.rad_halfloadbefore.Checked)
                {
                    Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE = true;
                    RunMode[0] = ((int)Enum_sys.DateMode.发送状态).ToString();
                    RunMode[1] = ((int)Enum_sys.MoveMode.前半程带料).ToString();
                }
                if (Mod_sys.Instance.gfrmAutoRun.rad_allload.Checked)
                {
                    Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE = true;
                    RunMode[0] = ((int)Enum_sys.DateMode.发送状态).ToString();
                    RunMode[1] = ((int)Enum_sys.MoveMode.全程带料).ToString();
                }


                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

                string[] RunData = new string[18];

                RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
                RunData[1] = "0";

                RunData[2] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "X", "-1");
                RunData[3] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "Y", "-1");
                RunData[4] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "Z", "-1");
                RunData[5] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "U", "-1");
                if (RunData[4] == "0.000")
                {
                    MessageBox.Show("请完成拍照点的示教操作!");
                    Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Enabled = false;

                    return;
                }
                RunData[6] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "X", "-1");
                RunData[7] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "Y", "-1");
                RunData[8] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "Z", "-1");
                RunData[9] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "U", "-1");
                if (RunData[8] == "0.000")
                {
                    MessageBox.Show("请完成废品点的示教操作!");
                    Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Enabled = false;

                    return;
                }
                RunData[10] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "X", "-1");
                RunData[11] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "Y", "-1");
                RunData[12] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "Z", "-1");
                RunData[13] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "U", "-1");
                if (RunData[12] == "0.000")
                {
                    MessageBox.Show("请完成整形点的示教操作!");
                    Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Enabled = false;

                    return;
                }
                RunData[14] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "X", "-1");
                RunData[15] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "Y", "-1");
                RunData[16] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "Z", "-1");
                RunData[17] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "U", "-1");
                if (RunData[16] == "0.000")
                {
                    MessageBox.Show("请完成落料点的示教操作!");
                    Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Enabled = false;

                    return;
                }
                foreach (var item in RunData)
                {
                    if (item == "-1")
                    {
                        MessageBox.Show("请完成所有点的示教操作!");
                        Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Enabled = false;
                        return;
                    }
                }
                if (RunData[1] == "")
                    return;

                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
                Mod_sys.Instance.gfrmAutoRun.btn_startSystem.Enabled = true;

            }
            catch
            { }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Mod_sys.Instance.gfrmAutoRun.RESET_SYS)
            {
                //MessageBox.Show("请确保机台复位!");
                tabControl1.SelectTab(0);
                return;
            }
            if (tabControl1.SelectedIndex == 0)
            {
                SendRobotData();
            }
            if (tabControl1.SelectedIndex == 1)
            {
                if (Mod_sys.Instance.gfrmAutoRun.txtbox_userName.Text != "调试人员")
                {
                    MessageBox.Show("需要调试人员权限!");
                    tabControl1.SelectTab(0);
                    return;
                }

                Mod_sys.Instance.gfrmProdChange.MdiParent = Mod_sys.Instance.gfrmMain;
                Mod_sys.Instance.gfrmProdChange.Parent = Mod_sys.Instance.gfrmMain.TabP_ProdChange;
                // Mod_sys.gfrmProdChange.Dock = DockStyle.Fill;
                Mod_sys.Instance.gfrmProdChange.Show();
                Mod_sys.Instance.gfrmNameModel.Show();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                Mod_sys.Instance.gHipotTest.MdiParent = Mod_sys.Instance.gfrmMain;
                Mod_sys.Instance.gHipotTest.Parent = Mod_sys.Instance.gfrmMain.TabP_ConnectTest;
                Mod_sys.Instance.gHipotTest.Dock = DockStyle.Fill;
                Mod_sys.Instance.gHipotTest.Show();
            }
        }
    }
}