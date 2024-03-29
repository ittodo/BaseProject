﻿using System;
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
                    Data.Static.UserContainer.Remove(current);
                    current.DisConnectLogic();
                    Console.WriteLine("Disconnect");
                    continue;
                }

                var item = current.Get();
                while (item.Item1 != 0)
                {
                    if (item.Item1 == (uint)global::Packet.PacketType.Version)
                    {
                        var version = item.Item2 as global::Packet.Version;
                        Console.WriteLine(version.Value);
                        Memory.Pool.Static.Remove(version);
                    }
                    else if (item.Item1 == (uint)global::Packet.PacketType.Login)
                    {
                        Console.WriteLine("Login");
                        var Login = item.Item2 as global::Packet.Login;
                        Console.WriteLine(Login.Name);

                        var user = Data.Static.UserContainer.Add(current);
                        user.PoolMember.Login = Login;
                    }
                    else if(item.Item1 == (uint)global::Packet.PacketType.Disconnect)
                    {
                        Data.Static.UserContainer.Remove(current);
                        current.DisConnectLogic();
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
