using System;
using System.Threading;

namespace ThreadPool
{
    public class Manager : IDisposable
    {
        System.Collections.Concurrent.ConcurrentDictionary<int, Worker> threads;

        System.Collections.Concurrent.ConcurrentQueue<Worker> stop;

        System.Collections.Concurrent.ConcurrentDictionary<int, Worker> start;

        System.Collections.Concurrent.ConcurrentQueue<Work> remainWork;

        System.Threading.ManualResetEvent resetevent;

        public static Manager Create(int threadCount = 4)
        {
            //Environment.ProcessorCount;
            return new Manager(threadCount);
        }

        private Manager(int threadCount)
        {
            var count = threadCount;

            resetevent = new ManualResetEvent(false);

            threads = new System.Collections.Concurrent.ConcurrentDictionary<int, Worker>();

            stop = new System.Collections.Concurrent.ConcurrentQueue<Worker>();

            start = new System.Collections.Concurrent.ConcurrentDictionary<int, Worker>();

            remainWork = new System.Collections.Concurrent.ConcurrentQueue<Work>();
            for (int i = 0; i < count; i++)
            {
                var thread = new Worker(i, this);
                threads[i] = thread;
                stop.Enqueue(thread);
            }
        }

        public ITask Run(Action action)
        {
            var item = new Work(action);
            remainWork.Enqueue(item);
            Worker w;
            if (stop.TryDequeue(out w))
            {
                if(remainWork.TryDequeue(out item) == true)
                {
                    w.SetTask(item);
                    start[w.index] = w;
                }
                else
                {
                    stop.Enqueue(w);
                }
            }
            
            return item;
        }

        public bool NextWork(Worker w)
        {
            Work item = null;
            if (remainWork.TryDequeue(out item))
            {
                w.SetTaskNoneWait(item);
                return true;
            }

            if (start.TryRemove(w.index, out w) == true)
            {
                stop.Enqueue(w);
            }
            else
            {
                System.Diagnostics.Debug.Assert(true);
            }

            return false;
        }

        public void Wait()
        {
            while (true)
            {
                if (remainWork.Count == 0 && start.Count == 0)
                {
                    return;
                }
                Thread.Sleep(0);
            }
        }

        public void Dispose()
        {

        }
    }

}
