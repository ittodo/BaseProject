using Socket.Connection.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Socket.Connection
{
    public class Listen : IConnect
    {
        public delegate void ConnectionRecivePacketHandler(Data.SocketAdapter _Connect);

        public Process.Recive Recive { get; set; }


        private IPEndPoint localEndPoint;

        private System.Net.Sockets.Socket listenSocket;

        private SocketAsyncEventArgs listenAsync;

        public readonly int MaxUserCount;

        public ConcurrentQueue<SocketAdapter> Connection;


        public Listen(IPEndPoint LocalEndPoint , int MaxUserCount = 1000)
        {
            this.MaxUserCount = MaxUserCount;

            localEndPoint = LocalEndPoint;

            Memory.Pool.Static.CreateOrAddPool<SocketControl>(this.MaxUserCount);

            Recive = new Process.Recive();

            Connection = new ConcurrentQueue<SocketAdapter>();
        }

        public void Start()
        {
            listenSocket = new System.Net.Sockets.Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listenSocket.Bind(localEndPoint);

            // start the server with a listen backlog of 100 connections
            listenSocket.Listen(this.MaxUserCount);

            // Listen socket
            listenAsync = new SocketAsyncEventArgs();

            listenAsync.Completed += new EventHandler<SocketAsyncEventArgs>(acceptCompleted);

            accept();
        }

        public void Close()
        {
            listenSocket.Close();

            foreach(var conn in Connection)
            {
                conn.DisconnectSocket();
            }
        }

        private void accept()
        {
            listenAsync.AcceptSocket = null;
            bool willRaiseEvent = listenSocket.AcceptAsync(listenAsync);
            if (!willRaiseEvent)
            {
                processAccept(listenAsync);
            }
        }

        private void acceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            processAccept(e);
        }

        private void processAccept(SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                var connect = Memory.Pool.Static.Create<SocketControl>();

                connect.Create(this, Recive, e.AcceptSocket);

                accept();
            }
        }

        void IConnect.AddSocketControl(SocketControl control)
        {
            Connection.Enqueue(control.SocketAdapter);
        }

        void IConnect.RemoveSocketControl(SocketControl control)
        {
            Connection.TryDequeue(out control.SocketAdapter);
        }
    }
}
