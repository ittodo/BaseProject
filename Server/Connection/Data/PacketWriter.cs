using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socket.Connection.Data
{
    public class PacketWriter : IPoolObject
    {
        public System.Net.Sockets.SocketAsyncEventArgs eventArgs;
        Memory.PacketStream st;

        SocketAdapter parent;
        
        public PacketWriter()
        {

        }

        public void Create(SocketAdapter adapter)
        {
            this.parent = adapter;
            eventArgs.UserToken = adapter;
        }

        public void Write(Process.ISerializeData _Send)
        {
            st = Pool.Static.Create<Memory.PacketStream>();

            _Send.Serialize(st);

            eventArgs.SetBuffer(st.GetSendPacketMemory());
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
