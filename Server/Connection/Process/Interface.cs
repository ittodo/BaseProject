using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Process
{
    public interface IDeserializeData
    {
        void Deserialize(Socket.Serialize.Binary binary);
    }

    public interface ISerializeData
    {
        int Serialize(Memory.PacketStream ps);
    }
}
