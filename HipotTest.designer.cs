namespace Hipot
{
    partial class HipotTest
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OffPort = new System.Windows.Forms.Button();
            this.com_Baud = new System.Windows.Forms.ComboBox();
            this.com_Port = new System.Windows.Forms.ComboBox();
            this.OnPort = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.butGetOSData = new System.Windows.Forms.Button();
            this.butGetIRData = new System.Windows.Forms.Button();
            this.butConnect = new System.Windows.Forms.Button();
            this.butStop = new System.Windows.Forms.Button();
            this.butStart = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOSpF = new System.Windows.Forms.TextBox();
            this.txtOSVolt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtOSState = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIRo = new System.Windows.Forms.Label();
            this.txtIRohm = new System.Windows.Forms.TextBox();
            this.txtIRVolt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIRState = new System.Windows.Forms.TextBox();
            this.txtbox_showLog = new System.Windows.Forms.RichTextBox();
            this.timerIR = new System.Windows.Forms.Timer(this.components);
            this.timerOS = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cmbWriteType = new System.Windows.Forms.ComboBox();
            this.Label23 = new System.Windows.Forms.Label();
            this.txtWrite = new System.Windows.Forms.TextBox();
            this.txtReWrite = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.butWrite = new System.Windows.Forms.Button();
            this.cmbWriteMry = new System.Windows.Forms.ComboBox();
            this.txtWriteAdd = new System.Windows.Forms.TextBox();
            this.txtWriteCnt = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtReRead = new System.Windows.Forms.TextBox();
            this.cmbReadType = new System.Windows.Forms.ComboBox();
            this.Label24 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.butRead = new System.Windows.Forms.Button();
            this.lstRead = new System.Windows.Forms.ListBox();
            this.cmbReadMry = new System.Windows.Forms.ComboBox();
            this.txtReadAdd = new System.Windows.Forms.TextBox();
            this.txtReadCnt = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtRemotePort = new System.Windows.Forms.TextBox();
            this.txtReClose = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtReLink = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLocalIP = new System.Windows.Forms.TextBox();
            this.butClose = new System.Windows.Forms.Button();
            this.butLink = new System.Windows.Forms.Button();
            this.timerOut = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.OffPort);
            this.groupBox1.Controls.Add(this.com_Baud);
            this.groupBox1.Controls.Add(this.com_Port);
            this.groupBox1.Controls.Add(this.OnPort);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(492, 450);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 161);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(14, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 21);
            this.label4.TabIndex = 4;
            this.label4.Text = "波特率";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(30, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "端口";
            // 
            // OffPort
            // 
            this.OffPort.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OffPort.Location = new System.Drawing.Point(102, 112);
            this.OffPort.Name = "OffPort";
            this.OffPort.Size = new System.Drawing.Size(76, 30);
            this.OffPort.TabIndex = 2;
            this.OffPort.Text = "关闭端口";
            this.OffPort.UseVisualStyleBackColor = true;
            this.OffPort.Click += new System.EventHandler(this.OffPort_Click);
            // 
            // com_Baud
            // 
            this.com_Baud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Baud.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.com_Baud.FormattingEnabled = true;
            this.com_Baud.Items.AddRange(new object[] {
            "19200"});
            this.com_Baud.Location = new System.Drawing.Point(78, 67);
            this.com_Baud.Name = "com_Baud";
            this.com_Baud.Size = new System.Drawing.Size(99, 28);
            this.com_Baud.TabIndex = 1;
            // 
            // com_Port
            // 
            this.com_Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Port.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.com_Port.FormattingEnabled = true;
            this.com_Port.Items.AddRange(new object[] {
            "COM3"});
            this.com_Port.Location = new System.Drawing.Point(78, 33);
            this.com_Port.Name = "com_Port";
            this.com_Port.Size = new System.Drawing.Size(100, 28);
            this.com_Port.TabIndex = 0;
            this.com_Port.Click += new System.EventHandler(this.com_Port_Click);
            // 
            // OnPort
            // 
            this.OnPort.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OnPort.Location = new System.Drawing.Point(13, 112);
            this.OnPort.Name = "OnPort";
            this.OnPort.Size = new System.Drawing.Size(75, 30);
            this.OnPort.TabIndex = 1;
            this.OnPort.Text = "打开端口";
            this.OnPort.UseVisualStyleBackColor = true;
            this.OnPort.Click += new System.EventHandler(this.OnPort_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.ReceivedBytesThreshold = 7;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.port_DataReceived);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.butGetOSData);
            this.groupBox2.Controls.Add(this.butGetIRData);
            this.groupBox2.Controls.Add(this.butConnect);
            this.groupBox2.Controls.Add(this.butStop);
            this.groupBox2.Controls.Add(this.butStart);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(496, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(196, 429);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发送指令";
            // 
            // butGetOSData
            // 
            this.butGetOSData.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butGetOSData.Location = new System.Drawing.Point(43, 302);
            this.butGetOSData.Name = "butGetOSData";
            this.butGetOSData.Size = new System.Drawing.Size(116, 34);
            this.butGetOSData.TabIndex = 4;
            this.butGetOSData.Text = "采集OS";
            this.butGetOSData.UseVisualStyleBackColor = true;
            this.butGetOSData.Click += new System.EventHandler(this.butGetOSData_Click);
            // 
            // butGetIRData
            // 
            this.butGetIRData.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butGetIRData.Location = new System.Drawing.Point(43, 243);
            this.butGetIRData.Name = "butGetIRData";
            this.butGetIRData.Size = new System.Drawing.Size(116, 34);
            this.butGetIRData.TabIndex = 3;
            this.butGetIRData.Text = "采集IR";
            this.butGetIRData.UseVisualStyleBackColor = true;
            this.butGetIRData.Click += new System.EventHandler(this.butGetData_Click);
            // 
            // butConnect
            // 
            this.butConnect.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butConnect.Location = new System.Drawing.Point(46, 66);
            this.butConnect.Name = "butConnect";
            this.butConnect.Size = new System.Drawing.Size(116, 34);
            this.butConnect.TabIndex = 2;
            this.butConnect.Text = "连接测试";
            this.butConnect.UseVisualStyleBackColor = true;
            this.butConnect.Click += new System.EventHandler(this.butConnect_Click);
            // 
            // butStop
            // 
            this.butStop.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butStop.Location = new System.Drawing.Point(43, 184);
            this.butStop.Name = "butStop";
            this.butStop.Size = new System.Drawing.Size(116, 34);
            this.butStop.TabIndex = 1;
            this.butStop.Text = "停止测试";
            this.butStop.UseVisualStyleBackColor = true;
            this.butStop.Click += new System.EventHandler(this.butStop_Click);
            // 
            // butStart
            // 
            this.butStart.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butStart.Location = new System.Drawing.Point(46, 125);
            this.butStart.Name = "butStart";
            this.butStart.Size = new System.Drawing.Size(116, 34);
            this.butStart.TabIndex = 0;
            this.butStart.Text = "开始测试";
            this.butStart.UseVisualStyleBackColor = true;
            this.butStart.Click += new System.EventHandler(this.butStart_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtOSpF);
            this.groupBox3.Controls.Add(this.txtOSVolt);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtOSState);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtIRo);
            this.groupBox3.Controls.Add(this.txtIRohm);
            this.groupBox3.Controls.Add(this.txtIRVolt);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtIRState);
            this.groupBox3.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(12, 450);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(474, 155);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "测试结果";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(428, 48);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(43, 27);
            this.label22.TabIndex = 12;
            this.label22.Text = "GΩ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 27);
            this.label2.TabIndex = 11;
            this.label2.Text = "Volt:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(263, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 27);
            this.label6.TabIndex = 10;
            this.label6.Text = "pF:";
            // 
            // txtOSpF
            // 
            this.txtOSpF.Location = new System.Drawing.Point(310, 97);
            this.txtOSpF.Name = "txtOSpF";
            this.txtOSpF.Size = new System.Drawing.Size(96, 34);
            this.txtOSpF.TabIndex = 9;
            // 
            // txtOSVolt
            // 
            this.txtOSVolt.Location = new System.Drawing.Point(191, 97);
            this.txtOSVolt.Name = "txtOSVolt";
            this.txtOSVolt.Size = new System.Drawing.Size(61, 34);
            this.txtOSVolt.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 27);
            this.label7.TabIndex = 7;
            this.label7.Text = "OS:";
            // 
            // txtOSState
            // 
            this.txtOSState.Location = new System.Drawing.Point(57, 97);
            this.txtOSState.Name = "txtOSState";
            this.txtOSState.Size = new System.Drawing.Size(64, 34);
            this.txtOSState.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 27);
            this.label5.TabIndex = 5;
            this.label5.Text = "Volt:";
            // 
            // txtIRo
            // 
            this.txtIRo.AutoSize = true;
            this.txtIRo.Location = new System.Drawing.Point(259, 48);
            this.txtIRo.Name = "txtIRo";
            this.txtIRo.Size = new System.Drawing.Size(61, 27);
            this.txtIRo.TabIndex = 4;
            this.txtIRo.Text = "ohm:";
            // 
            // txtIRohm
            // 
            this.txtIRohm.Location = new System.Drawing.Point(326, 45);
            this.txtIRohm.Name = "txtIRohm";
            this.txtIRohm.Size = new System.Drawing.Size(96, 34);
            this.txtIRohm.TabIndex = 3;
            // 
            // txtIRVolt
            // 
            this.txtIRVolt.Location = new System.Drawing.Point(191, 45);
            this.txtIRVolt.Name = "txtIRVolt";
            this.txtIRVolt.Size = new System.Drawing.Size(61, 34);
            this.txtIRVolt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "IR:";
            // 
            // txtIRState
            // 
            this.txtIRState.Location = new System.Drawing.Point(57, 45);
            this.txtIRState.Name = "txtIRState";
            this.txtIRState.Size = new System.Drawing.Size(64, 34);
            this.txtIRState.TabIndex = 0;
            // 
            // txtbox_showLog
            // 
            this.txtbox_showLog.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtbox_showLog.Location = new System.Drawing.Point(12, 33);
            this.txtbox_showLog.Name = "txtbox_showLog";
            this.txtbox_showLog.ReadOnly = true;
            this.txtbox_showLog.Size = new System.Drawing.Size(474, 419);
            this.txtbox_showLog.TabIndex = 8;
            this.txtbox_showLog.Text = "";
            // 
            // timerIR
            // 
            this.timerIR.Interval = 1700;
            this.timerIR.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerOS
            // 
            this.timerOS.Interval = 20;
            this.timerOS.Tick += new System.EventHandler(this.timerOS_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Controls.Add(this.txtbox_showLog);
            this.groupBox4.Controls.Add(this.groupBox1);
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(603, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(700, 611);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "HipotTest";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Controls.Add(this.txtRemotePort);
            this.groupBox5.Controls.Add(this.txtReClose);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.txtReLink);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.txtRemoteIP);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.txtLocalPort);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtLocalIP);
            this.groupBox5.Controls.Add(this.butClose);
            this.groupBox5.Controls.Add(this.butLink);
            this.groupBox5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox5.Location = new System.Drawing.Point(12, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(585, 611);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PLC通信";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cmbWriteType);
            this.groupBox7.Controls.Add(this.Label23);
            this.groupBox7.Controls.Add(this.txtWrite);
            this.groupBox7.Controls.Add(this.txtReWrite);
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.butWrite);
            this.groupBox7.Controls.Add(this.cmbWriteMry);
            this.groupBox7.Controls.Add(this.txtWriteAdd);
            this.groupBox7.Controls.Add(this.txtWriteCnt);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.label21);
            this.groupBox7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox7.Location = new System.Drawing.Point(24, 365);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(535, 243);
            this.groupBox7.TabIndex = 77;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Write";
            // 
            // cmbWriteType
            // 
            this.cmbWriteType.FormattingEnabled = true;
            this.cmbWriteType.Location = new System.Drawing.Point(409, 58);
            this.cmbWriteType.Name = "cmbWriteType";
            this.cmbWriteType.Size = new System.Drawing.Size(88, 22);
            this.cmbWriteType.TabIndex = 57;
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.Location = new System.Drawing.Point(349, 61);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(35, 14);
            this.Label23.TabIndex = 55;
            this.Label23.Text = "Type";
            // 
            // txtWrite
            // 
            this.txtWrite.Location = new System.Drawing.Point(11, 20);
            this.txtWrite.Multiline = true;
            this.txtWrite.Name = "txtWrite";
            this.txtWrite.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWrite.Size = new System.Drawing.Size(286, 210);
            this.txtWrite.TabIndex = 54;
            // 
            // txtReWrite
            // 
            this.txtReWrite.Location = new System.Drawing.Point(409, 170);
            this.txtReWrite.Name = "txtReWrite";
            this.txtReWrite.ReadOnly = true;
            this.txtReWrite.Size = new System.Drawing.Size(88, 23);
            this.txtReWrite.TabIndex = 52;
            this.txtReWrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(337, 173);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 14);
            this.label18.TabIndex = 53;
            this.label18.Text = "Return";
            // 
            // butWrite
            // 
            this.butWrite.Location = new System.Drawing.Point(409, 204);
            this.butWrite.Name = "butWrite";
            this.butWrite.Size = new System.Drawing.Size(88, 26);
            this.butWrite.TabIndex = 51;
            this.butWrite.Text = "Cmd Write";
            this.butWrite.UseVisualStyleBackColor = true;
            this.butWrite.Click += new System.EventHandler(this.butWrite_Click);
            // 
            // cmbWriteMry
            // 
            this.cmbWriteMry.FormattingEnabled = true;
            this.cmbWriteMry.Location = new System.Drawing.Point(409, 24);
            this.cmbWriteMry.Name = "cmbWriteMry";
            this.cmbWriteMry.Size = new System.Drawing.Size(88, 22);
            this.cmbWriteMry.TabIndex = 40;
            // 
            // txtWriteAdd
            // 
            this.txtWriteAdd.Location = new System.Drawing.Point(409, 94);
            this.txtWriteAdd.Name = "txtWriteAdd";
            this.txtWriteAdd.Size = new System.Drawing.Size(88, 23);
            this.txtWriteAdd.TabIndex = 36;
            this.txtWriteAdd.Text = "21";
            this.txtWriteAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtWriteCnt
            // 
            this.txtWriteCnt.Location = new System.Drawing.Point(409, 132);
            this.txtWriteCnt.Name = "txtWriteCnt";
            this.txtWriteCnt.Size = new System.Drawing.Size(88, 23);
            this.txtWriteCnt.TabIndex = 38;
            this.txtWriteCnt.Text = "1";
            this.txtWriteCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(343, 135);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(42, 14);
            this.label19.TabIndex = 49;
            this.label19.Text = "Count";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(337, 26);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(49, 14);
            this.label20.TabIndex = 39;
            this.label20.Text = "Memory";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(330, 97);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 14);
            this.label21.TabIndex = 43;
            this.label21.Text = "Address";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtReRead);
            this.groupBox6.Controls.Add(this.cmbReadType);
            this.groupBox6.Controls.Add(this.Label24);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.butRead);
            this.groupBox6.Controls.Add(this.lstRead);
            this.groupBox6.Controls.Add(this.cmbReadMry);
            this.groupBox6.Controls.Add(this.txtReadAdd);
            this.groupBox6.Controls.Add(this.txtReadCnt);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox6.Location = new System.Drawing.Point(24, 116);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(535, 243);
            this.groupBox6.TabIndex = 76;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Read";
            // 
            // txtReRead
            // 
            this.txtReRead.Location = new System.Drawing.Point(409, 168);
            this.txtReRead.Name = "txtReRead";
            this.txtReRead.ReadOnly = true;
            this.txtReRead.Size = new System.Drawing.Size(89, 23);
            this.txtReRead.TabIndex = 52;
            this.txtReRead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbReadType
            // 
            this.cmbReadType.FormattingEnabled = true;
            this.cmbReadType.Location = new System.Drawing.Point(409, 56);
            this.cmbReadType.Name = "cmbReadType";
            this.cmbReadType.Size = new System.Drawing.Size(89, 22);
            this.cmbReadType.TabIndex = 58;
            // 
            // Label24
            // 
            this.Label24.AutoSize = true;
            this.Label24.Location = new System.Drawing.Point(357, 59);
            this.Label24.Name = "Label24";
            this.Label24.Size = new System.Drawing.Size(35, 14);
            this.Label24.TabIndex = 56;
            this.Label24.Text = "Type";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(343, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 14);
            this.label14.TabIndex = 53;
            this.label14.Text = "Return";
            // 
            // butRead
            // 
            this.butRead.Location = new System.Drawing.Point(409, 203);
            this.butRead.Name = "butRead";
            this.butRead.Size = new System.Drawing.Size(88, 26);
            this.butRead.TabIndex = 51;
            this.butRead.Text = "Cmd Read";
            this.butRead.UseVisualStyleBackColor = true;
            this.butRead.Click += new System.EventHandler(this.butRead_Click);
            // 
            // lstRead
            // 
            this.lstRead.FormattingEnabled = true;
            this.lstRead.ItemHeight = 14;
            this.lstRead.Location = new System.Drawing.Point(11, 20);
            this.lstRead.Name = "lstRead";
            this.lstRead.ScrollAlwaysVisible = true;
            this.lstRead.Size = new System.Drawing.Size(286, 200);
            this.lstRead.TabIndex = 50;
            // 
            // cmbReadMry
            // 
            this.cmbReadMry.FormattingEnabled = true;
            this.cmbReadMry.Location = new System.Drawing.Point(409, 22);
            this.cmbReadMry.Name = "cmbReadMry";
            this.cmbReadMry.Size = new System.Drawing.Size(89, 22);
            this.cmbReadMry.TabIndex = 40;
            // 
            // txtReadAdd
            // 
            this.txtReadAdd.Location = new System.Drawing.Point(409, 92);
            this.txtReadAdd.Name = "txtReadAdd";
            this.txtReadAdd.Size = new System.Drawing.Size(89, 23);
            this.txtReadAdd.TabIndex = 36;
            this.txtReadAdd.Text = "20";
            this.txtReadAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtReadCnt
            // 
            this.txtReadCnt.Location = new System.Drawing.Point(409, 131);
            this.txtReadCnt.Name = "txtReadCnt";
            this.txtReadCnt.Size = new System.Drawing.Size(89, 23);
            this.txtReadCnt.TabIndex = 38;
            this.txtReadCnt.Text = "1";
            this.txtReadCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(350, 134);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 14);
            this.label15.TabIndex = 49;
            this.label15.Text = "Count";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(343, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 14);
            this.label16.TabIndex = 39;
            this.label16.Text = "Memory";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(337, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 14);
            this.label17.TabIndex = 43;
            this.label17.Text = "Address";
            // 
            // txtRemotePort
            // 
            this.txtRemotePort.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemotePort.Location = new System.Drawing.Point(275, 82);
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(44, 23);
            this.txtRemotePort.TabIndex = 70;
            this.txtRemotePort.Text = "9600";
            this.txtRemotePort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtReClose
            // 
            this.txtReClose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReClose.Location = new System.Drawing.Point(408, 82);
            this.txtReClose.Name = "txtReClose";
            this.txtReClose.ReadOnly = true;
            this.txtReClose.Size = new System.Drawing.Size(44, 23);
            this.txtReClose.TabIndex = 74;
            this.txtReClose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(332, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 14);
            this.label8.TabIndex = 75;
            this.label8.Text = "Re.DeLink";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(338, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 14);
            this.label9.TabIndex = 73;
            this.label9.Text = "Re.Link";
            // 
            // txtReLink
            // 
            this.txtReLink.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReLink.Location = new System.Drawing.Point(408, 46);
            this.txtReLink.Name = "txtReLink";
            this.txtReLink.ReadOnly = true;
            this.txtReLink.Size = new System.Drawing.Size(44, 23);
            this.txtReLink.TabIndex = 72;
            this.txtReLink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(210, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 71;
            this.label10.Text = "PLC_Port";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(21, 86);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 14);
            this.label11.TabIndex = 69;
            this.label11.Text = "PLC_IP";
            // 
            // txtRemoteIP
            // 
            this.txtRemoteIP.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemoteIP.Location = new System.Drawing.Point(76, 82);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(119, 23);
            this.txtRemoteIP.TabIndex = 68;
            this.txtRemoteIP.Text = "192.168.250.1";
            this.txtRemoteIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(210, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 14);
            this.label12.TabIndex = 67;
            this.label12.Text = "PC_Port";
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLocalPort.Location = new System.Drawing.Point(275, 46);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(44, 23);
            this.txtLocalPort.TabIndex = 66;
            this.txtLocalPort.Text = "0";
            this.txtLocalPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(21, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 14);
            this.label13.TabIndex = 65;
            this.label13.Text = "PC_IP";
            // 
            // txtLocalIP
            // 
            this.txtLocalIP.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLocalIP.Location = new System.Drawing.Point(76, 46);
            this.txtLocalIP.Name = "txtLocalIP";
            this.txtLocalIP.Size = new System.Drawing.Size(119, 23);
            this.txtLocalIP.TabIndex = 64;
            this.txtLocalIP.Text = "192.168.250.103";
            this.txtLocalIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // butClose
            // 
            this.butClose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butClose.Location = new System.Drawing.Point(469, 79);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(88, 26);
            this.butClose.TabIndex = 63;
            this.butClose.Text = "DeLink";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // butLink
            // 
            this.butLink.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butLink.Location = new System.Drawing.Point(469, 44);
            this.butLink.Name = "butLink";
            this.butLink.Size = new System.Drawing.Size(88, 26);
            this.butLink.TabIndex = 62;
            this.butLink.Text = "EntLink";
            this.butLink.UseVisualStyleBackColor = true;
            this.butLink.Click += new System.EventHandler(this.butLink_Click);
            // 
            // timerOut
            // 
            this.timerOut.Interval = 2500;
            this.timerOut.Tick += new System.EventHandler(this.timerOut_Tick);
            // 
            // HipotTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1342, 662);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HipotTest";
            this.Text = "HipotTest";
            this.Load += new System.EventHandler(this.HipotTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OffPort;
        private System.Windows.Forms.ComboBox com_Baud;
        private System.Windows.Forms.ComboBox com_Port;
        private System.Windows.Forms.Button OnPort;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button butStop;
        private System.Windows.Forms.Button butStart;
        private System.Windows.Forms.Button butConnect;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button butGetIRData;
        internal System.Windows.Forms.RichTextBox txtbox_showLog;
        private System.Windows.Forms.Button butGetOSData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label txtIRo;
        private System.Windows.Forms.TextBox txtIRohm;
        private System.Windows.Forms.TextBox txtIRVolt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIRState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOSpF;
        private System.Windows.Forms.TextBox txtOSVolt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtOSState;
        private System.Windows.Forms.Timer timerIR;
        private System.Windows.Forms.Timer timerOS;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.TextBox txtRemotePort;
        internal System.Windows.Forms.TextBox txtReClose;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox txtReLink;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox txtRemoteIP;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.TextBox txtLocalPort;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.TextBox txtLocalIP;
        internal System.Windows.Forms.Button butClose;
        internal System.Windows.Forms.Button butLink;
        internal System.Windows.Forms.GroupBox groupBox6;
        internal System.Windows.Forms.TextBox txtReRead;
        internal System.Windows.Forms.ComboBox cmbReadType;
        internal System.Windows.Forms.Label Label24;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Button butRead;
        internal System.Windows.Forms.ListBox lstRead;
        internal System.Windows.Forms.ComboBox cmbReadMry;
        internal System.Windows.Forms.TextBox txtReadAdd;
        internal System.Windows.Forms.TextBox txtReadCnt;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Label label17;
        internal System.Windows.Forms.GroupBox groupBox7;
        internal System.Windows.Forms.ComboBox cmbWriteType;
        internal System.Windows.Forms.Label Label23;
        internal System.Windows.Forms.TextBox txtWrite;
        internal System.Windows.Forms.TextBox txtReWrite;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Button butWrite;
        internal System.Windows.Forms.ComboBox cmbWriteMry;
        internal System.Windows.Forms.TextBox txtWriteAdd;
        internal System.Windows.Forms.TextBox txtWriteCnt;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label20;
        internal System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Timer timerOut;
    }
}

