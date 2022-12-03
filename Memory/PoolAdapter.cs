using System;
using System.Collections.Generic;
using System.Text;

namespace Memory
{
    public abstract class PoolAdapter<T> : IPoolObject where T : new()
    {
        public static Action<T> initInstance;
        public static Action<T> clear;
        public static Action<T> use;

        public bool IsUsed { get; set; }

        private T origin;

        public T Value { get { return origin; } }

        public PoolAdapter()
        {
            origin = new T();
        }

        public void InitInstance()
        {
            if(initInstance != null)
            {
                initInstance(origin);
            }
        }

        public void Clear()
        {
            if(clear != null)
            {
                clear(origin);
            }
        }

        public void Use()
        {
            if(use != null)
            {
                use(origin);
            }
        }
    }

}
