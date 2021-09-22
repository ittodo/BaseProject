using Socket.Connection.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace Socket.Connection
{
    public class Connect : IConnect
    {
        public delegate void ConnectionRecivePacketHandler(Data.SocketAdapter _Connect);

        public Process.Recive Recive { get; set; }


        #region Private
        IPEndPoint localEndPoint;

        System.Net.Sockets.Socket listenSocket;

        SocketAsyncEventArgs listenAsync;
        #endregion

        public SocketAdapter connect;
        public Connect(IPEndPoint localEndPoint)
        {
            this.localEndPoint = localEndPoint;

            Recive = new Process.Recive();
            Memory.Pool.Static.CreateOrAddPool<SocketControl>();
        }

        public void Start()
        {
            listenSocket = new System.Net.Sockets.Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Listen socket
            listenAsync = new SocketAsyncEventArgs();

            listenAsync.RemoteEndPoint = localEndPoint;

            listenAsync.Completed += new EventHandler<SocketAsyncEventArgs>(ioCompleted);

            bool isRaise = listenSocket.ConnectAsync(listenAsync);
            if(!isRaise)
            {
                connectOperation(listenAsync);
            }
        }

        public void Close()
        {
            connect.DisconnectSocket();
            connect = null;
        }

        private void ioCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch(e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    connectOperation(e);
                    break;
                default:
                    Console.WriteLine("ioComplete to {0}",
                    e.LastOperation);
                    break;
            }
        }

        private void connectOperation(SocketAsyncEventArgs e)
        {
            var client = this.listenSocket;
            if(e.SocketError== SocketError.Success)
            {
                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                var accept = Memory.Pool.Static.Create<SocketControl>();
                accept.Create(this, new Process.Recive(), client);
            }
            else
            {

            }
            
        }

        void IConnect.AddSocketControl(SocketControl control)
        {
            connect = control.SocketAdapter;
        }

        void IConnect.RemoveSocketControl(SocketControl control)
        {
            connect = null;
        }
    }
}
