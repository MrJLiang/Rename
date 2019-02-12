using System;
using System.IO;
using System.Windows.Forms;

namespace HAAGONtest
{
    internal class Run_sys
    {
        bool RunState = true;
        string FatherPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY";
        string StatisPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/产能统计/";
        string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/";
        string CSVStatisPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/产能统计/" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

        public void StartSystem()
        {
            Mod_sys.Instance.gCCD_sys.CCDResultEvent += gCCD_sys__CCDProcessEvent;
            Mod_sys.Instance.gTCPRobot._TCPEvent += gTCPRobot__TCPEvent;
            RunSystem();
        }

        public bool ZhenKongWarn = false;
        private void gTCPRobot__TCPEvent(object sender, string e)
        {
            if (e == "TakePicture\r\n")
                Mod_sys.Instance.gCCD_sys.CCDExcuteStart(Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text);
            if (e == "ZhenKongWrong\r\n")
            {
                ZhenKongWarn = true;
                MessageBox.Show("真空泄漏，请暂停复位，清理电芯后继续开始!", "真空泄漏");
            }
        }

        public void PauseSystem()
        {
            Mod_sys.Instance.gCCD_sys.CCDResultEvent -= gCCD_sys__CCDProcessEvent;
            Mod_sys.Instance.gTCPRobot._TCPEvent -= gTCPRobot__TCPEvent;
        }

        public void ResetSystem()
        {
            RunState = true;
        }

        private void RunSystem()
        {
            if (RunState)
            {
                ProdTotalNum = 0;
                Mod_sys.Instance.gCSV_sys.writeCSV(StatisPath + DateTime.Now.ToString("yyyy-MM-dd") + ".csv", "开始", DateTime.Now.ToLongTimeString().ToString(), "", "", "", "", Enum_sys.ProdState.全部);
                RunState = false;
            }
        }
        /// <summary>
        /// 机械手接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void gTCPRobot__TCPEvent(object sender, string e)
        //{
        //    //RunProcess = (Enum_sys.RobotState)Convert.ToInt16(e);
        //    //ReceiveState = true;
        //}

        int ProdTotalNum = 0;
        int TimeTemp = 0;
        double SpeedTime = 0;
        public int ProdCount = 0;

        string[] Names = { "C1", "C2", "C3", "C4", "C5", "C6", "C7" };
        string[] HeaderTexts = { "数目", "时间", "型号", "Al(mm)", "Ni(mm)", "宽度(mm)", "状态" };

        public void gCCD_sys__CCDProcessEvent(object sender, SendData e)
        {
            //if (ProdCount >= 15)
            //{
            //    ProdCount = 0;
            //    SpeedTime = (System.Environment.TickCount - TimeTemp) / 1000.0 / 3.0;
            //    Mod_sys.Instance.gfrmAutoRun.txt_speedps.Text = SpeedTime.ToString();
            //    Mod_sys.Instance.gfrmAutoRun.txt_ppm.Text = (60.0 / SpeedTime).ToString("#0.00");

            //}
            //TimeTemp = System.Environment.TickCount;

            Mod_sys.Instance.gfrmAutoRun.BeginInvoke(new MethodInvoker(delegate
            {


                if (Mod_sys.Instance.gfrmAutoRun.radioIS.Checked)
                {
                    Check_Action有极耳(e);
                }
                else
                {
                    Check_Action没极耳(e);
                }


                //if (radioButton_showAll.Checked)

                //    if (radioButton_showOK.Checked)
                //  Mod_sys.gCSV_sys.UpdataSheet(dataGridView_mainData, _CSVPath, Enum_sys.ProdQual.良好, Names, HeaderTexts);
                //if (radiobutton_showNG.Checked)
                //    Mod_sys.gCSV_sys.UpdataSheet(dataGridView_mainData, _CSVPath, Enum_sys.ProdQual.不良, Names, HeaderTexts);
                //if (radioButton_showFail.Checked)
                //    Mod_sys.gCSV_sys.UpdataSheet(dataGridView_mainData, _CSVPath, Enum_sys.ProdQual.异常, Names, HeaderTexts);

                // this.dataGridView_mainData.Sort(this.dataGridView_mainData.Columns["FirstName"], ListSortDirection.Ascending);
                // this.dataGridView_mainData.Sort(this.dataGridView_mainData.Columns[1], System.ComponentModel.ListSortDirection.Descending);
            }));
            //GC.Collect();  

        }

