using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket.Memory
{
    public interface IStream
    {
        int Capacity { get; set; }

        int Position { get; set; }

        void WriteHeader();
        void Write(Span<byte> value);

        Span<byte> ReadHeader();
        Span<byte> Read(int Count);
    }
}
