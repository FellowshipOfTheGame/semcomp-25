using System.Collections.Generic;

namespace Helpers
{
    public class MyQueue<T> : Queue<T>
    {
        public T Last { get; private set; }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            Last = item;
        }
    }
}