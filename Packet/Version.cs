using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packet
{
    public class Version : Socket.Connection.Process.IDeserializeData, Socket.Connection.Process.ISerializeData
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
            var reader = new Socket.Serialize.Binary(ps);
            reader.ReadUInt();
            var version = reader.ReadString();
            Value = version.Value;
        }

        public void Serialize(PacketStream ps)
        {
            var writer = new Socket.Serialize.Binary(ps);
            writer.Write((uint)Type);
            writer.Write(Value);
            writer.WriteHeader();
        }


        public static void AddHandle(Socket.Connection.Process.Recive recive)
        {
            recive.ControlMaker.Add((uint)Type, (T) =>
            {
                var st = Socket.Memory.Pool.Static.Create<Socket.Memory.PacketStream>();
                st.SetMemory(T);
                var version = new Packet.Version();
                version.Deserialize(st);
                return version;
            });
        }
    }
}
