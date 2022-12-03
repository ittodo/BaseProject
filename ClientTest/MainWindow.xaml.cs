using Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Socket.Run.Init();
        }

        Socket.Connection.Connect connect;

        private void ConnectSocketServer(object sender, RoutedEventArgs e)
        {
            this.Cosole.Text += "ConnectSocketServer\n";


            IPHostEntry ipHostInfo = Dns.GetHostEntry("ec2-3-35-188-69.ap-northeast-2.compute.amazonaws.com");

            IPAddress ipaddress = null;
            foreach (var item in ipHostInfo.AddressList)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipaddress = item;
                }
            }
            IPEndPoint localendpoint = new IPEndPoint(ipaddress, 11001);

            connect = new Socket.Connection.Connect(localendpoint);
            connect.Start();
        }

        private void SendServer(object sender, RoutedEventArgs e)
        {
            if(connect.connect != null)
            {
                connect.connect.Send(new Packet.Version("test"));
            }
        }

        private void ConnectSocketLocalServer(object sender, RoutedEventArgs e)
        {
            this.Cosole.Text += "ConnectSocketServer\n";

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress ipaddress = null;
            foreach(var item in ipHostInfo.AddressList)
            {
                if(item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipaddress = item;
                }
            }
            IPEndPoint localendpoint = new IPEndPoint(ipaddress, 11001);

            connect = new Socket.Connection.Connect(localendpoint);
            connect.Start();
        }

        private void DisConnect(object sender, RoutedEventArgs e)
        {
            if (connect.connect != null)
            {
                connect.connect.Send(new Packet.CommonType( Packet.PacketType.Disconnect ));
                
            }
        }
    }

}
