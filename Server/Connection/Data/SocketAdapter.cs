using Socket.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Socket.Connection.Data
{
    public class SocketAdapter
    {
        private bool _disConnectSocket;
        public bool IsDisconnectSocket
        {
            get
            {
                return _disConnectSocket;
            }
        }

        private bool _disConnectLogic;

        public bool IsDisconnectLogic
        {
            get
            {
                return _disConnectLogic;
            }
        }

        private bool IsDisconnect { get; set; }

        SocketControl Control;

        internal System.Net.Sockets.Socket Socket { get; set; }

        // 받는 패킷

        public (uint, Process.IDeserializeData) Get()
        {
            (uint, Process.IDeserializeData) control;
            if(packet.TryDequeue(out control))
            {
                return control;
            }
            return (0, null);
        }

        private System.Collections.Concurrent.ConcurrentQueue<(uint, Process.IDeserializeData)> packet;

        internal SocketAdapter(SocketControl obj )
        {
            Pool.Static.CreateOrAddPool<PacketWriter>();
            packet = new System.Collections.Concurrent.ConcurrentQueue<(uint, Process.IDeserializeData)>();
            this.Control = obj;
            IsDisconnect = false;
        }

        internal void AddProcessData(uint headerId, Process.IDeserializeData _Recive)
        {
            packet.Enqueue( (headerId , _Recive) );
        }

        // To Byte and Send
        public void Send(Process.ISerializeData _Send)
        {
            PacketWriter sender = Pool.Static.Create<PacketWriter>();

            sender.Create(this);

            sender.Write(_Send);

            Control.Send(sender);
        }

        internal void DisconnectSocket()
        {
            _disConnectSocket = true;
            if (_disConnectLogic == true && IsDisconnect == false)
            {
                disConnect();
            }
        }

        public void DisConnectLogic()
        {
            _disConnectLogic = true;
            if (_disConnectSocket == true && IsDisconnect == false)
            {
                disConnect();
            }
        }

        private void disConnect()
        {
            IsDisconnect = true;
            this.Control.Release();
        }
    }
}