        private void Check_Action有极耳(SendData e)
        {
            double M = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "M", "-1"));
            double LM = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "LM", "-1"));
            double DM = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "DM", "-1"));
            double DLM = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "DLM", "-1"));

            string[] RunData = new string[15];

            double U;
            U = e.U - Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "拍照", "DU", "0"));


            Mod_sys.Instance.gfrmAutoRun.txt_XCurrent.Text = e.X.ToString("#0.000");
            Mod_sys.Instance.gfrmAutoRun.txt_YCurrent.Text = e.Y.ToString("#0.000");
            Mod_sys.Instance.gfrmAutoRun.txt_ZCurrent.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "Z", "-1");
            Mod_sys.Instance.gfrmAutoRun.txt_UCurrent.Text = U.ToString("#0.000");

            RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
            RunData[1] = Mod_sys.Instance.gfrmAutoRun.txt_Speed.Text;
            RunData[2] = (e.X).ToString("#0.000");
            RunData[3] = (e.Y).ToString("#0.000");
            RunData[4] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "拍照", "DZ", "0");
            RunData[5] = U.ToString("#0.000");

            RunData[6] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DX", "0");
            RunData[7] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DY", "0");
            RunData[8] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DZ", "0");
            RunData[9] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DU", "0");

            RunData[10] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DX", "0");
            RunData[11] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DY", "0");
            RunData[12] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DZ", "0");
            RunData[13] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DU", "0");

            if (e.located == false)
            {
                RunData[14] = ((int)Enum_sys.ProdState.异常 - 1).ToString();
                Mod_sys.Instance.gCCD_sys.ShowCellState = Enum_sys.ProdState.异常;
            }
            else
            {
                ProdCount++;

                ProdTotalNum++;
                if (e.MedM - M <= DM && e.MedLM - LM <= DLM && e.MedM - M >= -DM && e.MedLM - LM >= -DLM && e.measured == true)
                {

                    Mod_sys.Instance.gCSV_sys.writeCSV(CSVStatisPath, ProdTotalNum.ToString(), DateTime.Now.ToLongTimeString().ToString(), Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text, e.MedM.ToString("#0.00"), e.MedLM.ToString("#0.00"), e.Width.ToString("#0.00"), Enum_sys.ProdState.OK);
                    RunData[14] = ((int)Enum_sys.ProdState.OK - 1).ToString();
                    Mod_sys.Instance.gCCD_sys.ShowCellState = Enum_sys.ProdState.OK;
                }
                else
                {
                    Mod_sys.Instance.gCSV_sys.writeCSV(CSVStatisPath, ProdTotalNum.ToString(), DateTime.Now.ToLongTimeString().ToString(), Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text, e.MedM.ToString("#0.00"), e.MedLM.ToString("#0.00"), e.Width.ToString("#0.00"), Enum_sys.ProdState.NG);
                    RunData[14] = ((int)Enum_sys.ProdState.NG - 1).ToString();
                    Mod_sys.Instance.gCCD_sys.ShowCellState = Enum_sys.ProdState.NG;
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.DispImageFit();
                    Mod_sys.Instance.gfrmAutoRun.NGSave();
                }
                Mod_sys.Instance.gfrmAutoRun.lbl_testTotalNum.Text = ProdTotalNum.ToString();
                Mod_sys.Instance.gCSV_sys.UpdataSheet(Mod_sys.Instance.gfrmAutoRun.dataGridView_mainData, CSVStatisPath, Enum_sys.ProdState.全部, Names, HeaderTexts);
            }


            if (!Mod_sys.Instance.gfrmAutoRun.ReadTest)
            {
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }


        }

        private void Check_Action没极耳(SendData e)
        {
            string CSVStatisPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/产能统计/" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
            double M = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "M", "-1"));
            double LM = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "LM", "-1"));
            double DM = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "DM", "-1"));
            double DLM = Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "SizeConfig.ini", "Size", "DLM", "-1"));

            double U;
            U = e.U - Convert.ToDouble(IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "拍照", "DU", "0"));

            string[] RunData = new string[15];
            Mod_sys.Instance.gfrmAutoRun.txt_XCurrent.Text = e.X.ToString("#0.000");
            Mod_sys.Instance.gfrmAutoRun.txt_YCurrent.Text = e.Y.ToString("#0.000");
            Mod_sys.Instance.gfrmAutoRun.txt_ZCurrent.Text = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "RunCoordConfig.ini", "A-拍照", "Z", "-1");
            Mod_sys.Instance.gfrmAutoRun.txt_UCurrent.Text = U.ToString("#0.000");

            int Speed;
            int.TryParse(Mod_sys.Instance.gfrmAutoRun.txt_Speed.Text, out Speed);


            RunData[0] = ((int)Enum_sys.DateMode.发送数据).ToString();
            RunData[1] = Speed.ToString();
            RunData[2] = (e.X).ToString("#0.000");
            RunData[3] = (e.Y).ToString("#0.000");
            RunData[4] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "拍照", "DZ", "0");
            RunData[5] = U.ToString("#0.000");

            RunData[6] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DX", "0");
            RunData[7] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DY", "0");
            RunData[8] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DZ", "0");
            RunData[9] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "整形", "DU", "0");

            RunData[10] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DX", "0");
            RunData[11] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DY", "0");
            RunData[12] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DZ", "0");
            RunData[13] = IniAPI.INIGetStringValue(ModelPath + Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text + "FitCoordConfig.ini", "落料", "DU", "0");
            if (e.located == false)
            {
                RunData[14] = ((int)Enum_sys.ProdState.异常 - 1).ToString();
                Mod_sys.Instance.gCCD_sys.ShowCellState = Enum_sys.ProdState.异常;

            }
            else
            {
                ProdCount++;

                ProdTotalNum++;
                if (e.MedM == 0 || e.MedLM == 0)
                {
                    Mod_sys.Instance.gCSV_sys.writeCSV(CSVStatisPath, ProdTotalNum.ToString(), DateTime.Now.ToLongTimeString().ToString(), Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text, e.MedM.ToString("#0.00"), e.MedLM.ToString("#0.00"), e.Width.ToString("#0.00"), Enum_sys.ProdState.异常);
                    RunData[14] = ((int)Enum_sys.ProdState.NG - 1).ToString();
                    Mod_sys.Instance.gCCD_sys.ShowCellState = Enum_sys.ProdState.NG;
                    Mod_sys.Instance.gfrmAutoRun.hWindow_Fit.DispImageFit();
                    Mod_sys.Instance.gfrmAutoRun.NGSave();

                }
                else
                {
                    Mod_sys.Instance.gCSV_sys.writeCSV(CSVStatisPath, ProdTotalNum.ToString(), DateTime.Now.ToLongTimeString().ToString(), Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text, e.MedM.ToString("#0.00"), e.MedLM.ToString("#0.00"), e.Width.ToString("#0.00"), Enum_sys.ProdState.OK);
                    RunData[14] = ((int)Enum_sys.ProdState.OK - 1).ToString();
                    Mod_sys.Instance.gCCD_sys.ShowCellState = Enum_sys.ProdState.OK;
                }
                Mod_sys.Instance.gfrmAutoRun.lbl_testTotalNum.Text = ProdTotalNum.ToString();
                Mod_sys.Instance.gCSV_sys.UpdataSheet(Mod_sys.Instance.gfrmAutoRun.dataGridView_mainData, CSVStatisPath, Enum_sys.ProdState.全部, Names, HeaderTexts);
            }
            if (!Mod_sys.Instance.gfrmAutoRun.ReadTest)
            {
                Mod_sys.Instance.gTCPRobot.TCPSend(RunData);
            }

        }
    }
}