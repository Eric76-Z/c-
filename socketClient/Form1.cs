using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace socketClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketSend;
        private void button1_Click(object sender, EventArgs e)
        {
            //创建负责通信的socket
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(txtServer.Text);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            //获得要连接的远程服务器应用程序的IP地址与端口号
            socketSend.Connect(point);
            ShowMsg("连接成功");
        }
        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 客服端给服务端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string str = txtMsg.Text.Trim();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socketSend.Send(buffer);
        }

        private void txtMsg_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
