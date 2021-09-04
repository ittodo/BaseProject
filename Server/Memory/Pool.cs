using System;
using System.Collections.Concurrent;

namespace Socket.Memory
{
    public class Pool
    {
        private static Pool _static;
        public static Pool Static
        {
            get
            {
                return _static = _static ?? new Pool();
            }
        }
        ConcurrentDictionary<int, ConcurrentQueue<object>> unUsedMemory = new ConcurrentDictionary<int, ConcurrentQueue<object>>();

        public void CreateOrAddPool<T>(int Count = 0) where T : class, IPoolObject, new()
        {
            var hashcode = typeof(T).GetHashCode();
            ConcurrentQueue<object> queue = null;
            queue = unUsedMemory.GetOrAdd(hashcode, (x) => { return new ConcurrentQueue<object>(); });

            for(int i = 0; i <Count; i++)
            {
                var newItem = new T();
                queue.Enqueue(newItem);
            }
        }

        public T Create<T>() where T : class, IPoolObject, new()
        {
            var hashcode = typeof(T).GetHashCode();
            ConcurrentQueue<object> value = null;
            unUsedMemory.TryGetValue(hashcode, out value);
            if (value != null)
            {
                object item = null;
                value.TryDequeue(out item);
                if (item == null)
                {
                    var newItem = new T();
                    newItem.InitInstance();
                    newItem.IsUsed = true;
                    newItem.Use();
                    
                    item = newItem;
                    //Console.WriteLine("Create");
                }
                return item as T;
            }
            return null;
        }

        public void Remove(IPoolObject removeItem)
        {
            if (removeItem == null)
            {
                return;
            }

            removeItem.IsUsed = false;
            removeItem.Clear();
            object obj = removeItem as object;
            
            var hashcode = removeItem.GetHashCode();
            

            ConcurrentQueue<object> value = null;
            unUsedMemory.TryGetValue(hashcode, out value);
            if (value != null)
            {
                value.Enqueue(obj);
                //Console.WriteLine("push");
            }
            else
            {
                //Console.WriteLine("push!!!---~~");
            }
        }

        public void RemoveAll()
        {
            var itr = unUsedMemory.GetEnumerator();
            while(itr.MoveNext())
            {
                var item = itr.Current;
                var dict = item.Value;
                dict.Clear();
            }
            unUsedMemory.Clear();
            unUsedMemory = null;
        }

    }
}
