using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HAAGONtest
{
    internal class TCPCommunition
    {
        private IPAddress ip;
        private IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
        public Thread _TCReceive;
        private Socket aimSocket;
        public bool TCReceiveStat = true;

        public delegate void TCPEvent(object sender, string e);

        public event TCPEvent _TCPEvent;

        private void TCPReceive(string e)
        {
            //定义一个局部变量，将事件对象赋值给该变量，防止多线程情况下取消事件
            TCPEvent mEvent = _TCPEvent;
            if (mEvent != null)
            {
                mEvent(this, e);
            }
        }

        public TCPCommunition()
        {
            GetAddressIP();
        }
        public void GetAddressIP()
        {
            string AddressIP = "";
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                    ip = _IPAddress;//设定全局的IP
                }
            }
        }

        public void TCPClientConnect(string TCPIPAddress, string TCPIPPort)
        {
            ip = IPAddress.Parse(TCPIPAddress);//目标IP
            point = new IPEndPoint(ip, Convert.ToInt32(TCPIPPort));//目标端口

            aimSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            aimSocket.Connect(point);//连接服务器

            _TCReceive = new Thread(new ThreadStart(TCReceive));
            _TCReceive.IsBackground = true;
            _TCReceive.Start();
        }



        //TCP Client接收线程
        public void TCReceive()
        {
            byte[] buffer = new byte[1024];
            TCReceiveStat = true;
            while (TCReceiveStat)
            {
                try
                {
                    int r = aimSocket.Receive(buffer);
                    if (r == 0)
                    {
                        MessageBox.Show("连接断开");
                        break;
                    }
                    string strRec = Encoding.Default.GetString(buffer, 0, r);
                    //txtRec.AppendText(aimSocket.RemoteEndPoint.ToString() + ":" + strRec + "\r\n");
                    TCPReceive(strRec);
                    //txtRec.AppendText(strRec + "\r\n");
                }
                catch
                { }
            }
        }

 


        public void TCPSend(string[] SendData)
        {
            string _SendData = "";
            try
            {
                for (int i = 0; i < SendData.Length; i++)
                {
                    //if (i != SendData.Length - 1)
                        _SendData += SendData[i] + ",";
                    //else
                    //    _SendData += SendData[i];
                }
                byte[] buffer = Encoding.Default.GetBytes(_SendData);
                aimSocket.SendTo(buffer, point);
            }
            catch
            { }
        }
    }
}