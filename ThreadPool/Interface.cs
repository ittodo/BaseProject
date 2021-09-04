using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public interface ITask
    {
        public enum EStatus { Wait , Run , Complete };
        public void Run();

        
        public EStatus Status { get; }

        public ITaskResult Result { get; }
    }

    

    public interface ITask<T> : ITask
    {
        public new ITaskResult<T> Result { get; }
    }


    public interface ITaskResult
    {
        public ITask.EStatus Status { get; }
    }

    public interface ITaskResult<T>
    {
        public ITask.EStatus Status { get; }
        public T Result { get; }
    }
}
