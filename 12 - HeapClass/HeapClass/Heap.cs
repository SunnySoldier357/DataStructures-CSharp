using System;

namespace HeapClass
{
    public class Heap<T> where T : IComparable<T>
    {
        //* Constants
        const int DEFAULT_LENGTH = 100;

        //* Private Properties
        private T[] _items;

        //* Public Properties
        public int Count { get; private set; }

        //* Constructors
        public Heap() : this(DEFAULT_LENGTH) { }

        public Heap(int length)
        {
            Count = 0;
            _items = new T[length];
        }

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the provided value to the heap.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            if (Count >= _items.Length)
                growBackingArray();

            _items[Count] = value;

            int index = Count;

            while (index > 0 && _items[index].CompareTo(_items[parent(index)]) > 0)
            {
                swap(index, parent(index));
                index = parent(index);
            }

            Count++;
        }

        /// <summary>
        /// <para>
        /// Removes all the items from the heap.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            Count = 0;
            _items = new T[DEFAULT_LENGTH];
        }

        /// <summary>
        /// <para>
        /// Returns the maximum value in the heap or throws an exception if the
        /// heap us empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return _items[0];
        }

        /// <summary>
        /// <para>
        /// Removes and returns the largest value in the heap. An exception is
        /// thrown if the heap is empty.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T RemoveMax()
        {
            if (Count <= 0)
                throw new InvalidOperationException();

            T max = _items[0];

            _items[0] = _items[Count - 1];
            Count--;

            int index = 0;

            while (index < Count)
            {
                // Get the left and right child indexes.
                int left = (2 * index) + 1;
                int right = (2 * index) + 2;

                // Make sure we are still within the heap.
                if (left >= Count)
                    break;

                // To avoid having to swap twice, we swap with the largest value.
                // E.g.,
                //      5
                //    6   8
                //
                // If we swapped with 6 first we'd have
                //
                //      6
                //    5   8
                //
                // and we'd require another swap to get the desired tree.
                //
                //      8
                //    6   5
                //
                // So we find the largest child and just do the right thing at
                // the start.
                int maxChildIndex = indexOfMaxChild(left, right);

                if (_items[index].CompareTo(_items[maxChildIndex]) > 0)
                {
                    // The current item is larger than its children (heap property
                    // is satisfied).
                    break;
                }

                swap(index, maxChildIndex);
                index = maxChildIndex;
            }

            return max;
        }

        // Private Properties
        private void growBackingArray()
        {
            T[] newItems = new T[_items.Length * 2];
            for (int i = 0; i < _items.Length; i++)
                newItems[i] = _items[i];

            _items = newItems;
        }

        private int indexOfMaxChild(int left, int right)
        {
            // Find the index of the child with the largest value.
            int maxChildIndex = -1;
            if (right >= Count)
            {
                // No right child.
                maxChildIndex = left;
            }
            else
            {
                if (_items[left].CompareTo(_items[right]) > 0)
                    maxChildIndex = left;
                else
                    maxChildIndex = right;
            }

            return maxChildIndex;
        }

        private int parent(int index) =>
            (index - 1) / 2;

        private void swap(int left, int right)
        {
            T temp = _items[left];
            _items[left] = _items[right];
            _items[right] = temp;
        }
    }
}