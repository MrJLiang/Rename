using System;
using System.Threading;
using System.Windows.Forms;

namespace HAAGONtest
{
    public partial class frmInitShow : Form
    {
        private Thread _InitThread;

        public frmInitShow()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void frmInitShow_Load(object sender, EventArgs e)
        {
            _InitThread = new Thread(new ThreadStart(InitThread));
            _InitThread.Start();
        }

        private Enum_sys.InitError InitName;

        private void InitThread()
        {
            try
            {
                InitName = Enum_sys.InitError.PLC;
                label1.Text = "正在连接PLC!";
                Mod_sys.Instance.gHipotTest.PLCConnectInit();
                Thread.Sleep(300);
            }
            catch
            {

                MessageBox.Show(InitName + "连接PLC失败");
                Application.Exit();
                return;

            }
            try
            {
                InitName = Enum_sys.InitError.Hipot;
                label1.Text = "正在连接Hipot设备!";
                Mod_sys.Instance.gHipotTest.OpenPort();
                Thread.Sleep(300);
            }
            catch
            {

                MessageBox.Show(InitName + "连接Hipot设备失败");
                Application.Exit();
                return;

            }
            try
            {
                InitName = Enum_sys.InitError.相机;
                label1.Text = "CCD相机正在初始化!";
                Mod_sys.Instance.gCCD_sys.CCDInit();
                Thread.Sleep(300);
            }
            catch
            {

                MessageBox.Show(InitName + "初始化失败");
                Application.Exit();
                return;

            }
            try
            {
                InitName = Enum_sys.InitError.机器手;
                label1.Text = "机械手正在初始化!";
                Mod_sys.Instance.gTCPRobot.TCPClientConnect("192.168.250.50", "9600");
                Thread.Sleep(300);
            }
            catch
            {

                MessageBox.Show(InitName + "初始化失败");
                Application.Exit();
                return;

            }

            label1.Text = "设备初始化成功!";
            Thread.Sleep(1000);
            BeginInvoke(new MethodInvoker(delegate
            {
                Mod_sys.Instance.gfrmMain.Show();
                this.Hide();
            }));
        }
    }
}