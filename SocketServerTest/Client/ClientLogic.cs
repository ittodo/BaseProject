using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest.Client
{
    public class ClientLogic
    {
        global::Socket.Connection.Connect client;
        public ClientLogic()
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
            client = new global::Socket.Connection.Connect(localendpoint);
            client.Start();
        }

        public void Send()
        {
            var  stream = Memory.Pool.Static.Create<Memory.PacketStream>();
            var bi = new Socket.Serialize.Binary(stream.GetMemory().Span);
            bi.Write(4);
            bi.Write(4);
            stream.Position = bi.position;
            client.connect.Send(stream);
        }

        public void Close()
        {
            client.Close();
            client = null;
        }
    }
}
