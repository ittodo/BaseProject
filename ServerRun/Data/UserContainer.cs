using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace ServerRun.Data
{
    public class UserContainer 
    {
        ConcurrentDictionary<Socket.Connection.Data.SocketAdapter, User.Instance> Container;
        
        public UserContainer()
        {
            Container = new ConcurrentDictionary<Socket.Connection.Data.SocketAdapter, User.Instance>();
        }

        public User.Instance Add(Socket.Connection.Data.SocketAdapter adapter)
        {
            var user = new User.Instance(adapter);
            if(Container.TryAdd(adapter, user) == true)
            {
                return user;
            }
            return null;
        }

        public void Remove(Socket.Connection.Data.SocketAdapter adapter)
        {
            User.Instance user = null;
            Container.TryRemove(adapter ,out user);
        }
    }
}