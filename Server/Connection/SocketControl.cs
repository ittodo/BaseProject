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
            var handle = new EventHandler<SocketAsyncEventArgs>(ioCompleted);
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

        private void ioCompleted(object sender, SocketAsyncEventArgs e)
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
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {

            }
            finally
            {
                socket.Close();

                connect.RemoveSocketControl(this);
                Memory.Pool.Static.Remove(this);
            }
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
            sender.eventArgs.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(ioSendCompleted);
            bool willRaiseEvent = socket.SendAsync(sender.eventArgs);
            if(!willRaiseEvent)
            {
                //sender.eventArgs.Completed(sender, sender.eventArgs);
                ioSendCompleted(sender , sender.eventArgs);
            }
        }

        private void ioSendCompleted(object obj, SocketAsyncEventArgs e)
        {
            Data.PacketWriter sender = obj as Data.PacketWriter;
            //System.Diagnostics.Debug.Assert(e == asyncEvent);

            switch (e.LastOperation)
            {
                case System.Net.Sockets.SocketAsyncOperation.Send:
                    {
                        if (e.SocketError == System.Net.Sockets.SocketError.Success)
                        {
                            System.Diagnostics.Trace.WriteLine($"SendPacket {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                        }
                        else
                        {
                            Close(e);
                        }
                    }
                    break;
                default:
                    Close(e);
                    break;
            }
            e.Completed -= new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(ioSendCompleted);
            sender.SendComplete();
            
        }


        public void Received()
        {
            Data.SocketAdapter token = (Data.SocketAdapter)asyncEvent.UserToken;
            if (asyncEvent.BytesTransferred > 0 && asyncEvent.SocketError == SocketError.Success)
            {

                System.Diagnostics.Trace.WriteLine($"Received {asyncEvent.BytesTransferred} {remain}: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                var memory = stream.GetRecivePacketMemory(remain);
                stream.Remain = remain;
                stream.Position = 0;
                stream.ByteTransferred = asyncEvent.BytesTransferred;

                int cal = stream.ByteTransferred + stream.Remain;

                //int startPosition = 0;
                //int stackRemain = remain;

                do
                {
                    var PacketSize = stream.ReadHeader();
                    if (PacketSize.IsRead == false)
                    {
                        var RemainBytes = stream.GetPacketSpan();
                        // remain bytes forward copy
                        stream.Position = 1024 - RemainBytes.Length;
                        stream.Write(RemainBytes);
                        stream.Remain = RemainBytes.Length;
                        remain = RemainBytes.Length;
                        break;
                    }

                    //var st = Pool.Static.Create<Memory.PacketStream>();
                    //st.SetMemory(m);
                    if ((PacketSize.Value) > stream.RemainBytes)
                    {
                        var RemainBytes = stream.GetPacketSpan();
                        stream.Position = 1024 - RemainBytes.Length;
                        stream.Write(RemainBytes);
                        stream.Remain = RemainBytes.Length;
                        remain = RemainBytes.Length;
                        break;
                    }

                    cal -= 4;
                    var bi = new Socket.Serialize.Binary(stream.Read( (int)PacketSize.Value) );
                    cal -= (int)PacketSize.Value;
                    remain = 0;

                    // Bi Read Packet


                    //var thisPacket = m.Slice(0, (int)PacketSize.Value);
                    //var handleId = reciveProcess.HeaderMaker(thisPacket);

                    //Process.Recive._HandleControlMaker processMaker;
                    //reciveProcess.ControlMaker.TryGetValue(handleId, out processMaker);
                    //var data = processMaker(thisPacket);
                    //SocketAdapter.AddProcessData(handleId, data);

                    ////var rww = bi.ReadString();

                    //startPosition = startPosition + (int)PacketSize.Value;

                    //Console.WriteLine(startPosition+" : " + PacketSize.ToString() + " : " + e.BytesTransferred.ToString() +" : " + rww.ToString() + " : " + startPosition.ToString());
                    //remain = 0;

                    if(cal < 0)
                    {
                        break;
                    }

                } while (cal != 0);
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
