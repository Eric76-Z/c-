using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        [Obsolete]
        public void UseDNS()
        {
            string hostName = Dns.GetHostName();
            Console.WriteLine("Host Name = " + hostName);
            IPHostEntry local = Dns.GetHostByName(hostName);
            txtIP.Text = local.AddressList[0].ToString();
            foreach (IPAddress ipaddress in local.AddressList)
            {
                Console.WriteLine("IPAddress = " + ipaddress.ToString());
                //string ip = ipaddress.ToString();
            }
        }
        public Form1()
        {
            InitializeComponent();
            ///多线程编程中，如果子线程需要使用主线程中创建的对象和控件，最好在主线程中体现进行检查取消
            ///
            CheckForIllegalCrossThreadCalls = false;

            /// 获取本地IP
            UseDNS();
        }


        /// 创建一个字典，用来存储记录服务器与客户端之间的连接(线程问题)
        ///
        private Dictionary<string, Socket> clientList = new Dictionary<string, Socket>();

        /// 创建连接
        private void btnListen_Click(object sender, EventArgs e)
        {
            Thread myServer = new Thread(MySocket);
            //设置这个线程是后台线程
            myServer.IsBackground = true;
            myServer.Start();
        }

        //①:创建一个用于监听连接的Socket对象；
        //②:用指定的端口号和服务器的Ip建立一个EndPoint对象；
        //③:用Socket对象的Bind()方法绑定EndPoint；
        //④:用Socket对象的Listen()方法开始监听；
        //⑤:接收到客户端的连接，用Socket对象的Accept()方法创建一个新的用于和客户端进行通信的Socket对象；
        //⑥:通信结束后一定记得关闭Socket。

        /// 创建连接的方法
        private void MySocket()
        {
            //1.创建一个用于监听连接的Socket对象；
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //2.用指定的端口号和服务器的Ip建立一个EndPoint对象；
            IPAddress iP = IPAddress.Parse(txtIP.Text);
            IPEndPoint endPoint = new IPEndPoint(iP, Convert.ToInt32(txtPort.Text));

            //3.用Socket对象的Bind()方法绑定EndPoint；
            server.Bind(endPoint);

            //4.用Socket对象的Listen()方法开始监听；

            //同一时刻内允许同时加入链接的最大数量
            server.Listen(20);
            txtLog.AppendText("服务器已经成功开启!");

            //5.接收到客户端的连接，用Socket对象的Accept()方法创建一个新的用于和客户端进行通信的Socket对象；
            while (true)
            {
                //接受接入的一个客户端
                Socket connectClient = server.Accept();
                if (connectClient != null)
                {
                    string infor = connectClient.RemoteEndPoint.ToString();
                    clientList.Add(infor, connectClient);

                    txtLog.AppendText(infor + "加入服务器!");

                    ///服务器将消息发送至客服端
                    string msg = infor + "已成功进入到聊天室!";

                    SendMsg(msg);

                    //每有一个客户端接入时，需要有一个线程进行服务

                    Thread threadClient = new Thread(ReciveMsg);//带参的方法可以把传递的参数放到start中
                    threadClient.IsBackground = true;

                    //创建的新的对应的Socket和客户端Socket进行通信
                    threadClient.Start(connectClient);
                }
            }
        }

        /// 服务器接收到客户端发送的消息
        private void ReciveMsg(object o)
        {
            //Socket connectClient = (Socket)o; //与下面效果一样

            Socket connectClient = o as Socket;//connectClient负责客户端的通信
            IPEndPoint endPoint = null;
            while (true)
            {
                try
                {
                    ///定义服务器接收的字节大小
                    byte[] arrMsg = new byte[1024 * 1024];

                    ///接收到的信息大小(所占字节数)
                    int length = connectClient.Receive(arrMsg);



                    if (length > 0)
                    {
                        string recMsg = Encoding.UTF8.GetString(arrMsg, 0, length);
                        //获取客户端的端口号
                        endPoint = connectClient.RemoteEndPoint as IPEndPoint;
                        //服务器显示客户端的端口号和消息
                        txtLog.AppendText(DateTime.Now + "[" + endPoint.Port.ToString() + "]：" + recMsg);

                        //服务器(connectClient)发送接收到的客户端信息给客户端
                        SendMsg("[" + endPoint.Port.ToString() + "]：" + recMsg);
                    }
                }
                catch (Exception)
                {
                    ///移除添加在字典中的服务器和客户端之间的线程
                    clientList.Remove(endPoint.ToString());

                    connectClient.Dispose();

                }
            }
        }
        /// 服务器发送消息，客户端接收
        private void SendMsg(string str)
        {
            ///遍历出字典中的所有线程
            foreach (var item in clientList)
            {
                byte[] arrMsg = Encoding.UTF8.GetBytes(str);

                ///获取键值(服务器），发送消息
                item.Value.Send(arrMsg);
            }
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void boxLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
