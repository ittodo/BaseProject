using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class Work : ITask
    {
        private Action task;

        public ITask.EStatus Status
        {
            get;
            set;
        }

        public ITaskResult Result
        {
            get;
        }

        public Work(System.Action task)
        {
            this.task = task;
            this.Status = ITask.EStatus.Wait;
        }

        public void Run()
        {
            task();
        }
    }
    public class TaskObject<T> : ITask<T>
    {
        private Func<T> task;
        private T result;

        public ITask.EStatus Status
        {
            get;
            set;
        }

        public ITaskResult<T> Result
        {
            get;
            set;
        }

        ITaskResult ITask.Result
        {
            get;
        }

        public TaskObject(Func<T> task , T result)
        {
            this.task = task;
            this.Status = ITask.EStatus.Wait;
        }

        public void Run()
        {
            result = task();
        }
    }
}
