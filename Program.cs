using System;
using System.Windows.Forms;

namespace HAAGONtest
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(Mod_sys.Instance.gfrmProdSelect);
        }
    }
}