using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Socket.Memory
{
    
    public class PacketStream : IStream , IPoolObject
    {
        private static BulkByte bulk = new BulkByte(Const.StreamCapacity);

        private BulkByte.Handle memoryhandle;
        byte[] buffer;
        System.Memory<byte> memory;
        private PacketStream(int capacity)
        {
            Capacity = capacity;
            var pair = bulk.New();
            memory = pair.Item1;
            memoryhandle = pair.Item2;
            Position = 4;
        }

        public int Capacity { get; set; }
        public int Position { get; set; }
        public int Remain { get; set; }
        public int ByteTransferred { get; set; }

        public int RemainBytes
        {
            get
            {
                return (ByteTransferred + Remain) - Position;
            }
        }


        public bool IsUsed { get; set; }

        
        public PacketStream()
            : this(Const.StreamCapacity)
        {
            // Pool Create Object
        }

        public void SetMemory(Memory<byte> memory)
        {
            Capacity = memory.Length;
            this.memory = memory;
            Position = 4;
        }

        public void InitInstance()
        {

        }

        public void Use()
        {
            //memory = new System.Memory<byte>(memory, 0, Const.StreamCapacity);
            Position = 4;
            //Get
        }

        public void Clear()
        {
            //memory = new System.Memory<byte>(this.buffer, 0, Const.StreamCapacity);
            Position = 4;
            //Put
        }

        public (bool IsRead, uint Value) ReadHeader()
        {
            if((ByteTransferred + Remain - Position ) < 4)
            {
                return (false, uint.MaxValue);
            }
            var header = memory.Span.Slice( (Const.StreamCapacity / 2) + Position - Remain, 4);
            var value = BitConverter.ToUInt32(header);

            Position += 4;
            return (true , value);
        }

        public Span<byte> Read(int Count)
        {
            int start = Position;
            Position += Count;

            return memory.Span.Slice(start, Count);
        }

        public void WriteHeader(int position)
        {
            var span = memory.Span.Slice(0, 4);
            var length = Position;

            span[0] = (byte)(length);
            span[1] = (byte)(length >> 8);
            span[2] = (byte)(length >> 16);
            span[3] = (byte)(length >> 24);
        }

        public void Write(Span<byte> value)
        {
            //var span = memory.Span.Slice(Position, value.Length);
            //value.CopyTo(span);
            
            Span<byte> to = memory.Slice(Position, value.Length).Span;
            value.CopyTo(to);


            for(int i = 0; i < value.Length; i++)
            {
                buffer[Position + i] = value[i];
            }

            Position += value.Length;
        }

        public Span<byte> GetSpan()
        {
            return memory.Span.Slice(Position);
        }

        public Memory<byte> GetMemory()
        {
            return memory;
        }

        public Memory<byte> GetSendPacketMemory()
        {
            return memory.Slice(0, Position);
        }

        public Memory<byte> GetRecivePacketMemory()
        {
            return memory.Slice(1024, Const.PacketSize);
        }

        public Memory<byte> GetRecivePacketMemory(int remain)
        {
            return memory.Slice(1024 - remain, Const.PacketSize + remain);
        }

        public Span<byte> GetPacketSpan(int remain)
        {
            return memory.Slice(1024 - remain, Const.PacketSize + remain).Span;
        }


        // 읽지 못한 Bytes
        public Span<byte> GetPacketSpan()
        {
            return memory.Slice(1024 + Position, Const.PacketSize + Position).Span;
        }
    }
}
