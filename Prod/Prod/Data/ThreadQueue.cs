#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prod.Data
{
    class ThreadQueue<T>
    {
        private object lockObject = new object();
        private Queue<T> queue; 
        public ThreadQueue()
        {
            this.queue = new Queue<T>();
        }

        public void Add(T item)
        {
            lock(lockObject)
            {
                queue.Enqueue(item);
            }
        }

        public Option<T> TryTake()
        {
            Option<T> item = Option<T>.None;
            lock (lockObject)
            {
                if (queue.Count > 0)
                {
                    item = new Option<T>(queue.Dequeue());
                }
            }
            return item;
        }
    }
}
