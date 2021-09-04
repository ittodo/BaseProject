using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Socket.Memory
{
    public class PacketStream : IStream , IPoolObject
    {
        byte[] buffer;

        System.Memory<byte> memory;
        public PacketStream(int capacity)
        {
            Capacity = capacity;
            
            buffer = new byte[capacity];
            memory = new System.Memory<byte>(this.buffer, 0, Const.StreamCapacity);
            Position = 4;
        }

        public int Capacity { get; set; }
        public int Position { get; set; }
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
            memory = new System.Memory<byte>(this.buffer, 0, Const.StreamCapacity);
            Position = 4;
            //Get
        }

        public void Clear()
        {
            memory = new System.Memory<byte>(this.buffer, 0, Const.StreamCapacity);
            Position = 4;
            //Put
        }
        public Span<byte> ReadHeader()
        {
            int start = 0;
            Position = 4;
            return memory.Span.Slice(start, 4);
        }

        public Span<byte> Read(int Count)
        {
            int start = Position;
            Position += Count;

            return memory.Span.Slice(start, Count);
        }

        public void WriteHeader()
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
            var span = memory.Span.Slice(Position, value.Length);
            //var span = new Span<byte>(buffer, Position, value.Length);
            value.CopyTo(span);
            Position += value.Length;
        }

        public Memory<byte> GetMemory()
        {
            return memory;
        }

        public Memory<byte> GetSendPacketMemory()
        {
            return new System.Memory<byte>(this.buffer, 0, Position);
        }

        public Memory<byte> GetRecivePacketMemory()
        {
            return new System.Memory<byte>(this.buffer, 1024, Const.PacketSize);
        }

        public Memory<byte> GetRecivePacketMemory(int remain)
        {
            return new System.Memory<byte>(this.buffer, 1024-remain, Const.PacketSize + remain);
        }

        public Span<byte> GetPacketSpan(int remain)
        {
            return new System.Span<byte>(this.buffer, 1024 - remain, Const.PacketSize + remain);
        }
    }
}
