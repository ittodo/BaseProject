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

        public SocketAsyncEventArgs AsyncEvent { get; set; }

        Memory.PacketStream Stream;

        public Data.SocketAdapter SocketAdapter;

        int remain = 0;

        System.Net.Sockets.Socket socket { get; set; }
        Process.Recive reciveProcess { get; set; }

        private IConnect connect;

        public SocketControl()
        {
            AsyncEvent = new SocketAsyncEventArgs();
            var handle = new EventHandler<SocketAsyncEventArgs>(io_Completed);
            AsyncEvent.Completed += handle;

        }

        ~SocketControl()
        {
        }

        public void Create(IConnect connect , Process.Recive reciveProcess, System.Net.Sockets.Socket socket)
        {
            Console.WriteLine("Connect " + socket.RemoteEndPoint);


            this.connect = connect;

            SocketAdapter = new Data.SocketAdapter(this);

            SocketAdapter.Socket = socket;

            AsyncEvent.UserToken = SocketAdapter;

            Stream = Pool.Static.Create<Memory.PacketStream>();

            AsyncEvent.SetBuffer(Stream.GetRecivePacketMemory());

            this.socket = socket;

            this.reciveProcess = reciveProcess;

            AsyncRecive(AsyncEvent);

            connect.AddSocketControl(this);
        }

        public void InitInstance()
        {

        }

        public void Clear()
        {
            if (Stream != null)
            {
                Pool.Static.Remove(Stream);
                Stream = null;
            }

        }

        public void Use()
        {
            if (Stream != null)
            {
                Pool.Static.Remove(Stream);
                Stream = null;
            }
        }

        public void io_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    Received(e);
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

        public void AsyncRecive(SocketAsyncEventArgs e)
        {
            bool willRaiseEvent = socket.ReceiveAsync(AsyncEvent);
            if (!willRaiseEvent)
            {
                Received(e);
            }
        }

        public void Received(SocketAsyncEventArgs e)
        {
            Data.SocketAdapter token = (Data.SocketAdapter)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                var memory = Stream.GetRecivePacketMemory(remain);

                int startPosition = 0;

                int stackRemain = remain;

                do
                {
                    var m = memory.Slice(startPosition);
                    if (m.Length < 4)
                    {
                        Stream.Position = 1024 - m.Length;
                        Stream.Write(m.Span);
                        remain = m.Length;
                        break;
                    }

                    var st = Pool.Static.Create<Memory.PacketStream>();
                    st.SetMemory(m);

                    var bi = new Socket.Serialize.Binary(st);
                    var PacketSize = bi.ReadHeader();

                    if ((PacketSize.Value + 4) > m.Length)
                    {
                        Stream.Position = 1024 - m.Length;
                        Stream.Write(m.Span);
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

                } while (startPosition < (e.BytesTransferred + stackRemain));
                //token.ProcessPacket(memory);
                AsyncRecive(e);
            }
            else
            {
                this.SocketAdapter.DisconnectSocket();
            }
        }

    }
}
