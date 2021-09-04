using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Process
{
    public interface IDeserializeData
    {
        public void Deserialize(Memory.PacketStream ps);
    }

    public interface ISerializeData
    {
        void Serialize(Memory.PacketStream ps);
    }
}
