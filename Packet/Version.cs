using Memory;
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

        public void Deserialize(Socket.Serialize.Binary binary)
        {
            var reader = binary;
            reader.ReadUInt();
            var version = reader.ReadString();
            Value = version.Value;
        }

        public int Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps.GetSendPacketSpan());
            writer.Write((uint)Type);
            writer.Write(Value);
            return writer.position;
        }

        public static void AddHandle(Socket.Connection.Process.Recive recive)
        {
            Pool.Static.CreateOrAddPool<Packet.Version>();

            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                var version = Pool.Static.Create<Packet.Version>();
                version.Deserialize(T);
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
