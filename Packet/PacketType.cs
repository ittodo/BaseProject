using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Packet
{
    
    public enum PacketType : uint
    {
        Error = 0,
        Version ,
        Login,
        Disconnect,

    }
}
