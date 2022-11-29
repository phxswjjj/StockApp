using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Utility
{
    internal class FixedSizeQueue<T>
    {
        ConcurrentQueue<T> Queues = new ConcurrentQueue<T>();
        public int MaxSize { get; }
        private readonly object LockObject = new object();
        public int Count => this.Queues.Count;

        public FixedSizeQueue(int maxSize)
        {
            this.MaxSize = maxSize;
        }

        public void Push(T obj)
        {
            this.Queues.Enqueue(obj);
            lock (this.LockObject)
            {
                while (this.Queues.Count > this.MaxSize
                    && this.Queues.TryDequeue(out T dq)) ;
            }
        }

        internal IEnumerable<DateTime> Latest(DateTime t)
        {
            foreach (var q in this.Queues.Cast<DateTime>())
            {
                if (q >= t)
                    yield return q;
            }
        }
    }
}
