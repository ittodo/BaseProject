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

        IPEndPoint localEndPoint;

        System.Net.Sockets.Socket listenSocket;

        SocketAsyncEventArgs listenAsync;

        public Process.Recive Recive { get; set; }

        public readonly int MaxUserCount;

        public ConcurrentQueue<SocketAdapter> Connection;


        public Listen(IPEndPoint _LocalEndPoint , int MaxUserCount = 1000)
        {
            this.MaxUserCount = MaxUserCount;

            localEndPoint = _LocalEndPoint;

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

            listenAsync.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);

            Accept();
        }

        public void Close()
        {
            listenSocket.Close();

            foreach(var conn in Connection)
            {
                conn.DisconnectSocket();
            }
        }

        private void Accept()
        {
            listenAsync.AcceptSocket = null;
            bool willRaiseEvent = listenSocket.AcceptAsync(listenAsync);
            if (!willRaiseEvent)
            {
                ProcessAccept(listenAsync);
            }
        }

        private void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                var connect = Memory.Pool.Static.Create<SocketControl>();

                connect.Create(this, Recive, e.AcceptSocket);
                Accept();
            }
            else
            {
                int i = 0;
                i++;
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
