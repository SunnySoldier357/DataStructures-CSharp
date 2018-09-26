using System;
using DoublyLinkedList;

namespace DequeClass
{
    public class Deque<T>
    {
        // Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of items currently in the deque, or 0 if the deque is empty.
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
        /// Adds the provided value to the head of the queue. This will be the next 
        /// item dequeued by <see cref="DequeueFirst"/>.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void EnqueueFirst(T value) => _items.AddFirst(value);

        /// <summary>
        /// <para>
        /// Adds the provided value to the tail of the queue. This will be the next
        /// item dequeued by <see cref="DequeueLast"/>.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void EnqueueLast(T value) => _items.AddLast(value);

        /// <summary>
        /// <para>
        /// Removes and returns the first item in the deque. An 
        /// <see cref="InvalidOperationException"/> is thrown if the deque is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T DequeueFirst()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("DequeueFirst called when deque is empty");

            T temp = _items.Head.Value;
            _items.RemoveFirst();

            return temp;
        }

        /// <summary>
        /// <para>
        /// Removes and returns the last item in the deque. An 
        /// <see cref="InvalidOperationException"/> is thrown if the deque is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T DequeueLast()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("DequeueLast called when deque is empty");

            T temp = _items.Tail.Value;
            _items.RemoveLast();

            return temp;
        }

        /// <summary>
        /// <para>
        /// Returns the first item in the deque but leaves the collection unchanged. An
        /// <see cref="InvalidOperationException"/> is thrown if the deque is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T PeekFirst()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("PeekFirst called when deque is empty");

            return _items.Head.Value;
        }


        /// <summary>
        /// <para>
        /// Returns the last item in the deque but leaves the collection unchanged. An
        /// <see cref="InvalidOperationException"/> is thrown if the deque is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T PeekLast()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("PeekLast called when deque is empty");

            return _items.Tail.Value;
        }
    }
}