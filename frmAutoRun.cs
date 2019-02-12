using HalconDotNet;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace HAAGONtest
{
    public partial class frmAutoRun : Form
    {
        private string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/";
        string FatherPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY";
        string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/日志/";
        string NGPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/保存图片/";


        public frmAutoRun()
        {
            InitializeComponent();
        }


        private void frmAutoRun_Load(object sender, EventArgs e)
        {

            if (System.IO.File.Exists(LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "SystemLog.txt"))   //文件不存在时，创建新文件，并写入文件标题
            {
                txtbox_showLog.LoadFile(LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "SystemLog.txt", RichTextBoxStreamType.PlainText);
            }


            //初始化CCD窗口
            timer_updateForm.Enabled = true;
            timerRest.Enabled = true;

            txtbox_userName.Text = "操作人员";
            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 系统准备好，可以开始了!!!\r\n";

            rad_noload.Enabled = true;
            rad_halfloadbefore.Enabled = true;
            rad_allload.Enabled = true;
            rad_halfloadAfter.Enabled = true;


            txt_dx.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DX", "0");
            txt_dy.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DY", "0");
            txt_dz.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DZ", "0");
            txt_du.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DU", "0");
            BeginInvoke(new MethodInvoker(delegate
            {
                SaveLog();
            }));

        }


        private void btn_min_Click(object sender, EventArgs e)
        {
            Mod_sys.Instance.gfrmMain.WindowState = FormWindowState.Minimized;
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            Mod_sys.Instance.gLogin.Show();
        }

        private void btn_touchKeyboard_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        }

        private void btn_startSystem_Click(object sender, EventArgs e)
        {

            ReadTest = false;
            if (txtbox_productName.Text == "")
            {
                MessageBox.Show("请选择或新建产品型号!");
                return;
            }
            rad_noload.Enabled = false;
            rad_halfloadAfter.Enabled = false;
            rad_allload.Enabled = false;
            rad_halfloadbefore.Enabled = false;
            RESET_SYS = false;
            if (btn_startSystem.Text == "开始")
            {
                Thread.Sleep(40);
                StartSys();

            }
            else
            {
                PauseAndReast();
            }

            SaveLog();
        }

        private void PauseAndReast()
        {
            txtbox_status_after.Text = "停止...";
            txtbox_status_after.BackColor = System.Drawing.Color.Red;
            btn_startSystem.Text = "开始";
            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 机台暂停!!!\r\n";

            string[] RunDataP = { ((int)Enum_sys.DateMode.发送数据).ToString(), "0" };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunDataP);

            Mod_sys.Instance.gRun_sys.PauseSystem();
            btn_resetSystem.Enabled = true;
            btn_userlogin.Enabled = true;

            System.Threading.Thread.Sleep(200);

            rad_noload.Enabled = true;
            rad_halfloadAfter.Enabled = true;
            rad_allload.Enabled = true;
            rad_halfloadbefore.Enabled = true;
            RESET_SYS = true;
            TimeSave = 0;
            Runtime = 0;
            StartTime = 0;
            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 机台复位!!!\r\n";
            Mod_sys.Instance.gRun_sys.ResetSystem();

            string[] RunDataR = { ((int)Enum_sys.DateMode.发送数据).ToString(), "150" };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunDataR);

            SaveLog();
        }

        private void StartSys()
        {
            hWindow_Fit.RePaint += new ChoiceTech.Halcon.Control.DelegateRePaint(Mod_sys.Instance.gCCD_sys.PaintImage);

            timerPPM.Enabled = true;
            Mod_sys.Instance.gfrmProdChange.SetFocustimer.Enabled = false;
            Mod_sys.Instance.gRun_sys.ZhenKongWarn = false;

            txt_M.Text = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "SizeConfig.ini", "Size", "M", "-1");
            txt_LM.Text = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "SizeConfig.ini", "Size", "LM", "-1");
            txt_DM.Text = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "SizeConfig.ini", "Size", "DM", "-1");
            txt_DLM.Text = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "SizeConfig.ini", "Size", "DLM", "-1");

            txtbox_status_after.Text = "运行...";
            txtbox_status_after.BackColor = System.Drawing.Color.Lime;

            TimeSave = Runtime;
            btn_startSystem.Text = "暂停";

            StartTime = System.Environment.TickCount;

            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 机台启动!!!\r\n";

            Mod_sys.Instance.gRun_sys.StartSystem();

            btn_resetSystem.Enabled = false;
            btn_userlogin.Enabled = false;

            string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), txt_Speed.Text };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
        }

        public void SaveLog()
        {
            txtbox_showLog.Font = new Font("宋体", 10.5f);

            txtbox_showLog.SelectionStart = txtbox_showLog.Text.Length;
            txtbox_showLog.SelectionLength = 0;
            txtbox_showLog.Focus();
            txtbox_showLog.SaveFile(LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "SystemLog.txt", RichTextBoxStreamType.PlainText);
        }

        public bool RESET_SYS = true;

        private void btn_resetSystem_Click(object sender, EventArgs e)
        {


            rad_noload.Enabled = true;
            rad_halfloadAfter.Enabled = true;
            rad_allload.Enabled = true;
            rad_halfloadbefore.Enabled = true;
            RESET_SYS = true;
            TimeSave = 0;
            Runtime = 0;
            StartTime = 0;
            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 机台复位!!!\r\n";
            Mod_sys.Instance.gRun_sys.ResetSystem();

            string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "150" };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunData);

            SaveLog();
        }

        private string fileName;
        private double AllProdNum;

        private void btn_readcsv_Click(object sender, EventArgs e)
        {
            dataGridView_mainData.DataSource = null;

            dataGridView_mainData.Rows.Clear();
            dataGridView_mainData.Columns.Clear();
            opencsvDialog.Filter = "CSV文件|*.CSV";
            if (opencsvDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                this.dataGridView_mainData.DataSource = null;
                fileName = opencsvDialog.FileName;

                DataView dv = Mod_sys.Instance.gCSV_sys.OpenCSV(fileName).DefaultView;
                DataTable dt;
                this.dataGridView_mainData.DataSource = Mod_sys.Instance.gCSV_sys.OpenCSV(fileName);
                string filter = "状态 = '" + Enum_sys.ProdState.全部 + "'";
                dv.RowFilter = filter;
                dt = dv.ToTable();
                lbl_testTotalNum.Text = (dataGridView_mainData.RowCount - dt.Rows.Count).ToString();
                AllProdNum = (dataGridView_mainData.RowCount - dt.Rows.Count);
            }
        }

        private void btn_savecsv_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtshow = new DataTable();
                dtshow = (DataTable)dataGridView_mainData.DataSource;
                string fileName = opencsvDialog.FileName;

                Mod_sys.Instance.gCSV_sys.SaveCSV(dtshow, fileName);
                MessageBox.Show("保存成功!");
            }
            catch
            {
                return;
            }
        }

        private int TimeSave = 0;
        private int Runtime = 0;
        private int StartTime = 0;

        private void timer_updateForm_Tick(object sender, EventArgs e)
        {
            //测试时间更新
            int cycleTime_end = System.Environment.TickCount;
            lbl_runtime.Text = Math.Round(cycleTime_end / 3600000.0).ToString() + "h" +
                                  Math.Round(cycleTime_end / 60000 % 60.0).ToString() + "m" +
                                  Math.Round(cycleTime_end / 1000 % 60.0).ToString() + "s";

            if (btn_startSystem.Text == "暂停")
            {
                Runtime = System.Environment.TickCount - StartTime + TimeSave;
                lbl_autorunTimeCount.Text = Math.Round(Runtime / 3600000.0).ToString() + "h" +
                                      Math.Round(Runtime / 60000 % 60.0).ToString() + "m" +
                                      Math.Round(Runtime / 1000 % 60.0).ToString() + "s";
            }
        }

        //查看产能列表
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (fileName == null)
            {
                return;
            }
            DataView dv = Mod_sys.Instance.gCSV_sys.OpenCSV(fileName).DefaultView;
            DataTable dt;

            if (radioButton_showAll.Checked && sender == radioButton_showAll)
            {
                this.dataGridView_mainData.DataSource = Mod_sys.Instance.gCSV_sys.OpenCSV(fileName);
                string filter = "状态 = '" + Enum_sys.ProdState.全部 + "'";
                dv.RowFilter = filter;
                dt = dv.ToTable();
                lbl_testTotalNum.Text = (dataGridView_mainData.RowCount - dt.Rows.Count).ToString();
            }
            if (radioButton_showOK.Checked && sender == radioButton_showOK)
            {
                string filter = "状态 = '" + Enum_sys.ProdState.OK + "'";
                dv.RowFilter = filter;
                this.dataGridView_mainData.DataSource = dv;
                lbl_testTotalNum.Text = dataGridView_mainData.RowCount.ToString();

                double yieldrate = dataGridView_mainData.RowCount / AllProdNum;
                string result = string.Format("{0:0.00%}", yieldrate);//得到5.88%
                txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 电芯极耳良品率:" + result + "!!!\r\n";
            }
            if (radiobutton_showNG.Checked && sender == radiobutton_showNG)
            {
                string filter = "状态 = '" + Enum_sys.ProdState.NG + "'";
                dv.RowFilter = filter;
                this.dataGridView_mainData.DataSource = dv;
                lbl_testTotalNum.Text = dataGridView_mainData.RowCount.ToString();

                double defectrate = dataGridView_mainData.RowCount / AllProdNum;
                string result = string.Format("{0:0.00%}", defectrate);//得到5.88%
                txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 电芯极耳不良品率:" + result + "!!!\r\n";
            }

            if (radioButton_showFail.Checked && sender == radioButton_showFail)
            {
                string filter = "状态 = '" + Enum_sys.ProdState.异常 + "'";
                dv.RowFilter = filter;
                this.dataGridView_mainData.DataSource = dv;
                lbl_testTotalNum.Text = dataGridView_mainData.RowCount.ToString();

                double failrate = dataGridView_mainData.RowCount / AllProdNum;
                string result = string.Format("{0:0.00%}", failrate);//得到5.88%
                txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 电芯极耳异常率:" + result + "!!!\r\n";
            }
        }

        private void txtbox_showLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_EnterCoord_Click(object sender, EventArgs e)
        {
            double dx, dy, dz, du;

            double.TryParse(txt_dx.Text, out dx);
            double.TryParse(txt_dy.Text, out dy);
            double.TryParse(txt_dz.Text, out dz);
            double.TryParse(txt_du.Text, out du);

            IniAPI.INIWriteValue(ModelPath + txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DX", dx.ToString());
            IniAPI.INIWriteValue(ModelPath + txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DY", dy.ToString());
            IniAPI.INIWriteValue(ModelPath + txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DZ", dz.ToString());
            IniAPI.INIWriteValue(ModelPath + txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DU", du.ToString());
        }

        private void com_selectcoord_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (com_selectcoord.Text == "拍照")
            {
                labelX.Text = "左右(mm)";
                labelY.Text = "上下(mm)";
            }
            else
            {
                labelX.Text = "X(mm)";
                labelY.Text = "Y(mm)";
            }

            txt_dx.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DX", "0");
            txt_dy.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DY", "0");
            txt_dz.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DZ", "0");
            txt_du.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", com_selectcoord.Text, "DU", "0");
        }

        public bool UNSAFT_CCD_MODE;

        private void rad_load_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_noload.Checked && sender == rad_noload)
            {
                UNSAFT_CCD_MODE = false;
                SendRobotData(((int)Enum_sys.MoveMode.半程空跑).ToString());
            }
            if (rad_halfloadAfter.Checked && sender == rad_halfloadAfter)
            {
                UNSAFT_CCD_MODE = false;
                SendRobotData(((int)Enum_sys.MoveMode.后半程带料).ToString());
            }
            if (rad_halfloadbefore.Checked && sender == rad_halfloadbefore)
            {
                UNSAFT_CCD_MODE = true;
                SendRobotData(((int)Enum_sys.MoveMode.前半程带料).ToString());
            }
            if (rad_allload.Checked && sender == rad_allload)
            {
                UNSAFT_CCD_MODE = true;
                SendRobotData(((int)Enum_sys.MoveMode.全程带料).ToString());
            }
        }

        private void SendRobotData(string e)
        {
            try
            {
                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), e };

                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

                string[] RunData = new string[18];

                RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
                RunData[1] = "0";

                RunData[2] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "X", "-1");
                RunData[3] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "Y", "-1");
                RunData[4] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "Z", "-1");
                RunData[5] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "U", "-1");

                RunData[6] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "X", "-1");
                RunData[7] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "Y", "-1");
                RunData[8] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "Z", "-1");
                RunData[9] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "F-废品", "U", "-1");

                RunData[10] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "X", "-1");
                RunData[11] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "Y", "-1");
                RunData[12] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "Z", "-1");
                RunData[13] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "B-整形", "U", "-1");

                RunData[14] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "X", "-1");
                RunData[15] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "Y", "-1");
                RunData[16] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "Z", "-1");
                RunData[17] = IniAPI.INIGetStringValue(ModelPath + txtbox_productName.Text + "RunCoordConfig.ini", "C-落料", "U", "-1");

                foreach (var item in RunData)
                {
                    if (item == "-1")
                    {
                        MessageBox.Show("请完成所有点的示教操作!");
                        return;
                    }
                }
                if (RunData[1] == "")
                    return;
                if (Convert.ToInt32(RunData[1]) < 0 || Convert.ToInt32(RunData[1]) > 100)
                    return;

                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            catch
            { }
        }

        private void SpeedtrackBar_Scroll(object sender, EventArgs e)
        {
            txt_Speed.Text = SpeedtrackBar.Value.ToString();
        }


        public bool ReadTest;

        private void butReadTest_Click(object sender, EventArgs e)
        {
            hWindow_Fit.RePaint += new ChoiceTech.Halcon.Control.DelegateRePaint(Mod_sys.Instance.gCCD_sys.PaintImage);
            ReadTest = true;
            Mod_sys.Instance.gCCD_sys.CCDExcuteStart(Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text);
            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 图像预检测!!!\r\n";
            SaveLog();

        }

        public void NGSave()
        {
            if (radioNGOpen.Checked)
            {

                try
                {
                    HOperatorSet.WriteImage(Mod_sys.Instance.gCCD_sys.ShowImage, "bmp", 0, NGPath + txtbox_productName.Text + "-" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".bmp");

                    HOperatorSet.WriteImage(hWindow_Fit.HWindowID.DumpWindowImage(), "jpeg", 0, NGPath + txtbox_productName.Text + "-" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".jpg");
                }
                catch
                {
                }
                txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> NG图像已保存!!!\r\n";
                SaveLog();

            }
            else
            {

            }
        }

        private void butSavePic_Click(object sender, EventArgs e)
        {

            try
            {
                HOperatorSet.WriteImage(Mod_sys.Instance.gCCD_sys.ShowImage, "bmp", 0, NGPath + txtbox_productName.Text + "-" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".bmp");
                HOperatorSet.WriteImage(hWindow_Fit.HWindowID.DumpWindowImage(), "jpeg", 0, NGPath + txtbox_productName.Text + "-" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".jpg");
                txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 图像已保存!!!\r\n";
                SaveLog();
            }
            catch
            {
            }

        }

        private void butLoadPic_Click(object sender, EventArgs e)
        {


            Mod_sys.Instance.gCCD_sys.ShowImage.Dispose();
            Mod_sys.Instance.gCCD_sys.m_Image.Dispose();

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "jpg类型图片(*.jpg)|*.jpg|BMP类型图片(*.bmp)|*.bmp|PNG类型图片(*.png)|*.png";

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.ReadImage(out Mod_sys.Instance.gCCD_sys.ShowImage, openfile.FileName);
                HobjectToHimage(Mod_sys.Instance.gCCD_sys.ShowImage, ref Mod_sys.Instance.gCCD_sys.m_Image);
                hWindow_Fit.Image = Mod_sys.Instance.gCCD_sys.m_Image;
                hWindow_Fit.DispImageFit();
            }


            txtbox_showLog.Text += "[ViewInfo]" + DateTime.Now.ToString() + " --> 加载图像!!!\r\n";
            SaveLog();
        }
        private void HobjectToHimage(HObject hobject, ref HImage image)
        {
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
        }

        private void timerRest_Tick(object sender, EventArgs e)
        {
            if (Mod_sys.Instance.gHipotTest.ReadDataByte[0] == "0" && Mod_sys.Instance.gHipotTest.ReadDataByte[3] == "1")
            {
                PauseAndReast();
            }

            if (Mod_sys.Instance.gHipotTest.ReadDataByte[0] == "1" && Mod_sys.Instance.gHipotTest.ReadDataByte[3] == "0" && btn_startSystem.Text == "开始" && Mod_sys.Instance.gRun_sys.ZhenKongWarn == false)
            {
                Thread.Sleep(40);

                StartSys();

            }
        }

        private void timerPPM_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Mod_sys.Instance.gRun_sys.ProdCount == 0)
                {
                    return;
                }
                txt_speedps.Text = (60.0 / Mod_sys.Instance.gRun_sys.ProdCount).ToString("#0.00");
                txt_ppm.Text = (60.0 / Convert.ToDouble(txt_speedps.Text)).ToString("#0");
                Mod_sys.Instance.gRun_sys.ProdCount = 0;
            }
            catch
            {

            }

            //SpeedTime = (System.Environment.TickCount - TimeTemp) / 1000.0 / 3.0;
            //Mod_sys.Instance.gfrmAutoRun.txt_speedps.Text = SpeedTime.ToString();
        }
    }
}