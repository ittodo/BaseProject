using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

using Worker = ThreadPool;

namespace ServerRun
{
    public static class Run
    {
        static Socket.Connection.Listen userListen;
        public static void Execute()
        {
            Socket.Run.Init();

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
            userListen = new Socket.Connection.Listen(localendpoint);

            userListen.Recive.HeaderMaker = ((T) =>
            {
                var st = Socket.Memory.Pool.Static.Create<Socket.Memory.PacketStream>();
                st.SetMemory(T);
                var reader = new Socket.Serialize.Binary(st);
                var rtvalue = reader.ReadUInt();
                return rtvalue.Value;
            });

            Recive.Init.AddHandles(userListen.Recive);

            userListen.Start();

            var manager = Worker.Manager.Create();
            manager.Run(() =>
            {

            });

            manager.Wait();

            var logicUserThread = new Thread(() =>
            {
                do
                {
                    Packet.Run.Execute(userListen);

                } while (cleanUp == 0);
                Packet.Run.CleanUp(userListen);
                
                Program.CleanUp();
            });


            //thread.Start();
            logicUserThread.Start();
            //thread.IsBackground = true;
            logicUserThread.IsBackground = true;
        }

        static int cleanUp = 0;


        // 이게 처음
        public static void CleanUp(  )
        {
            userListen.Close();
            System.Threading.Interlocked.Exchange(ref cleanUp, 1);
            
        }

    }
}
