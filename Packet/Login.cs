using Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packet
{
    public class Login : Socket.Connection.Process.IDeserializeData, Socket.Connection.Process.ISerializeData, IPoolObject
    {
        static readonly PacketType Type = PacketType.Login;

        public string Name;


        public Login()
        {
        }

        public void Deserialize(Socket.Serialize.Binary binary)
        {
            var reader = binary;
            var Name = reader.ReadString();
            this.Name = Name.Value;
        }

        public int Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps.GetSendPacketSpan());
            writer.Write(this.Name);
            return writer.position;
        }


        public static void AddHandle(Socket.Connection.Process.Recive recive)
        {
            Pool.Static.CreateOrAddPool<Packet.Login>();
            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                var version = Pool.Static.Create<Packet.Login>();
                version.Deserialize(T);
                return version;
            });
        }

        public bool IsUsed { get; set; }

        public void InitInstance()
        {
            this.Name = null;
        }

        public void Use()
        {
            this.Name = null;
        }

        public void Clear()
        {
            this.Name = null;
        }
    }
}
