using System.Collections;
using System.Collections.Generic;

namespace TripPlannerLogic
{
    public abstract class CustomContainer<T> : IEnumerable<T>
    {
        protected List<T> _container;
        protected int _size;
        public T this[int index]
        {
            get
            {
                return _container[index];
            }
        }
        public bool Contains(T item)
        {
            return _container.Contains(item);
        }
        public int Count
        {
            get
            {
                return _container.Count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public abstract void Add(T item);

    }
}
