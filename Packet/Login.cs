using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packet
{
    public class Login : Socket.Connection.Process.IDeserializeData, Socket.Connection.Process.ISerializeData
    {
        static readonly PacketType Type = PacketType.Login;

        public Login()
        {
            
        }

        public void Deserialize(PacketStream ps)
        {
            
        }

        public void Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps);
            writer.Write((uint)Type);
            writer.WriteHeader();
        }


        public static void AddHandle(Socket.Connection.Process.Recive recive)
        {
            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                return null;
            });
        }
    }
}
