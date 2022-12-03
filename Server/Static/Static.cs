using Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Socket.Static
{
    public static class Static
    {
        public static StringBuilder StringBuilder;
        public static ThreadLocal<byte[]> SerializeBinaryBytes;
        public static Memory.Pool Pool;

        public static void CreateInstance()
        {
            StringBuilder = new StringBuilder();
            SerializeBinaryBytes = new ThreadLocal<byte[]>(()=> {
                return new byte[16];
            });

            Pool = Pool.Static;
            Pool.CreateOrAddPool<Memory.PacketStream>(2048);
        }

        public static void CleanUp()
        {
            StringBuilder = null;
            SerializeBinaryBytes = null;
            Pool.RemoveAll();
            Pool = null;
        }
    }
}
