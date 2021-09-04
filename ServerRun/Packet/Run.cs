using System;
using System.Collections.Generic;
using System.Text;

namespace ServerRun.Packet
{
    public static class Run
    {
        public static void Execute(Socket.Connection.Listen listen)
        {
            var itr = listen.Connection.GetEnumerator();
            while (itr.MoveNext())
            {
                var current = itr.Current;
                if (current.IsDisconnectSocket == true)
                {
                    current.DisConnectLogic();
                    Console.WriteLine("Disconnect");
                }

                var item = current.Get();
                while (item.Item1 != 0)
                {
                    if (item.Item1 == (uint)global::Packet.PacketType.Version)
                    {
                        var version = item.Item2 as global::Packet.Version;
                        Console.WriteLine(version.Value);
                    }
                    else if (item.Item1 == (uint)global::Packet.PacketType.Login)
                    {
                        Console.WriteLine("Login");
                    }
                    else if(item.Item1 == (uint)global::Packet.PacketType.Disconnect)
                    {
                        Console.WriteLine("Disconnect");
                        global::ServerRun.Run.CleanUp();
                    }
                    item = current.Get();
                }
            }
            System.Threading.Thread.Sleep(10);

        }

        public static void CleanUp(Socket.Connection.Listen listen)
        {
            var itr = listen.Connection.GetEnumerator();
            while (itr.MoveNext())
            {
                var current = itr.Current;

                itr.Current.DisConnectLogic();
                Console.WriteLine("CleanUp");
            }
        }
    }
}
