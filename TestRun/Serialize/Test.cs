using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestRun.Serialize
{
    public class Test
    {
        public void Execute()
        {
            Socket.Static.Static.CreateInstance();
            Socket.Memory.PacketStream stream = new Socket.Memory.PacketStream();
            int integer = 1;
            uint uinteger = 2;

            var pp = stream.GetSpan();

            

            Socket.Serialize.Binary b = new Socket.Serialize.Binary(pp);
            b.Write(integer);
            b.Write(uinteger);
            b.Write("Unicode  한글 석ㄲ어서 ㅏㅈㄹ댬ㄹ,ㅡ퍄쟐쟐ㅈㄹㅈㄷ랴ㅑㄹ❤❤❤❤🧡❣💕🧡❤");
            Encoding(b);

            b = new Socket.Serialize.Binary(pp);
            var pair1 = b.ReadInt();
            var pair2 = b.ReadUInt();
            var pair3 = b.ReadString();
            var pair4 = b.ReadInt();


            Socket.Memory.BulkByte bulk = new Socket.Memory.BulkByte(16);
            Random r = new Random();
            List<Socket.Memory.BulkByte.Handle> handle = new List<Socket.Memory.BulkByte.Handle>();

            for (int k = 0; k < 20; k++)
            {
                for (int i = 0; i < 20; i++)
                {
                    var pair = bulk.New();
                    var span = pair.Item1.Span;
                    span.Fill(0xf0);
                    handle.Add(pair.Item2);
                }

                for (int i = 0; i < 20; i++)
                {
                    var index = r.Next(0, handle.Count);
                    var h = handle[index];
                    handle.RemoveAt(index);
                    bulk.Release(h);
                }


                for (int i = 0; i < 40000; i++)
                {
                    var pair = bulk.New();
                    var span = pair.Item1.Span;
                    span.Fill(0xfe);
                    handle.Add(pair.Item2);
                }

            }

            int j = 0;
            j++;


    }

        public void Encoding(Socket.Serialize.Binary b)
        {
            b.Write(1);
        }
    }
}
