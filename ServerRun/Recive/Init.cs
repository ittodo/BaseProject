using System;
using System.Collections.Generic;
using System.Text;

namespace ServerRun.Recive
{
    public static class Init
    {
        public static void AddHandles(Socket.Connection.Process.Recive recive)
        {
            global::Packet.Version.AddHandle(recive);
            global::Packet.Login.AddHandle(recive);

            global::Packet.CommonType.AddHandle(global::Packet.PacketType.Disconnect , recive);
        }
    }
}
