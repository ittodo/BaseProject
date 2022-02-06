using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packet
{
    public class Version : Socket.Connection.Process.IDeserializeData, Socket.Connection.Process.ISerializeData , IPoolObject
    {
        static readonly PacketType Type = PacketType.Version;


        public string Value;


        public Version(string version)
        {
            Value = version;
        }

        public Version()
        {
            
        }

        public void Deserialize(PacketStream ps)
        {
            var reader = new Socket.Serialize.Binary(ps.GetPacketSpan());
            reader.ReadUInt();
            var version = reader.ReadString();
            Value = version.Value;
        }

        public int Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps.GetPacketSpan());
            writer.Write((uint)Type);
            writer.Write(Value);
            return writer.position;
        }


        public static void AddHandle(Socket.Connection.Process.Recive recive)
        {
            Pool.Static.CreateOrAddPool<Packet.Version>();

            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                var st = Socket.Memory.Pool.Static.Create<Socket.Memory.PacketStream>();
                st.SetMemory(T);
                var version = Pool.Static.Create<Packet.Version>();
                version.Deserialize(st);
                return version;
            });
        }

        // Pool Interface
        public bool IsUsed { get; set; }

        public void InitInstance()
        {
            this.Value = null;
        }

        public void Use()
        {
            this.Value = null;
            //throw new NotImplementedException();
        }

        public void Clear()
        {
            this.Value = null;
            //throw new NotImplementedException();
        }
    }
}
