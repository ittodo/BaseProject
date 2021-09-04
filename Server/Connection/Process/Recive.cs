using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Process
{
    public class Recive
    {
        public _HandleControlHeader HeaderMaker;
        public Dictionary<uint, _HandleControlMaker> ControlMaker;

        public delegate uint _HandleControlHeader(Memory<byte> mem);
        public delegate IDeserializeData _HandleControlMaker(Memory<byte> mem);

        public Recive()
        {
            HeaderMaker = null;
            ControlMaker = new Dictionary<uint, _HandleControlMaker>();
        }
    }
}
