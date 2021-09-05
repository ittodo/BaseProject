using Socket.Memory;
using Socket.Serialize;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Socket.Connection
{
    internal class SocketControl : IPoolObject
    {
        public bool IsUsed { get; set; }

        public Data.SocketAdapter SocketAdapter;
                
        private System.Net.Sockets.Socket socket { get; set; }

        private SocketAsyncEventArgs asyncEvent { get; set; }

        private int remain = 0;

        private Memory.PacketStream stream;

        private Process.Recive reciveProcess { get; set; }

        private IConnect connect;

        public SocketControl()
        {
            asyncEvent = new SocketAsyncEventArgs();
            var handle = new EventHandler<SocketAsyncEventArgs>(io_Completed);
            asyncEvent.Completed += handle;
        }

        ~SocketControl()
        {
        }

        public void Create(IConnect connect , Process.Recive reciveProcess, System.Net.Sockets.Socket socket)
        {
            Console.WriteLine("Connect " + socket.RemoteEndPoint);

            this.connect = connect;

            this.SocketAdapter = new Data.SocketAdapter(this);

            this.SocketAdapter.Socket = socket;

            this.asyncEvent.UserToken = this.SocketAdapter;

            this.stream = Pool.Static.Create<Memory.PacketStream>();

            this.asyncEvent.SetBuffer(stream.GetRecivePacketMemory());

            this.socket = socket;

            this.reciveProcess = reciveProcess;

            this.connect.AddSocketControl(this);

            AsyncRecive(/*this.asyncEvent*/);
        }

        public void InitInstance()
        {

        }

        public void Clear()
        {
            if (stream != null)
            {
                Pool.Static.Remove(stream);
                stream = null;
            }
        }

        public void Use()
        {
            if (stream != null)
            {
                Pool.Static.Remove(stream);
                stream = null;
            }
        }

        public void io_Completed(object sender, SocketAsyncEventArgs e)
        {
            System.Diagnostics.Debug.Assert(e == asyncEvent);

            switch (asyncEvent.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    Received(/*asyncEvent*/);
                    break;
                default:
                    Close(e);
                    break;
            }
        }

        private void Close(SocketAsyncEventArgs e)
        {
            SocketAdapter.DisconnectSocket();
        }

        public void Release()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            catch { }

            socket.Close();

            connect.RemoveSocketControl(this);
            Memory.Pool.Static.Remove(this);
        }

        public void AsyncRecive(/*SocketAsyncEventArgs e*/)
        {
            bool willRaiseEvent = socket.ReceiveAsync(asyncEvent);
            if (!willRaiseEvent)
            {
                Received(/*asyncEvent*/);
            }
        }

        public void Send(Data.PacketWriter sender)
        {
            socket.SendAsync(sender.eventArgs);
        }

        public void Received()
        {
            Data.SocketAdapter token = (Data.SocketAdapter)asyncEvent.UserToken;
            if (asyncEvent.BytesTransferred > 0 && asyncEvent.SocketError == SocketError.Success)
            {
                var memory = stream.GetRecivePacketMemory(remain);

                int startPosition = 0;

                int stackRemain = remain;

                do
                {
                    var m = memory.Slice(startPosition);
                    if (m.Length < 4)
                    {
                        // remain bytes forward copy
                        stream.Position = 1024 - m.Length;
                        stream.Write(m.Span);
                        remain = m.Length;
                        break;
                    }

                    var st = Pool.Static.Create<Memory.PacketStream>();
                    st.SetMemory(m);

                    var bi = new Socket.Serialize.Binary(st);
                    var PacketSize = bi.ReadHeader();

                    if ((PacketSize.Value + 4) > m.Length)
                    {
                        stream.Position = 1024 - m.Length;
                        stream.Write(m.Span);
                        remain = m.Length;
                        //remain = (int)PacketSize.Value + 4 - m.Length;
                        break;
                    }
                    var thisPacket = m.Slice(0, (int)PacketSize.Value);
                    var handleId = reciveProcess.HeaderMaker(thisPacket);

                    Process.Recive._HandleControlMaker processMaker;
                    reciveProcess.ControlMaker.TryGetValue(handleId, out processMaker);
                    var data = processMaker(thisPacket);
                    SocketAdapter.AddProcessData(handleId, data);

                    //var rww = bi.ReadString();

                    startPosition = startPosition + (int)PacketSize.Value;

                    //Console.WriteLine(startPosition+" : " + PacketSize.ToString() + " : " + e.BytesTransferred.ToString() +" : " + rww.ToString() + " : " + startPosition.ToString());
                    //remain = 0;

                    Pool.Static.Remove(st);

                } while (startPosition < (asyncEvent.BytesTransferred + stackRemain));
                //token.ProcessPacket(memory);
                AsyncRecive(/*e*/);
            }
            else
            {
                this.SocketAdapter.DisconnectSocket();
            }
        }

    }
}
