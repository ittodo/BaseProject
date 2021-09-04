using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class Worker
    {
        public bool IsRun
        {
            get
            {
                return lockcount != 0;
            }
        }

        public int lockcount;

        public int index;

        System.Threading.Thread thread;

        System.Threading.AutoResetEvent resetevent;

        ITask task;

        Manager manager;

        public Worker(int index, Manager manager)
        {
            this.index = index;
            this.manager = manager;
            this.lockcount = 0;
            this.resetevent = new System.Threading.AutoResetEvent(false);
            this.thread = new System.Threading.Thread(Run);
            this.thread.Start();
        }

        public void SetTask(ITask obj)
        {
            System.Diagnostics.Debug.Assert(IsRun == false);

            task = obj;

            System.Threading.Interlocked.Increment(ref lockcount);

            resetevent.Set();
        }

        public void SetTaskNoneWait(ITask obj)
        {
            System.Diagnostics.Debug.Assert(IsRun == false);

            task = obj;

            System.Threading.Interlocked.Increment(ref lockcount);
        }

        private void Run(object obj)
        {
            while (true)
            {
                Console.WriteLine($"waitOne Thread : {index} ");
                resetevent.WaitOne();

                Console.WriteLine($"start Thread : {index} ");

                task.Run();
                task = null;
                System.Threading.Interlocked.Decrement(ref lockcount);

                while (manager.NextWork(this))
                {
                    task.Run();
                    System.Threading.Interlocked.Decrement(ref lockcount);

                    Console.WriteLine($"nextWork : {index} ");
                }
                Console.WriteLine($"stop Thread : {index} ");
            }
        }


    }
}
