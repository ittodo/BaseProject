using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Data
{
    public class PacketSender : IPoolObject
    {
        System.Net.Sockets.SocketAsyncEventArgs eventArgs;
        Memory.PacketStream st;

        SocketAdapter parent;
        
        public PacketSender()
        {

        }

        public void Create(SocketAdapter adapter)
        {
            this.parent = adapter;
            eventArgs.UserToken = adapter;
        }

        public void Send(Process.ISerializeData _Send)
        {
            st = Pool.Static.Create<Memory.PacketStream>();

            _Send.Serialize(st);

            //var writer = new Socket.Serialize.Binary(st);

            //int ii = 1;
            //uint ui = 2;
            //long l = 3;
            //ulong ul = 4;
            //short s = 5;
            //ushort us = 0;
            //float f = 0.0f;
            //double d = 0.0;
            //string text = "bf\0iek\0";
            //writer.Write(ii);
            //writer.Write(ui);
            //writer.Write(l);
            //writer.Write(ul);
            //writer.Write(s);
            //writer.Write(us);
            //writer.Write(f);
            //writer.Write(d);
            //writer.Write(text);
            //writer.WriteHeader();

            eventArgs.SetBuffer(st.GetSendPacketMemory());
            parent.Socket.SendAsync(eventArgs);

            
        }

        public void io_Completed(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case System.Net.Sockets.SocketAsyncOperation.Send:
                    {
                        if(e.SocketError == System.Net.Sockets.SocketError.Success)
                        {
                            
                        }
                        else
                        {
                            parent.DisconnectSocket();
                        }
                    }
                    break;
                default:
                    parent.DisconnectSocket();
                    break;
            }
            Pool.Static.Remove(st);
            st = null;
            Pool.Static.Remove(this);
        }

        public bool IsUsed { get; set; }

        public void Clear()
        {

        }

        public void InitInstance()
        {
            this.eventArgs = new System.Net.Sockets.SocketAsyncEventArgs();
            this.eventArgs.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(io_Completed);
        }

        public void Use()
        {
            
        }
    }
}
