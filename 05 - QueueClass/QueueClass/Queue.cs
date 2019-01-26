using System;
using DoublyLinkedList;

namespace QueueClass
{
    public class Queue<T>
    {
        // Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of items currently in the queue. Returns 0 if the
        /// queue is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count => _items.Count;

        // Private Properties
        private LinkedList<T> _items = new LinkedList<T>();

        // Public Methods

        /// <summary>
        /// <para>
        /// Adds an item to the end of the queue.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value) => _items.AddFirst(value);

        /// <summary>
        /// <para>
        /// Removes and returns the oldest item from the queue. An
        /// <see cref="InvalidOperationException"/> is thrown if the queue is 
        /// empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T Dequeue()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("The queue is empty");

            T last = _items.Tail.Value;
            _items.RemoveLast();

            return last;
        }

        /// <summary>
        /// <para>
        /// Returns the next item that would be returned if <see cref="Dequeue"/> were
        /// called. The queue is left unchanged. An <see cref="InvalidOperationException"/>
        /// is thrown if the queue is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T Peek()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("The stack is empty");

            return _items.Tail.Value;
        }
    }
}