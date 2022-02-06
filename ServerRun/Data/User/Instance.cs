using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ServerRun.Data.User
{
    public struct PoolMember
    {
        public global::Packet.Login Login;
    }

    public class Instance
    {
        Socket.Connection.Data.SocketAdapter SocketAdapter;

        public PoolMember PoolMember;

        public Instance(Socket.Connection.Data.SocketAdapter SocketAdapter)
        {
            this.SocketAdapter = SocketAdapter;
        }

        public void CleanUp()
        {
            this.SocketAdapter = null;

            Socket.Memory.Pool.Static.Remove(PoolMember.Login);
        }
    }
}