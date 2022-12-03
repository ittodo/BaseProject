using global::Memory;
using System;
using System.Collections.Generic;
using System.Text;



namespace Socket.Connection.Data
{
    public class PacketWriter : Memory.IPoolObject
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

            var packetSize = _Send.Serialize(st);

            st.WriteHeader(packetSize);
            st.Position += packetSize;

            eventArgs.SetBuffer(st.GetSendPacketMemory());
        }

        public void Write(Memory.PacketStream stream)
        {
            st = stream;

            eventArgs.SetBuffer(st.GetSendPacketMemory());
        }

        public void WriteZeroByte()
        {
            st = Pool.Static.Create<Memory.PacketStream>();
            st.Position = 0;
            eventArgs.SetBuffer(st.GetSendPacketMemory());
        }

        public void ioCompleted(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
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

        }

        public void SendComplete()
        {

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
            //this.eventArgs.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(ioCompleted);
        }

        public void Use()
        {
            
        }
    }
}
