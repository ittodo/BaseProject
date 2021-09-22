using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packet
{
    public class CommonType : Socket.Connection.Process.IDeserializeData, Socket.Connection.Process.ISerializeData
    {
        readonly PacketType Type;
        public CommonType (PacketType Type)
        {
            this.Type = Type;
        }

        public void Deserialize(PacketStream ps)
        {
            
        }

        public int Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps.GetPacketSpan());
            writer.Write((uint)Type);
            return writer.position;
        }

        public static void AddHandle(PacketType Type , Socket.Connection.Process.Recive recive)
        {
            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                return null;
            });
        }
    }
}
