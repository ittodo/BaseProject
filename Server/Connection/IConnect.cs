using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection
{
    internal interface IConnect
    {
        void AddSocketControl(SocketControl control);
        void RemoveSocketControl(SocketControl control);
    }
}
