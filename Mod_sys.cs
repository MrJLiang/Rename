using System;
using Hipot;


namespace HAAGONtest
{
    internal class Mod_sys
    {
        #region 单例.....
        private static object syncObj = new object();
        private static Mod_sys _Instance = null;
        public static Mod_sys Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (syncObj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Mod_sys();
                        }
                    }
                }

                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }


        private Mod_sys()
        {

        }
        #endregion
        /// <summary>
        /// 窗口定义
        /// </summary>
        public frmProdSelect gfrmProdSelect = new frmProdSelect();
        public frmMain gfrmMain = new frmMain();
        public frmInitShow gfrmInitShow = new frmInitShow();
        public ChangePassWord gChangePassWord = new ChangePassWord();
        public frmAutoRun gfrmAutoRun = new frmAutoRun();
        public frmProdChange gfrmProdChange = new frmProdChange();
        public Login gLogin = new Login();
        public frmNameModel gfrmNameModel = new frmNameModel();

        /// <summary>
        /// Hipot类
        /// </summary>
        public HipotTest gHipotTest = new HipotTest();

        /// <summary>
        /// CCD类
        /// </summary>
        public CCD_sys gCCD_sys = new CCD_sys();

        /// <summary>
        /// CSV类
        /// </summary>
        public CSV_sys gCSV_sys = new CSV_sys();

        /// <summary>
        /// 机台运行类
        /// </summary>
        public Run_sys gRun_sys = new Run_sys();

        /// <summary>
        /// 机械手通信
        /// </summary>
        public TCPCommunition gTCPRobot = new TCPCommunition();


    }
}