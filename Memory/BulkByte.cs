using System;
using System.Collections.Generic;
using System.Text;

namespace Memory
{
    public class BulkByte
    {
        //     2^20 = 1,048,576 (0x100000) 1mb
        private const int bulkSize = 1048000;
        readonly public int itemSize;
        readonly public int maxPosition;

        List<Alloc> heap;

        Queue<long> free;

        Alloc current;
        
        public BulkByte(int itemSize)
        {
            this.itemSize = itemSize;
            this.maxPosition = bulkSize / itemSize - 1;
            this.heap = new List<Alloc>();
            this.heap.Capacity = 200;
            this.free = new Queue<long>();
        }


        public (Memory<byte>,Handle) New()
        {
            long item = 0 ;
            var handle = new Handle();
            if (free.TryDequeue(out item) == true)
            {
                int heapindex = (int)(item / maxPosition);
                int index = (int)(item % maxPosition);
                
                handle.set(heapindex, index);
                return (heap[heapindex][index], handle);
            }

            if(current == null)
            {
                current = new Alloc(itemSize);
                heap.Add(current);
            }
            
            if(current.IsNew() == false)
            {
                current = new Alloc(itemSize);
                heap.Add(current);
            }

            var m = current.New();
            handle.set(heap.Count -1, current.position - 1);

            return (m, handle);
        }

        public void Release(Handle h)
        {
            var freeindex = h.heapindex * maxPosition + h.index;

            free.Enqueue(freeindex);
        }

        public struct Handle
        {
            public int heapindex { get; private set; }
            public int index { get; private set; }

            public void set(int heapindex , int index)
            {
                this.heapindex = heapindex;
                this.index = index;
            }
        }


        class Alloc
        {
            private byte[] arr = new byte[bulkSize];
            public int position = 0;
            readonly public int itemSize;
            readonly public int maxPosition;

            public Alloc(int itemSize)
            {
                this.itemSize = itemSize;
                this.maxPosition = bulkSize / itemSize - 1;
            }

            public Memory<byte> this[int index]
            {
                get
                {
                    return new Memory<byte>(arr, itemSize * index, itemSize);
                }
            }

            public bool IsNew()
            {
                if (maxPosition < position)
                {
                    return false;
                }
                return true;
            }

            public Memory<byte> New()
            {
                if(maxPosition < position)
                {
                    return null;
                }
                var memory = new Memory<byte>(arr, itemSize * position, itemSize);
                position++;
                return memory;
            }
        }

    }
}
