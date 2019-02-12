using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using HAAGONtest;

namespace Hipot
{
    public partial class HipotTest : Form
    {
        public string[] ReadDataByte = new string[16] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        public string[] WriteDataByte = new string[16] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/日志/";


        public HipotTest()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void HipotTest_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "HipotLog.txt"))   //文件不存在时，创建新文件，并写入文件标题
            {
                txtbox_showLog.LoadFile(LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "HipotLog.txt", RichTextBoxStreamType.PlainText);
            }
            //OpenPort();

            // PLCInit();
        }

        public void PLCInit()
        {
            this.CenterToScreen();
            cmbReadMry.Items.Clear();
            cmbReadMry.Items.Add("CIO");
            cmbReadMry.Items.Add("WR");
            cmbReadMry.Items.Add("HR");
            cmbReadMry.Items.Add("DR");
            cmbReadMry.Items.Add("TIM");
            cmbReadMry.Items.Add("CNT");
            cmbReadMry.Items.Add("E0");
            cmbReadMry.Items.Add("E1");
            cmbWriteMry.Items.Clear();
            cmbWriteMry.Items.Add("CIO");
            cmbWriteMry.Items.Add("WR");
            cmbWriteMry.Items.Add("HR");
            cmbWriteMry.Items.Add("DR");
            cmbWriteMry.Items.Add("TIM");
            cmbWriteMry.Items.Add("CNT");
            cmbWriteMry.Items.Add("E0");
            cmbWriteMry.Items.Add("E1");
            cmbReadType.Items.Clear();
            cmbReadType.Items.Add("INT16");
            cmbReadType.Items.Add("UINT16");
            cmbReadType.Items.Add("DINT32");
            cmbReadType.Items.Add("HEX32");
            cmbReadType.Items.Add("REAL32");
            cmbReadType.Items.Add("BIN16");
            cmbReadType.Items.Add("BCD16");
            cmbReadType.Items.Add("BCD32");
            cmbWriteType.Items.Clear();
            cmbWriteType.Items.Add("INT16");
            cmbWriteType.Items.Add("UINT16");
            cmbWriteType.Items.Add("DINT32");
            cmbWriteType.Items.Add("HEX32");
            cmbWriteType.Items.Add("REAL32");
            cmbWriteType.Items.Add("BIN16");
            cmbWriteType.Items.Add("BCD16");
            cmbWriteType.Items.Add("BCD32");
            cmbReadMry.SelectedIndex = 1;
            cmbWriteMry.SelectedIndex = 1;
            cmbReadType.SelectedIndex = 5;
            cmbWriteType.SelectedIndex = 5;
            lstRead.Items.Clear();
            txtWrite.Text = "";

            PLCReadStart();

        }

        public void PLCConnectInit()
        {
            short re = 0;
            string restr = "";
            re = PLC.EntLink(txtLocalIP.Text.Trim(), Convert.ToUInt16(txtLocalPort.Text), txtRemoteIP.Text.Trim(), (Convert.ToUInt16(txtRemotePort.Text)), "LFLB25AFQEPTITQWEUQ180505FINS/TCP/V34", ref PlcHand);
            txtReLink.Text = re.ToString();
            if (re == 0)
            {
                EntLink = true;
            }
            else
            {
                EntLink = false;
                throw new Exception();

            }
        }

        public void OpenPort()
        {
            //serialPort1.PortName = SearchAndAddSerialToComboBox(serialPort1);
            try
            {
                serialPort1.PortName = "COM3";
                serialPort1.BaudRate = Convert.ToInt32("19200");

                serialPort1.Open();
                serialPort1.DiscardInBuffer();
            }
            catch
            {
                throw new Exception();


            }

        }


        private void OnPort_Click(object sender, EventArgs e)
        {
            try
            {
                OnPort.Enabled = false;
                OffPort.Enabled = true;
            }
            catch
            {
                MessageBox.Show("端口错误，请检查端口", "错误");
            }
        }

        private void com_Port_Click(object sender, EventArgs e)
        {
        }

        private string SearchAndAddSerialToComboBox(SerialPort MyPort)
        {
            string Buffer;
            for (int i = 1; i < 21; i++)
            {
                try
                {
                    Buffer = "COM" + i.ToString();
                    MyPort.PortName = Buffer;
                    MyPort.Open();
                    MyPort.Close();
                    return Buffer;

                }
                catch { }
            }
            return null;
        }

        private void OffPort_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();
                OnPort.Enabled = true;
                OffPort.Enabled = false;
            }
            catch
            {
            }
        }


        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            BeginInvoke(new MethodInvoker(delegate
            {
                int BufferSize = serialPort1.BytesToRead;
                string[] str = new string[BufferSize];
                for (int i = 0; i < BufferSize; i++)
                {
                    byte data;
                    data = (byte)serialPort1.ReadByte();//此处需要强制类型转换，将(int)类型数据转换为(byte类型数据，不必考虑是否会丢失数据                
                    str[i] = Convert.ToString(data, 16).ToUpper();//转换为大写十六进制字符串                

                }


                //for (int i = 0; i < str.Length; i++)
                //{
                //    txtbox_showLog.AppendText("0x" + (str[i].Length == 1 ? "0" + str[i] : str[i]) + " ");//空位补“0”   
                //}
                //txtbox_showLog.Text += "\r\n";

                serialPort1.DiscardInBuffer();
                ShowIRData(StaticIRData(str, new IRData()));
                ShowOSData(StaticOSData(str, new OSData()));
            }));



        }

        public struct IRData
        {
            public bool TestState;
            public bool IRState;
            public double IRVolt;
            public double IRohm;
            public double TestTime;

        }
        public IRData StaticIRData(string[] str, IRData e)
        {
            if (str.Length == 25 && str[9] == "3")
            {
                e.TestState = true;

                if (str[7] == "74")
                {
                    e.IRState = true;
                }
                else
                {
                    e.IRState = false;

                }
                e.IRVolt = 0;
                e.IRohm = 0;
                e.TestTime = 0;
                //e.IRVolt = Convert.ToDouble(Convert.ToByte(str[11])) * 256 + Convert.ToDouble(Convert.ToByte(str[10]));
                e.IRohm = 0.0001 * (HexStringtoInt(str[15]) * 256 * 256 * 256 + HexStringtoInt(str[14]) * 256 * 256 + HexStringtoInt(str[13]) * 256 + HexStringtoInt(str[12]));
                //e.TestTime = Convert.ToDouble(Convert.ToByte(str[21])) * 256 + Convert.ToDouble(Convert.ToByte(str[20]));
                return e;
            }
            else
            {
                e.TestState = false;
                e.IRState = false;
                e.IRVolt = 0;
                e.IRohm = 0;
                e.TestTime = 0;
                return e;
            }
        }

        public struct OSData
        {
            public bool TestState;
            public bool OSState;
            public double OSVolt;
            public double OSpF;
            public double TestTime;

        }
        public OSData StaticOSData(string[] str, OSData e)
        {
            if (str.Length == 19 && str[9] == "6")
            {
                e.TestState = true;

                if (str[7] == "74")
                {
                    e.OSState = true;
                }
                else
                {
                    e.OSState = false;
                }
                e.OSVolt = 0;
                e.OSpF = 0;
                e.TestTime = 0;
                //e.IRVolt = Convert.ToDouble(Convert.ToByte(str[11])) * 256 + Convert.ToDouble(Convert.ToByte(str[10]));
                //e.IRohm = Convert.ToDouble(Convert.ToByte(str[15])) * 256 * 256 * 256 + Convert.ToDouble(Convert.ToByte(str[14])) * 256 * 256 + Convert.ToDouble(Convert.ToByte(str[13])) * 256 + Convert.ToDouble(Convert.ToByte(str[12]));
                //e.TestTime = Convert.ToDouble(Convert.ToByte(str[21])) * 256 + Convert.ToDouble(Convert.ToByte(str[20]));
                return e;
            }
            else
            {
                e.TestState = false;
                e.OSState = false;
                e.OSVolt = 0;
                e.OSpF = 0;
                e.TestTime = 0;
                return e;
            }
        }
        //**********************************************显示结果**********************************************//
        private void ShowIRData(IRData e)
        {
            if (e.TestState)
            {
                if (e.IRState)
                {
                    txtIRState.Text = "PASS";
                }
                else
                {
                    txtIRState.Text = "FAIL";

                }
                txtIRVolt.Text = e.IRVolt.ToString();
                txtIRohm.Text = e.IRohm.ToString();
            }

        }

        private void ShowOSData(OSData e)
        {
            if (e.TestState)
            {
                if (e.OSState)
                {
                    txtOSState.Text = "PASS";
                }
                else
                {
                    txtOSState.Text = "FAIL";

                }
                txtOSVolt.Text = e.OSVolt.ToString();
                txtOSpF.Text = e.OSpF.ToString();
                butStart.Enabled = true;
                SendTestData();
            }

        }
        //**********************************************发送测试数据**********************************************//
        private void SendTestData()
        {
            timerOut.Enabled = false;
            if (txtOSState.Text == "PASS" && txtIRState.Text == "PASS")
            {
                WriteDataByte[0] = "0";
                WriteDataByte[1] = "1";
                WriteDataByte[2] = "1";
                WriteDataByte[3] = "0";
                WritePLC(WriteDataByte);
            }
            else
            {
                serialPort1.ReceivedBytesThreshold = 7;
                StopCommand();
                WriteDataByte[0] = "0";
                WriteDataByte[1] = "1";
                WriteDataByte[2] = "0";
                WriteDataByte[3] = "1";
                WritePLC(WriteDataByte);

            }
            HipotTestFinished = true;

            txtbox_showLog.Text += DateTime.Now.ToString() + " --> IR: " + txtIRState.Text + "  OS: " + txtOSState.Text + "  ohm: " + txtIRohm.Text + "Gohm\r\n";
            txtbox_showLog.Font = new Font("宋体", 10.5f);
            txtbox_showLog.SelectionStart = txtbox_showLog.Text.Length;
            txtbox_showLog.SelectionLength = 0;
            txtbox_showLog.Focus();
            txtbox_showLog.SaveFile(LogPath + DateTime.Now.ToString("yyyy-MM-dd") + "HipotLog.txt", RichTextBoxStreamType.PlainText);

            Mod_sys.Instance.gfrmAutoRun.BeginInvoke(new MethodInvoker(delegate
            {
                Mod_sys.Instance.gfrmAutoRun.txtbox_showLog.Text += DateTime.Now.ToString() + " --> IR: " + txtIRState.Text + "  OS: " + txtOSState.Text + "  ohm: " + txtIRohm.Text + "Gohm\r\n";

                Mod_sys.Instance.gfrmAutoRun.SaveLog();
            }));
        }

        private void butStart_Click(object sender, EventArgs e)
        {
            HipotTestExcute();

        }
        private void HipotTestExcute()
        {
            //butStart.Enabled = false;
            txtIRState.Text = "";
            txtOSState.Text = "";

            serialPort1.ReceivedBytesThreshold = 7;
            StartCommand();
            timerIR.Enabled = true;
            timerOS.Enabled = false;
            timerOut.Enabled = true;
        }

        private void butStop_Click(object sender, EventArgs e)
        {
            HipotTestStop();
        }

        private void HipotTestStop()
        {
            serialPort1.ReceivedBytesThreshold = 7;
            StopCommand();
        }

        public void StartCommand()
        {
            byte[] data = new byte[6] { 0xab, 0x01, 0x70, 0x01, 0x22, 0x6c };
            serialPort1.Write(data, 0, data.Length);
        }

        public void StopCommand()
        {
            try
            {
                byte[] data = new byte[6] { 0xab, 0x01, 0x70, 0x01, 0x21, 0x6d };
                serialPort1.Write(data, 0, data.Length);
            }
            catch
            {

            }

        }

        public void ResultIRCommand()
        {
            byte[] data = new byte[8] { 0xab, 0x01, 0x70, 0x03, 0xb1, 0x01, 0xf7, 0xe3 };
            serialPort1.Write(data, 0, data.Length);
        }
        public void ResultOSCommand()
        {
            byte[] data = new byte[8] { 0xab, 0x01, 0x70, 0x03, 0xb1, 0x02, 0x47, 0x92 };
            serialPort1.Write(data, 0, data.Length);
        }

        public void ConnectTest()
        {
            byte[] data = new byte[6] { 0xab, 0x01, 0x70, 0x01, 0x20, 0x6e };
            serialPort1.Write(data, 0, data.Length);
        }

        private void butConnect_Click(object sender, EventArgs e)
        {
            ConnectTest();
        }

        private void butGetData_Click(object sender, EventArgs e)
        {
            serialPort1.ReceivedBytesThreshold = 25;
            ResultIRCommand();

        }

        private void butGetOSData_Click(object sender, EventArgs e)
        {
            serialPort1.ReceivedBytesThreshold = 19;
            ResultOSCommand();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerIR.Enabled = false;
            serialPort1.ReceivedBytesThreshold = 25;
            ResultIRCommand();
            timerOS.Enabled = true;

        }

        private void timerOS_Tick(object sender, EventArgs e)
        {
            timerOS.Enabled = false;
            serialPort1.ReceivedBytesThreshold = 19;
            ResultOSCommand();
        }

        int HexStringtoInt(string str)
        {

            string[] temp = new string[2] { "0", "0" };
            for (int i = 0; i < str.Length; i++)
            {

                temp[i] = str.Substring(i, 1);


            }

            int[] tempint = new int[2] { 0, 0 };

            for (int i = 0; i < str.Length; i++)
            {
                switch (temp[i])
                {
                    case "0":
                        tempint[i] = 0;
                        break;
                    case "1":
                        tempint[i] = 1;
                        break;
                    case "2":
                        tempint[i] = 2;
                        break;
                    case "3":
                        tempint[i] = 3;
                        break;
                    case "4":
                        tempint[i] = 4;
                        break;
                    case "5":
                        tempint[i] = 5;
                        break;
                    case "6":
                        tempint[i] = 6;
                        break;
                    case "7":
                        tempint[i] = 7;
                        break;
                    case "8":
                        tempint[i] = 8;
                        break;
                    case "9":
                        tempint[i] = 9;
                        break;
                    case "A":
                        tempint[i] = 10;
                        break;
                    case "B":
                        tempint[i] = 11;
                        break;
                    case "C":
                        tempint[i] = 12;
                        break;
                    case "D":
                        tempint[i] = 13;
                        break;
                    case "E":
                        tempint[i] = 14;
                        break;
                    case "F":
                        tempint[i] = 15;
                        break;
                }
            }
            if (str.Length == 2)
            {
                return tempint[0] * 16 + tempint[1];
            }
            else
            {
                return tempint[0];
            }
        }

        //**********************************PLC****************************//
        FinsTcp.PlcClient PLC = new FinsTcp.PlcClient();
        bool EntLink;
        Int32 PlcHand;

        private void PLCReadStart()
        {

            _PLCreadThread = new Thread(new ThreadStart(PLCreadThread));
            _PLCreadThread.Start();
        }


        private void butLink_Click(object sender, EventArgs e)
        {

            short re = 0;
            string restr = "";
            re = PLC.EntLink(txtLocalIP.Text.Trim(), Convert.ToUInt16(txtLocalPort.Text), txtRemoteIP.Text.Trim(), (Convert.ToUInt16(txtRemotePort.Text)), "LFLB25AFQEPTITQWEUQ180505FINS/TCP/V34", ref PlcHand);
            txtReLink.Text = re.ToString();
            if (re == 0)
            {
                EntLink = true;
                MessageBox.Show("PLC联接成功!");
            }
            else
            {
                EntLink = false;
                MessageBox.Show("PLC联接失败: " + restr);
            }
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            short re = 0;
            EntLink = false;
            re = PLC.DeLink(PlcHand);
            txtReClose.Text = re.ToString();
        }

        private void butRead_Click(object sender, EventArgs e)
        {
            //ReadData();


            _PLCreadThread = new Thread(new ThreadStart(PLCreadThread));
            _PLCreadThread.Start();
        }

        private void butWrite_Click(object sender, EventArgs e)
        {
            WriteDataByte[0] = "1";
            WriteDataByte[1] = "1";
            WriteDataByte[2] = "1";
            WriteDataByte[3] = "1";
            WritePLC(WriteDataByte);
        }



        private void ReadData()
        {
            try
            {
                short re = 0;
                string str = "";
                object[] RD = null;
                if (!EntLink)
                {

                    MessageBox.Show("还未与PLC建立联接！");
                    return;
                }
                int var1 = cmbReadMry.SelectedIndex + 1;
                FinsTcp.PlcClient.PlcMemory mry = (FinsTcp.PlcClient.PlcMemory)var1;
                var1 = cmbReadType.SelectedIndex + 1;
                FinsTcp.PlcClient.DataType typ = (FinsTcp.PlcClient.DataType)var1;

                re = PLC.CmdRead(PlcHand, mry, typ, Convert.ToUInt16(txtReadAdd.Text), Convert.ToUInt16(txtReadCnt.Text), ref RD);
                txtReRead.Text = re.ToString();
                lstRead.Items.Clear();
                if (RD != null)
                {
                    lock (this)
                    {
                        string RDTemp = (string)RD[0];
                        for (short i = 15; i >= 0; i--)
                        {

                            //lstRead.Items.Add(RDTemp.Substring(i, 1));

                            ReadDataByte[15 - i] = RDTemp.Substring(i, 1);
                        }
                    }

                }

            }
            catch
            {

            }



        }
        private void WritePLC(string[] txtByte)
        {
            try
            {
                short i = 0;
                short re = 0;
                string[] temp = null;
                object[] WD = null;
                if (!EntLink)
                {
                    MessageBox.Show("还未与PLC建立联接！");
                    return;
                }
                WD = new object[Convert.ToUInt16(txtWriteCnt.Text)];

                string[] txtString = new string[1];
                for (i = 15; i >= 0; i--)
                {
                    txtString[0] += txtByte[i];
                }

                temp = txtString;
                for (i = 0; i < WD.Length; i++)
                {
                    if (i >= temp.Length)
                    {
                        WD[i] = 0;
                    }
                    else
                    {
                        WD[i] = temp[i].Trim();
                    }
                }
                int var1 = cmbWriteMry.SelectedIndex + 1;
                FinsTcp.PlcClient.PlcMemory mry = (FinsTcp.PlcClient.PlcMemory)var1;
                var1 = cmbWriteType.SelectedIndex + 1;
                FinsTcp.PlcClient.DataType typ = (FinsTcp.PlcClient.DataType)var1;
                re = PLC.CmdWrite(PlcHand, mry, typ, Convert.ToUInt16(txtWriteAdd.Text), Convert.ToUInt16(txtWriteCnt.Text), ref WD);
                txtWrite.Text = "";
                for (i = 0; i < WD.Length; i++)
                {
                    txtWrite.Text += temp[i] + "\r\n";
                }

            }
            catch
            {


            }

        }



        Thread _PLCreadThread;
        bool HipotTestFinished;
        public bool HipotExcuteState;
        private void PLCreadThread()
        {
            WriteDataByte[0] = "0";
            WriteDataByte[1] = "0";
            WriteDataByte[2] = "0";
            WriteDataByte[3] = "0";
            WritePLC(WriteDataByte);
            HipotTestFinished = true;
            HipotExcuteState = true;

            while (HipotExcuteState)
            {
                ReadData();
                Thread.Sleep(10);

                if (ReadDataByte[0] == "1" && ReadDataByte[2] == "1")
                {
                    WriteDataByte[0] = "0";
                    WriteDataByte[1] = "0";
                    WriteDataByte[2] = "0";
                    WriteDataByte[3] = "0";
                    WritePLC(WriteDataByte);
                }



                if (!HipotTestFinished)
                {
                    continue;
                }
                if (Mod_sys.Instance.gfrmAutoRun.radioHipotON.Checked)
                {
                    if (ReadDataByte[0] == "1" && ReadDataByte[1] == "1")
                    {

                        HipotTestFinished = false;
                        WriteDataByte[0] = "1";
                        WriteDataByte[1] = "0";
                        WriteDataByte[2] = "0";
                        WriteDataByte[3] = "0";
                        WritePLC(WriteDataByte);

                        BeginInvoke(new MethodInvoker(delegate
                        {
                            HipotTestExcute();
                        }));

                    }

                }
                else
                {
                    if (ReadDataByte[0] == "1" && ReadDataByte[1] == "1")
                    {

                        Thread.Sleep(10);
                        WriteDataByte[0] = "1";
                        WriteDataByte[1] = "0";
                        WriteDataByte[2] = "0";
                        WriteDataByte[3] = "0";
                        WritePLC(WriteDataByte);
                        Thread.Sleep(10);
                        WriteDataByte[0] = "0";
                        WriteDataByte[1] = "1";
                        WriteDataByte[2] = "1";
                        WriteDataByte[3] = "0";
                        WritePLC(WriteDataByte);
                    }

                }



            }

            HipotTestStop();
        }

        private void timerOut_Tick(object sender, EventArgs e)
        {

            timerOut.Enabled = false;
            serialPort1.ReceivedBytesThreshold = 7;
            StopCommand();
            Thread.Sleep(50);
            HipotTestExcute();
        }
    }
}
