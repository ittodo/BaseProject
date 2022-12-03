using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Process
{
    public class Recive
    {
        public _HandleControlHeader ParseHeader;
        public Dictionary<uint, _HandleControlMaker> ControlMaker;

        public delegate uint _HandleControlHeader(Socket.Serialize.Binary binary);
        public delegate IDeserializeData _HandleControlMaker(Socket.Serialize.Binary binary);

        public Recive()
        {
            ParseHeader = null;
            ControlMaker = new Dictionary<uint, _HandleControlMaker>();
        }
    }
}
