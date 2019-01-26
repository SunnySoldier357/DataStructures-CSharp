using System;

namespace DequeUsingArrays
{
    public class Deque<T>
    {
        // Private Properties

        // The index of the first (oldest) item in the queue.
        private int _head = 0;

        // The number of items in the queue.
        private int _size = 0;

        // The index of the last (newest) item in the queue.
        private int _tail = -1;

        private T[] _items;

        // Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of items currently in the deque or 0 if the deque
        /// is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count => _size;

        // Public Methods

        /// <summary>
        /// <para>
        /// Adds the provided value to the head of the queue. This will be the next
        /// item dequeued by <see cref="DequeueFirst"/>.
        /// </para>
        /// <para>
        /// Performance: O(1) in most cases; O(n) when growth is necessary.
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueFirst(T item)
        {
            // If the array needs to grow.
            if (_items.Length == _size)
                allocateNewArray(1);

            // Since we know the array isn't full and _head is greater than 0, we know
            // that the slot in front of head is open.
            if (_head > 0)
                _head--;
            else
            {
                // Otherwise we need to wrap around to the end of the array.
                _head = _items.Length - 1;
            }

            _items[_head] = item;
            _size++;
        }

        /// <summary>
        /// <para>
        /// Adds the provided value to the tail of the queue. This will be the next
        /// item dequeued by <see cref="DequeueLast"/>.
        /// </para>
        /// <para>
        /// Performance: O(1) in most cases; O(n) when growth is necessary.
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueLast(T item)
        {
            // If the array needs to grow.
            if (_items.Length == _size)
                allocateNewArray(0);

            // Now we have a properly sized array and can focus on wrapping issues.
            // If _tail is at the end of the array we need to wrap around.
            if (_tail == _items.Length - 1)
                _tail = 0;
            else
                _tail++;

            _items[_tail] = item;
            _size++;
        }

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
            if (_size == 0)
                throw new InvalidOperationException("The deque is empty.");

            T value = _items[_head];

            if (_head == _items.Length - 1)
            {
                // If the head is at the last index in the array, wrap it around.
                _head = 0;
            }
            else
            {
                // Moce to the next slot.
                _head++;
            }

            _size--;

            return value;
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
            if (_size == 0)
                throw new InvalidOperationException("The deque is empty");
            
            T value = _items[_tail];

            if (_tail == 0)
            {
                // If the tail is at the first index in the array, wrap it around.
                _tail = _items.Length - 1;
            }
            else
            {
                // Move to the previous slot.
                _tail--;
            }

            _size--;

            return value;
        }

        /// <summary>
        /// <para>
        /// Returns the first item in the deque but leaves the collection unchanged.
        /// An <see cref="InvalidOperationException"/> is thrown if the deque is
        /// empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T PeekFirst()
        {
            if (_size == 0)
                throw new InvalidOperationException("The deque is empty");

            return _items[_head];
        }

        /// <summary>
        /// <para>
        /// Returns the last item in the deque but leaves the collection unchanged.
        /// An <see cref="InvalidOperationException"/> is thrown if the deque is
        /// empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T PeekLast()
        {
            if (_size == 0)
                throw new InvalidOperationException("The deque is empty");

            return _items[_tail];
        }

        // Private Methods
        private void allocateNewArray(int startingIndex)
        {
            int newLength = _size == 0 ? 4 : _size * 2;

            T[] newArray = new T[newLength];

            if (_size > 0)
            {
                int targetIndex = startingIndex;

                // Copy the contents...
                // If the array has no wrapping, just copy the valid range.
                // Else, copy from head to end of the array and then from 0 to the tail.
                
                // If tail is less than head, we've wrapped.
                if (_tail < _head)
                {
                    // Copy the _items[head].._items[end] -> newArray[0]..newArray[N].
                    for (int index = _head; index < _items.Length; index++)
                    {
                        newArray[targetIndex] = _items[index];
                        targetIndex++;
                    }

                    // Copy _items[0]..items[tail] -> newArray[N + 1]
                    for (int index = 0; index <= _tail; index++)
                    {
                        newArray[targetIndex] = _items[index];
                        targetIndex++;
                    }
                }
                else
                {
                    // Copy the _items[head]..items[tail] -> newArray[0]..newArray[N]
                    for (int index = _head; index <= _tail; index++)
                    {
                        newArray[targetIndex] = _items[index];
                        targetIndex++;
                    }
                }

                _head = startingIndex;
                _tail = targetIndex - 1; // Compensate for the extra bump.
            }
            else
            {
                // Nothing in the array.
                _head = 0;
                _tail = -1;
            }

            _items = newArray;
        }
    }
}