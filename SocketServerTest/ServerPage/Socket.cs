using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest.ServerPage
{
    public class Socket
    {
        global::Socket.Connection.Listen userListen;
        public Socket()
        {
            global::Socket.Run.Init();

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipaddress = null;
            foreach (var item in ipHostInfo.AddressList)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipaddress = item;
                }
            }
            IPEndPoint localendpoint = new IPEndPoint(ipaddress, 11001);
            userListen = new global::Socket.Connection.Listen(localendpoint);
            userListen.Start();
        }

        public void Close()
        {
            userListen.Close();
            userListen = null;
        }
    }
}
