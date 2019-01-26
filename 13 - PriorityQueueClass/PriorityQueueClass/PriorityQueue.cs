using System;
using HeapClass;

namespace PriorityQueueClass
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        //* Private Properties
        private Heap<T> _heap;

        //* Public Properties
        public int Count => _heap.Count;

        //* Constructors
        public PriorityQueue() =>
            _heap = new Heap<T>();

        //* Public Methods

        public void Clear() =>
            _heap.Clear();

        public void Enqueue(T value) =>
            _heap.Add(value);

        public T Dequeue() =>
            _heap.RemoveMax();
    }
}