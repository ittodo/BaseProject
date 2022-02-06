using Socket.Memory;
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

        public void Deserialize(PacketStream ps)
        {
            var reader = new Socket.Serialize.Binary(ps.GetPacketSpan());
            var Name = reader.ReadString();
            this.Name = Name.Value;
        }

        public int Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps.GetPacketSpan());
            writer.Write(this.Name);
            return writer.position;
        }


        public static void AddHandle(Socket.Connection.Process.Recive recive)
        {
            Pool.Static.CreateOrAddPool<Packet.Login>();
            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                var st = Socket.Memory.Pool.Static.Create<Socket.Memory.PacketStream>();
                st.SetMemory(T);
                var version = Pool.Static.Create<Packet.Login>();
                version.Deserialize(st);
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
