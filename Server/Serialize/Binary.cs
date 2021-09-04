using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Socket.Serialize
{
    /// <summary>
    /// ref struct 무조건 한 스택 내에서만 사용 가능 밖으로 못나감
    /// </summary>
    public ref struct Binary
    {
        #region Private
        private byte[] temp;
        private Memory.IStream _buffer;
        #endregion


        public Binary(Memory.IStream stream )
        {
            temp = Static.Static.SerializeBinaryBytes.Value;
            _buffer = stream;
            _buffer.Position = 4;
        }

        public bool Flush()
        {
            return true;
        }

        public (bool IsRead , uint Value) ReadHeader()
        {
            var value = BitConverter.ToUInt32(_buffer.ReadHeader());
            return (true, value);
        }

        public (bool IsRead, double Value) ReadDouble()
        {
            var value = BitConverter.ToDouble(_buffer.Read(8));
            return (true, value);
        }

        public (bool IsRead, float Value) ReadFloat()
        {
            var value = BitConverter.ToSingle(_buffer.Read(4));
            return (true, value);
        }

        public (bool IsRead, int Value) ReadInt()
        {
            int value = BitConverter.ToInt32(_buffer.Read(4));
            return (true ,value);
        }

        public (bool IsRead, long Value) ReadLong()
        {
            var value = BitConverter.ToInt64(_buffer.Read(8));
            return (true, value);
        }

        public (bool IsRead, short Value) ReadShort()
        {
            var value = BitConverter.ToInt16(_buffer.Read(2));
            return (true, value);
        }

        public (bool IsRead, string Value) ReadString()
        {
            Static.Static.StringBuilder.Clear();
            var countkvp = ReadUInt();
            var value = Encoding.Unicode.GetString(_buffer.Read(2 * (int)countkvp.Value));
            return (true , value);
        }

        private (bool IsRead, char Value) ReadChar()
        {
            var value = BitConverter.ToChar(_buffer.Read(2));
            return (true, value);
        }

        public (bool IsRead, uint Value) ReadUInt()
        {
            var value = BitConverter.ToUInt32(_buffer.Read(4));
            return (true, value);
        }

        public (bool IsRead, ulong Value) ReadULong()
        {
            var value = BitConverter.ToUInt64(_buffer.Read(8));
            return (true, value);
        }

        public (bool IsRead, ushort Value) ReadUShort()
        {
            var value = BitConverter.ToUInt16(_buffer.Read(2));
            return (true, value);
        }

        public bool Write(int value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 8);
            temp[2] = (byte)(value >> 16);
            temp[3] = (byte)(value >> 24);
            Span<byte> tempInt = new Span<byte>(temp,0, 4);

            _buffer.Write(tempInt);
            return true;
        }

        public bool Write(uint value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 8);
            temp[2] = (byte)(value >> 16);
            temp[3] = (byte)(value >> 24);

            Span<byte> tempInt = new Span<byte>(temp, 0, 4);

            _buffer.Write(tempInt);
            return true;
        }

        public bool Write(short value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 8);

            Span<byte> tempshort = new Span<byte>(temp, 0, 2);

            _buffer.Write(tempshort);
            return true;
        }

        public bool Write(ushort value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 8);

            Span<byte> tempshort = new Span<byte>(temp, 0, 2);

            _buffer.Write(tempshort);
            return true;
        }

        public bool Write(string value)
        {
            Write((uint)value.Length);
            int i = 0;
            int bytecount;
            for(; i < value.Length/4; i++)
            {
                bytecount = Encoding.Unicode.GetBytes(value, i*4, 4, this.temp, 0);
                Span<byte> longbyte = new Span<byte>(temp, 0, bytecount);
                _buffer.Write(longbyte);
            }

            var remainLength = value.Length - i * 4;
            bytecount = Encoding.Unicode.GetBytes(value, i * 4, remainLength, this.temp, 0);
            Span<byte> remainbyte = new Span<byte>(temp, 0, bytecount);
            _buffer.Write(remainbyte);
            
            return true;
        }

        public bool Write(char value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 8);

            Span<byte> tempshort = new Span<byte>(temp, 0, 2);

            _buffer.Write(tempshort);
            return true;
        }

        public bool Write(long value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 0x8);
            temp[2] = (byte)(value >> 0x10);
            temp[3] = (byte)(value >> 0x18);
            temp[4] = (byte)(value >> 0x20);
            temp[5] = (byte)(value >> 0x28);
            temp[6] = (byte)(value >> 0x30);
            temp[7] = (byte)(value >> 0x38);

            Span<byte> templong = new Span<byte>(temp, 0, 8);

            _buffer.Write(templong);
            return true;
        }

        public bool Write(ulong value)
        {
            temp[0] = (byte)(value);
            temp[1] = (byte)(value >> 0x8);
            temp[2] = (byte)(value >> 0x10);
            temp[3] = (byte)(value >> 0x18);
            temp[4] = (byte)(value >> 0x20);
            temp[5] = (byte)(value >> 0x28);
            temp[6] = (byte)(value >> 0x30);
            temp[7] = (byte)(value >> 0x38);

            Span<byte> templong = new Span<byte>(temp, 0, 8);

            _buffer.Write(templong);
            return true;
        }

        public bool WriteHeader()
        {
            _buffer.WriteHeader();
            return true;
        }

        public unsafe bool Write(float value)
        {
            uint val = *((uint*)&value);
            return Write(val);
        }

        public unsafe bool Write(double value)
        {
            ulong val = *((ulong*)&value);
            return Write(val);
        }

    }
}
