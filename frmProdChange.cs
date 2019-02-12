using System;
using System.Windows.Forms;
using HalconDotNet;
namespace HAAGONtest
{
    public partial class frmProdChange : Form
    {
        string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/";
        public frmProdChange()
        {
            InitializeComponent();
        }

        private void frmProdChange_Load(object sender, EventArgs e)
        {
            SetFocustimer.Enabled = false;
            Mod_sys.Instance.gCCD_sys.InitCCDModelWindow(pic_CCDModel);

        }

        private void bt_RecModel_Click(object sender, EventArgs e)
        {
            SetFocustimer.Enabled = false;
            MessageBox.Show("请把电芯样品放上测试台!");
            Mod_sys.Instance.gCCD_sys.CCDCreateStart(txt_CCDCurrentModel.Text);
        }

        private void bt_Start_Click_1(object sender, EventArgs e)
        {
            SetFocustimer.Enabled = false;
            MessageBox.Show("请把量块放上测试台!");
            if (txt_Calibrate.Text == "")
            {
                MessageBox.Show("请输入量块尺寸!");
                return;
            }

            Mod_sys.Instance.gCCD_sys.CCDcalibrationStart(txt_Calibrate.Text);
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            SetFocustimer.Enabled = false;
            MessageBox.Show("保存成功!");
        }

        private void btn_OffsetStart_Click(object sender, EventArgs e)
        {
            SetFocustimer.Enabled = false;

            if (txt_X.Text == "" || txt_Y.Text == "")
            {
                MessageBox.Show("请输入偏移量!");
                return;
            }

            Mod_sys.Instance.gCCD_sys.CCDOffsetStart(txt_CCDCurrentModel.Text, txt_X.Text, txt_Y.Text);
        }

