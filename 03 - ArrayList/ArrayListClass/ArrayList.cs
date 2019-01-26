using System;
using System.Collections;
using System.Collections.Generic;

namespace ArrayListClass
{
    public class ArrayList<T> : IList<T>
    {
        // Public Properties

        /// <summary>
        /// <para>
        /// Returns <see langword="false"/> because the collection is not read-only.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// <para>
        /// Returns an <see cref="int"/> that indicates the number of items currently in 
        /// the collection. When the list is empty, the value is 0.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// <para>
        /// Gets or sets the value at the specified index.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index < Count)
                    return _items[index];

                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < Count)
                    _items[index] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        // Private Properties
        private T[] _items;

        // Constructors
        public ArrayList() : this(0) { }

        public ArrayList(int length)
        {
            if (length < 0)
                throw new ArgumentException("length");

            _items = new T[length];
        }

        // Public Methods

        /// <summary>
        /// <para>
        /// Appends the provided value to the end of the collection.
        /// </para>
        /// <para>
        /// Performance: O(1) when the array capacity is greater than Count; O(n) when
        /// growth is necessary.
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (_items.Length == Count)
                growArray();

            _items[Count++] = item;
        }

        /// <summary>
        /// <para>
        /// Removes all the items from the array list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            _items = new T[0];
            Count = 0;
        }

        /// <summary>
        /// <para>
        /// Returns <see langword="true"/> if the provided value exists in the collection.
        /// Otherwise it returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item) => IndexOf(item) != -1;

        /// <summary>
        /// <para>
        /// Copies the contents of the internal array from start to finish into the provided array
        /// starting at the specified array index.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex) => Array.Copy(_items, 0, array, arrayIndex, Count);

        /// <summary>
        /// <para>
        /// Returns the first index in the collection whose value equals the provided value.
        /// Returns -1 if no matching value is found.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// <para>
        /// Adds the provided value at the specified index in the collection. If the specified 
        /// index is equal to or larger than Count, an exception is thrown.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="IndexOutOfRangeException"/>
        public void Insert(int index, T item)
        {
            if (index >= Count)
                throw new IndexOutOfRangeException();

            if (_items.Length == Count)
                growArray();

            // Shift all the items following index one slot to the right.
            Array.Copy(_items, index, _items, index + 1, Count - index);

            _items[index] = item;

            Count++;
        }

        /// <summary>
        /// <para>
        /// Removes the first item in the collection whose value matches the provided value.
        /// Returns <see langword="true"/> if a value was removed. Otherwise it returns 
        /// <see langword="false"/>.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    RemoveAt(i);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// <para>
        /// Removes the value at the specified index.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (index >= Count)
                throw new IndexOutOfRangeException();

            int shiftStart = index + 1;
            if (shiftStart < Count)
            {
                // Shift al the items following index one slot to the left.
                Array.Copy(_items, shiftStart, _items, index, Count - shiftStart);
            }

            Count--;
        }

        /// <summary>
        /// <para>
        /// Returns an <see cref="IEnumerator{T}"/> instance that allows enumerating
        /// the array list values in order from first to last.
        /// </para>
        /// <para>
        /// Performance: Returning the enumerator instance is an O(1) operation. Enumerating
        /// every item is an O(n) operation.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return _items[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Private Methods
        private void growArray()
        {
            int newLength = _items.Length == 0 ? 16 : _items.Length << 1;

            T[] newArray = new T[newLength];

            _items.CopyTo(newArray, 0);
            _items = newArray;
        }
    }
}