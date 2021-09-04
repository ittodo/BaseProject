using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket.Serialize
{
    public enum ReadType
    {
        INT, UINT, Short, UShort,String , Long, ULong, Float , Double
    }
    interface ISerialize
    {
        bool Write(int value);
        bool Write(uint value);
        bool Write(short value);
        bool Write(ushort value);
        bool Write(string value);
        bool Write(long value);
        bool Write(ulong value);
        bool Write(float value);
        bool Write(double value);
        bool Flush();

        (bool IsRead ,int Value) ReadInt();
        (bool IsRead, uint Value) ReadUInt();
        (bool IsRead, short Value) ReadShort();
        (bool IsRead, ushort Value) ReadUShort();
        (bool IsRead, string Value) ReadString();
        (bool IsRead, long Value) ReadLong();
        (bool IsRead, ulong Value) ReadULong();
        (bool IsRead, float Value) ReadFloat();
        (bool IsRead, double Value) ReadDouble();
    }
}
