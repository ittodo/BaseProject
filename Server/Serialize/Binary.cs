using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        //private Memory.IStream _buffer;

        Span<byte> cache;
        #endregion
        public int position { get; private set; }


        //public Binary(Memory.IStream stream )
        //{
        //    this._buffer = stream;
        //    this._buffer.Position = 4;
        //    this.cache = stream.GetSpan();
        //    this.position = 0;
        //}

        public Binary(Span<byte> cache)
        {
            
            this.cache = cache;
            this.position = 0;
        }

        public bool Flush()
        {
            return true;
        }

        //public (bool IsRead , uint Value) ReadHeader()
        //{
        //    var value = BitConverter.ToUInt32(_buffer.ReadHeader());
        //    return (true, value);
        //}

        public (bool IsRead, double Value) ReadDouble()
        {
            var value = BitConverter.ToDouble(cache.Slice(position , 8));
            position += 8;
            return (true, value);
        }

        public (bool IsRead, float Value) ReadFloat()
        {
            var value = BitConverter.ToSingle(cache.Slice(position, 4));
            position += 4;
            return (true, value);
        }

        public (bool IsRead, int Value) ReadInt()
        {
            int value = BitConverter.ToInt32(cache.Slice(position, 4));
            position += 4;
            return (true ,value);
        }

        public (bool IsRead, long Value) ReadLong()
        {
            var value = BitConverter.ToInt64(cache.Slice(position, 8));
            position += 8;
            return (true, value);
        }

        public (bool IsRead, short Value) ReadShort()
        {
            var value = BitConverter.ToInt16(cache.Slice(position, 2));
            position += 2;
            return (true, value);
        }

        public (bool IsRead, string Value) ReadString()
        {
            Static.Static.StringBuilder.Clear();
            var countkvp = ReadUInt();            
            
            var value = Encoding.Unicode.GetString(cache.Slice(position, 2 * (int)countkvp.Value));
            position += 2 * (int)countkvp.Value;
            return (true , value);
        }

        private (bool IsRead, char Value) ReadChar()
        {
            var value = BitConverter.ToChar(cache.Slice(position, 2));
            position += 2;
            return (true, value);
        }

        public (bool IsRead, uint Value) ReadUInt()
        {
            var value = BitConverter.ToUInt32(cache.Slice(position, 4));
            position += 4;
            return (true, value);
        }

        public (bool IsRead, ulong Value) ReadULong()
        {
            var value = BitConverter.ToUInt64(cache.Slice(position, 8));
            position += 8;
            return (true, value);
        }

        public (bool IsRead, ushort Value) ReadUShort()
        {
            var value = BitConverter.ToUInt16(cache.Slice(position, 2));
            position += 2;
            return (true, value);
        }

        private unsafe bool Write(byte* source, byte* dest, int position)
        {
            System.Buffer.MemoryCopy(source, dest, position, position);
            this.position += position;
            
            return true;
        }

        public bool Write(int value)
        {
            //temp[0] = (byte)(value);
            //temp[1] = (byte)(value >> 8);
            //temp[2] = (byte)(value >> 16);
            //temp[3] = (byte)(value >> 24);
            //Span<byte> tempInt = new Span<byte>(temp,0, 4);
            //_buffer.Write(tempInt);

            //System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(cache, value);
            

            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 4);
                }
            }
            return true;
        }

        public unsafe bool Write(uint value)
        {
            //Span<byte> p = stackalloc byte[2];
            //temp[0] = (byte)(value);
            //temp[1] = (byte)(value >> 8);
            //temp[2] = (byte)(value >> 16);
            //temp[3] = (byte)(value >> 24);
            //Span<byte> tempInt = new Span<byte>(temp, 0, 4);
            //_buffer.Write(tempInt);

            //var span = _buffer.GetSpan();
            //System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(span, value);


            //System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(cache.Slice(position) , value);


            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 4);
                }
            }


            return true;
        }

        

        public bool Write(short value)
        {
            //temp[0] = (byte)(value);
            //temp[1] = (byte)(value >> 8);
            //Span<byte> tempshort = new Span<byte>(temp, 0, 2);
            //_buffer.Write(tempshort);

            //System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian(cache, value);


            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 2);
                }
            }
            

            return true;
        }

        unsafe public bool Write(ushort value)
        {
            

            //temp[0] = (byte)(value);
            //temp[1] = (byte)(value >> 8);

            //Span<byte> tempshort = new Span<byte>(temp, 0, 2);

            //_buffer.Write(tempshort);


            //System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(cache, value);

            
            fixed (byte* array = &cache[position])
            {
                Write((byte*)(&value), array, 2);
            }
            return true;
        }

        public bool Write(string value)
        {
            var length = value.Length;

            Write((uint)length);

            unsafe
            {
                fixed(char * source = value)
                {
                    fixed (byte* array = &cache[position])
                    {
                        Write((byte *)source, array, length * 2);
                    }
                }
            }


            //int i = 0;
            //int bytecount;
            //for(; i < value.Length/8; i++)
            //{
            //    bytecount = Encoding.Unicode.GetBytes(value, i*8, 8, this.temp, 0);
            //    Span<byte> longbyte = new Span<byte>(temp, 0, bytecount);
            //    longbyte.CopyTo(cache);
            //    //_buffer.Write(longbyte);
            //}

            //var remainLength = value.Length - i * 8;
            //bytecount = Encoding.Unicode.GetBytes(value, i * 8, remainLength, this.temp, 0);
            //Span<byte> remainbyte = new Span<byte>(temp, 0, bytecount);

            //remainbyte.CopyTo(cache);
            ////_buffer.Write(remainbyte);
            
            return true;
        }

        public bool Write(char value)
        {
            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 2);
                }
            }

            return true;
        }

        public bool Write(long value)
        {
            //temp[0] = (byte)(value);
            //temp[1] = (byte)(value >> 0x8);
            //temp[2] = (byte)(value >> 0x10);
            //temp[3] = (byte)(value >> 0x18);
            //temp[4] = (byte)(value >> 0x20);
            //temp[5] = (byte)(value >> 0x28);
            //temp[6] = (byte)(value >> 0x30);
            //temp[7] = (byte)(value >> 0x38);

            //Span<byte> templong = new Span<byte>(temp, 0, 8);

            //_buffer.Write(templong);


            //System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(cache, value);


            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 8);
                }
            }
            return true;
        }

        public bool Write(ulong value)
        {
            //temp[0] = (byte)(value);
            //temp[1] = (byte)(value >> 0x8);
            //temp[2] = (byte)(value >> 0x10);
            //temp[3] = (byte)(value >> 0x18);
            //temp[4] = (byte)(value >> 0x20);
            //temp[5] = (byte)(value >> 0x28);
            //temp[6] = (byte)(value >> 0x30);
            //temp[7] = (byte)(value >> 0x38);

            //Span<byte> templong = new Span<byte>(temp, 0, 8);

            //_buffer.Write(templong);


            //System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(cache, value);

            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 8);
                }
            }
            return true;
        }

        

        public bool Write(float value)
        {
            //uint val = *((uint*)&value);
            ////Write(val);

            //System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(cache, val);


            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 4);
                }
            }

            return true;
        }

        public bool Write(double value)
        {
            //ulong val = *((ulong*)&value);
            ////Write(val);

            //System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(cache, val);

            unsafe
            {
                fixed (byte* array = &cache[position])
                {
                    Write((byte*)(&value), array, 8);
                }
            }
            return true;
        }

        

    }
}
