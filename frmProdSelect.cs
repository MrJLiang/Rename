using Microsoft.Win32;
using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;

namespace HAAGONtest
{
    public partial class frmProdSelect : Form
    {
        public frmProdSelect()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            //if (Combox_Prodlist.Text == "")
            //{
            //    MessageBox.Show("请选择产品型号!");
            //    return;
            //}
            Starttimer.Enabled = false;
            //产品型号赋值
            Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text = Combox_Prodlist.Text;

            Combox_Prodlist.Items.Clear();

            Mod_sys.Instance.gfrmInitShow.Show();

            this.Hide();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Combox_Prodlist.Items.Clear();
            Application.Exit();
        }

        private void frmProdSelect_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;

            //    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

            //    //R_run.SetValue("应用名称", Application.ExecutablePath);

            //    R_run.DeleteValue("应用名称");

            //    R_run.Close();

            //    R_local.Close();

            //    //GlobalVariant.Instance.UserConfig.AutoStart = isAuto;
            //}
            //catch (Exception)
            //{
            //    //MessageBoxDlg dlg = new MessageBoxDlg();

            //    //dlg.InitialData("您需要管理员权限修改", "提示", MessageBoxButtons.OK, MessageBoxDlgIcon.Error);

            //    //dlg.ShowDialog();

            //    MessageBox.Show("您需要管理员权限修改", "提示");
            //}
            string FatherPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY";
            string ModelPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model";
            string StatisPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/产能统计/";
            string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/日志/";
            string NGPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/保存图片/";
            if (!Directory.Exists(FatherPath))
            {
                Directory.CreateDirectory(FatherPath);
            }
            if (!Directory.Exists(ModelPath))
            {
                Directory.CreateDirectory(ModelPath);
            }
            if (!Directory.Exists(StatisPath))
            {
                Directory.CreateDirectory(StatisPath);
            }
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            if (!Directory.Exists(NGPath))
            {
                Directory.CreateDirectory(NGPath);
            }

            //DataTable dtModel = new DataTable();
            //string ModelNamePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/ATL-BATTERY/Model/产品型号.csv";
            //if (!System.IO.File.Exists(ModelNamePath))
            //{
            //    //MessageBox.Show("文件不存在!");
            //    //this.Hide();
            //    //Mod_sys.gfrmMain.Show();

            //}
            //else
            //{
            //    dtModel = Mod_sys.Instance.gCSV_sys.OpenCSV(ModelNamePath);

            //    Combox_Prodlist.Text = dtModel.Rows[dtModel.Rows.Count - 1].ItemArray[0].ToString();

            //    for (int i = dtModel.Rows.Count - 1; i >= 0; i--)
            //    {
            //        Combox_Prodlist.Items.Add(dtModel.Rows[i].ItemArray[0]);
            //    }
            //}
            DirectoryInfo dir = new DirectoryInfo(ModelPath);

            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录

            foreach (FileSystemInfo i in fileinfo)
            {
                //if (i.Name.Split('.')[0] == "w")
                //{
                //    File.Delete(ModelPath + "/" + i.Name);
                //}

                if (i.Name.Split('.')[0] == "w")
                {
                    Computer MyComputer = new Computer();
                    MyComputer.FileSystem.RenameFile(ModelPath + "/" + i.Name, "2" + "." + i.Name.Split('.')[1]);
                    //File.Delete(ModelPath + "/" + i.Name);
                }
            }

            string r = null;
            dir = new DirectoryInfo(ModelPath);
            fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i.Name.Split('.')[0] != r)
                {
                    Combox_Prodlist.Items.Add(i.Name.Split('.')[0]);
                }
                r = i.Name.Split('.')[0];
            }



            //Directory.CreateDirectory(ModelPath + "/1");
            ////Directory.CreateDirectory(ModelPath + "/1"+"/222");
            //Directory.CreateDirectory(ModelPath + "/2");
            //Directory.CreateDirectory(ModelPath + "/3");
            //Directory.CreateDirectory(ModelPath + "/4");

            //FindFile(ModelPath);


            Starttimer.Enabled = true;
        }

        private void Starttimer_Tick(object sender, EventArgs e)
        {
            Starttimer.Enabled = false;
            Mod_sys.Instance.gfrmAutoRun.txtbox_productName.Text = Combox_Prodlist.Text;

            Combox_Prodlist.Items.Clear();

            Mod_sys.Instance.gfrmInitShow.Show();

            this.Hide();
        }

        public void FindFile(string sSourcePath)
        {

            DirectoryInfo theFolder = new DirectoryInfo(sSourcePath);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                Combox_Prodlist.Items.Add(NextFolder.Name);
                //FileInfo[] fileInfo = NextFolder.GetFiles();
                //foreach (FileInfo NextFile in fileInfo)  //遍历文件
                //    Combox_Prodlist.Items.Add(NextFile.Name);
            }



        }

        public static void DeleteDirectory(string directoryPath, string fileName)
        {

            //删除文件
            //for (int i = 0; i < Directory.GetFiles(directoryPath).ToList().Count; i++)
            //{
            //    if (Directory.GetFiles(directoryPath)[i] == fileName)
            //    {
            //        File.Delete(fileName);
            //    }
            //}

            //删除文件夹
            //for (int i = 0; i < Directory.GetDirectories(directoryPath).ToList().Count; i++)
            //{
            //    if (Directory.GetDirectories(directoryPath)[i] == fileName)
            //    {
            //        Directory.Delete(fileName, true);
            //    }
            //}
        }
    }
}