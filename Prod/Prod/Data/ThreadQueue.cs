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
    /// <summary>
    /// A threadsafe queue for passing work objects around.
    /// To prevent race conditions, ensure that only a single thread 
    /// pushes data, and that a separate, single thread takes.
    /// </summary>
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

        /// <summary>
        /// If there is data available, this will return a filled Opion 
        /// instance. Otherwise, None is returned.
        /// 
        /// This function can be used in a loop such as
        /// <code>
        /// Option<SomeType> next = queue.TryTake();
        /// while(next.hasValue) 
        /// {
        ///     //use next
        ///     next = queue.TryTake();
        /// }
        /// </code>
        /// 
        /// </summary>
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