        private void txt_SaveSize_Click(object sender, EventArgs e)
        {
            if (txt_M.Text == "" || txt_DM.Text == "" || txt_LM.Text == "" || txt_DLM.Text == "")
            {
                MessageBox.Show("请把数据输入完整!");
                return;
            }
            IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "SizeConfig.ini", "Size", "M", txt_M.Text);
            IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "SizeConfig.ini", "Size", "LM", txt_LM.Text);
            IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "SizeConfig.ini", "Size", "DM", txt_DM.Text);
            IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "SizeConfig.ini", "Size", "DLM", txt_DLM.Text);
            MessageBox.Show("数据保存成功!");
        }

        private void SetFocustimer_Tick(object sender, EventArgs e)
        {
            Mod_sys.Instance.gCCD_sys.CCDSetFocus();
        }

        private void btn_StartVideo_Click(object sender, EventArgs e)
        {
            if (btn_StartVideo.Text == "开始")
            {
                btn_StartVideo.Text = "停止";
                MessageBox.Show("请把电芯样品放上测试台!");
                SetFocustimer.Enabled = true;
            }
            else
            {
                btn_StartVideo.Text = "开始";

                SetFocustimer.Enabled = false;
            }
        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;
            string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.步进运动).ToString() };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);
            double DX, DY, DZ, DU;
            double.TryParse(txt_DX.Text, out DX);
            double.TryParse(txt_DY.Text, out DY);
            double.TryParse(txt_DZ.Text, out DZ);
            double.TryParse(txt_DU.Text, out DU);

            if (sender == btn_PulX)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "0", DX.ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_SubX)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "0", (-DX).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_PulY)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "1", DY.ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_SubY)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "1", (-DY).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_PulZ)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "2", DZ.ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_SubZ)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "2", (-DZ).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_PulU)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "3", DU.ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            if (sender == btn_SubU)
            {
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "3", (-DU).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
        }

        private void btn_SelectTeachPoint_Click(object sender, EventArgs e)
        {
            Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;

            if (comboTeachPoint.Text == "")
            {
                MessageBox.Show("请选择示教点!");
                return;
            }
            string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.点位运动).ToString() };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

            string PointSelect = comboTeachPoint.SelectedIndex.ToString();
            string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), PointSelect };
            Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
        }

        private void gTCPRobot__TCPEvent(object sender, string e)
        {
            if (e == "TakePicture\r\n")
            {
                Mod_sys.Instance.gCCD_sys.CCDExcuteStart(Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text);
                return;
            }

            try
            {
                e = e.Replace(" ", "");
                string[] array1 = e.Split(new char[5] { 'X', 'Y', 'Z', 'U', 'V' });
                string show = "";
                foreach (var item in array1)
                {
                    show += item;
                }
                string[] array2 = show.Split(':');
                txt_XCurrent.Text = array2[1];
                txt_YCurrent.Text = array2[2];
                txt_ZCurrent.Text = array2[3];
                txt_UCurrent.Text = array2[4];

                textBlankX.Text = array2[1];
                textBlankY.Text = array2[2];
                textBlankZ.Text = array2[3];
                textBlankU.Text = array2[4];
                if (GetPostSts == true)
                {
                    Mod_sys.Instance.gCCD_sys.CCDResultEvent -= Mod_sys.Instance.gRun_sys.gCCD_sys__CCDProcessEvent;
                    BlankX = array2[1];
                    BlankY = array2[2];
                    BlankZ = array2[3];
                    BlankU = array2[4];
                    GetPostSts = false;
                }
                if (comboTeachPoint.Text == "")
                {
                    return;
                }



                IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", comboTeachPoint.Text, "X", array2[1]);
                IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", comboTeachPoint.Text, "Y", array2[2]);
                IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", comboTeachPoint.Text, "Z", array2[3]);
                IniAPI.INIWriteValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", comboTeachPoint.Text, "U", array2[4]);

                Mod_sys.Instance.gTCPRobot._TCPEvent -= gTCPRobot__TCPEvent;
            }
            catch
            {

            }

        }

        //private void btn_Test_Click(object sender, EventArgs e)
        //{
        //    if (!System.IO.File.Exists(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini"))   //文件不存在时，创建新文件，并写入文件标题
        //    {
        //        MessageBox.Show("请先进行示教！");
        //        return;
        //    }
        //    if (txt_Speed.Text == "")
        //        return;

        //    string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)(Enum_sys.MoveMode.调试运动)).ToString() };
        //    Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

        //    SendCoord();
        //}

        private void SendCoord()
        {
            try
            {
                string[] RunData = new string[18];

                RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
                RunData[1] = "5";

                RunData[2] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "A-拍照", "X", "-1");
                RunData[3] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "A-拍照", "Y", "-1");
                RunData[4] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "A-拍照", "Z", "-1");
                RunData[5] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "A-拍照", "U", "-1");
                if (RunData[4] == "0.000")
                {
                    MessageBox.Show("请完成拍照点的示教操作!");
                    return;
                }

                RunData[6] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "F-废品", "X", "-1");
                RunData[7] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "F-废品", "Y", "-1");
                RunData[8] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "F-废品", "Z", "-1");
                RunData[9] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "F-废品", "U", "-1");
                if (RunData[8] == "0.000")
                {
                    MessageBox.Show("请完成废品点的示教操作!");
                    return;
                }
                RunData[10] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "B-整形", "X", "-1");
                RunData[11] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "B-整形", "Y", "-1");
                RunData[12] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "B-整形", "Z", "-1");
                RunData[13] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "B-整形", "U", "-1");
                if (RunData[12] == "0.000")
                {
                    MessageBox.Show("请完成整形点的示教操作!");
                    return;
                }
                RunData[14] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "C-落料", "X", "-1");
                RunData[15] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "C-落料", "Y", "-1");
                RunData[16] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "C-落料", "Z", "-1");
                RunData[17] = IniAPI.INIGetStringValue(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini", "C-落料", "U", "-1");
                if (RunData[16] == "0.000")
                {
                    MessageBox.Show("请完成落料点的示教操作!");
                    return;
                }

                foreach (var item in RunData)
                {
                    if (item == "-1")
                    {
                        MessageBox.Show("请完成所有点的示教操作!");
                        return;
                    }
                }

                if (Convert.ToInt32(RunData[1]) <= 0 || Convert.ToInt32(RunData[1]) > 100)
                    return;

                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            catch
            { }
        }

        private void btn_EnterCoord_Click(object sender, EventArgs e)
        {
            //string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.补偿运动).ToString() };
            //Mod_sys.gTCPRobot.TCPSend(RunMode);

            //if (!System.IO.File.Exists(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini"))   //文件不存在时，创建新文件，并写入文件标题
            //{
            //    MessageBox.Show("请先进行示教！");
            //    return;
            //}
            //if (txt_Speed.Text == "")
            //    return;

            //SendCoord();

            Mod_sys.Instance.gCCD_sys.CCDFitCoord(txt_CCDCurrentModel.Text);
        }

        private void btn_Runfit_Click(object sender, EventArgs e)
        {
            string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.补偿运动).ToString() };

            Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

            if (!System.IO.File.Exists(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini"))   //文件不存在时，创建新文件，并写入文件标题
            {
                MessageBox.Show("请先进行示教！");
                return;
            }
            //if (txt_Speed.Text == "")
            //    return;

            SendCoord();
        }

        private void btn_vacuo_Click(object sender, EventArgs e)
        {
            if (btn_vacuo.Text == "吸真空")
            {
                btn_vacuo.Text = "破真空";
                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.步进运动).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "4", "0" };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
            else
            {
                btn_vacuo.Text = "吸真空";

                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.步进运动).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);
                string[] RunData = { ((int)Enum_sys.DateMode.发送数据).ToString(), "5", "0" };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }
        }


        bool GetPostSts = false;
        string BlankX, BlankY, BlankZ, BlankU;

        private void butSave_Click(object sender, EventArgs e)
        {
            double dx, dy, dz, du;


            double.TryParse(IniAPI.INIGetStringValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DX", "0"), out dx);
            double.TryParse(IniAPI.INIGetStringValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DY", "0"), out dy);
            double.TryParse(IniAPI.INIGetStringValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DZ", "0"), out dz);
            double.TryParse(IniAPI.INIGetStringValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DU", "0"), out du);


            dx = dx + Convert.ToDouble(textBlankX.Text) - Convert.ToDouble(BlankX);
            dy = dy + Convert.ToDouble(textBlankY.Text) - Convert.ToDouble(BlankY);
            dz = dz + Convert.ToDouble(textBlankZ.Text) - Convert.ToDouble(BlankZ) + 35;
            du = du + Convert.ToDouble(textBlankU.Text) - Convert.ToDouble(BlankU);

            IniAPI.INIWriteValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DX", dx.ToString("#0.000"));
            IniAPI.INIWriteValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DY", dy.ToString("#0.000"));
            IniAPI.INIWriteValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DZ", dz.ToString("#0.000"));
            IniAPI.INIWriteValue(ModelPath + txt_RobCurrentModel.Text + "FitCoordConfig.ini", "落料", "DU", du.ToString("#0.000"));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (butGetPost.Text == "获取位置")
            {
                Mod_sys.Instance.gfrmAutoRun.UNSAFT_CCD_MODE = false;
                GetPostSts = true;

                Mod_sys.Instance.gCCD_sys.CCDResultEvent += Mod_sys.Instance.gRun_sys.gCCD_sys__CCDProcessEvent;

                Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;
                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.下料补偿).ToString() };

                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

                if (!System.IO.File.Exists(ModelPath + txt_CCDCurrentModel.Text + "RunCoordConfig.ini"))   //文件不存在时，创建新文件，并写入文件标题
                {
                    MessageBox.Show("请先进行示教！");
                    return;
                }
                //if (txt_Speed.Text == "")
                //    return;

                SendCoord();
                butGetPost.Text = "下降";

                butSave.Enabled = false;
                return;
            }
            if (butGetPost.Text == "下降")
            {
                Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;
                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.下料补偿).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

                string[] RunData = new string[2];
                RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
                RunData[1] = "200";
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
                butGetPost.Text = "开真空";
                butSave.Enabled = true;
                return;

            }
            if (butGetPost.Text == "开真空")
            {
                Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;
                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.下料补偿).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

                string[] RunData = new string[2];
                RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
                RunData[1] = "300";
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
                butGetPost.Text = "上升";
                butSave.Enabled = true;
                return;

            }
            if (butGetPost.Text == "上升")
            {
                Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;
                string[] RunMode = { ((int)Enum_sys.DateMode.发送状态).ToString(), ((int)Enum_sys.MoveMode.下料补偿).ToString() };
                Mod_sys.Instance.gTCPRobot.TCPSend(RunMode);

                string[] RunData = new string[2];
                RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
                RunData[1] = "400";
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
                butGetPost.Text = "获取位置";
                butSave.Enabled = false;
                return;
            }

        }


    }
}