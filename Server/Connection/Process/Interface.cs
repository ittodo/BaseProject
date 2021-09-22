using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Process
{
    public interface IDeserializeData
    {
        void Deserialize(Memory.PacketStream ps);
    }

    public interface ISerializeData
    {
        int Serialize(Memory.PacketStream ps);
    }
}
