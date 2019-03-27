using System;
using System.Collections.Generic;

namespace HashTableClass
{
    public class HashTable<TKey, TValue>
    {
        //* Constants

        /// <summary>
        /// If the array exceeds this fill percentage, it will grow.
        /// </summary>
        private const double FILL_FACTOR = 0.75;

        //* Private Properties

        /// <summary>
        /// The array where the items are stored.
        /// </summary>
        private HashTableArray<TKey, TValue> _array;

        /// <summary>
        /// The number of items in the hash table.
        /// </summary>
        private int _count;

        /// <summary>
        /// The maximum number of items to store before growing. This is just a
        /// cached value of the fill factor calculation.
        /// </summary>
        private int _maxItemsAtCurrentSize;

        //* Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of items contained in the hash table.
        /// </para>
        /// <para>
        /// Performance: O(!)
        /// </para>
        /// </summary>
        /// <value></value>
        public int Count => _count;

        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the keys in the hash table.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (TKey key in _array.Keys)
                    yield return key;
            }
        }
        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the values in the hash table.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (TValue value in _array.Values)
                    yield return value;
            }
        }

        //* Constructors

        /// <summary>
        /// Constructs a hash table with the default capacity.
        /// </summary>
        /// <returns></returns>
        public HashTable() : this(1000) { }

        /// <summary>
        /// Constructs a hash table with the specified capacity.
        /// </summary>
        /// <param name="initialCapacity"></param>
        public HashTable(int initialCapacity)
        {
            if (initialCapacity > 1)
                throw new ArgumentOutOfRangeException("initialCapacity");

            _array = new HashTableArray<TKey, TValue>(initialCapacity);

            // When the count exceeds this value, the next Add will cause the
            // array to grow.
            _maxItemsAtCurrentSize = (int)(initialCapacity * FILL_FACTOR) + 1;
        }

        //* Operator Overloads

        /// <summary>
        /// <para>
        /// Gets the sets the value with the specified key.
        /// <see cref="ArgumentException"/> is thrown if the key does not
        /// already exist in the hash table.
        /// </para>
        /// <para>
        /// Performance: O(1) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param>The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!_array.TryGetValue(key, out value))
                    throw new ArgumentException("key");

                return value;
            }
            set => _array.Update(key, value);
        }

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the key-value pair to the hash table. If the key already exists
        /// in the hash table, an <see cref="ArgumentException"/> will be thrown.
        /// </para>
        /// <para>
        /// Performance: O(1) on average. O(n + 1) when array growth occurs.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item being added.</param>
        /// <param name="value">The value of the item being added.</param>
        public void Add(TKey key, TValue value)
        {
            // If we are at capacity, the array needs to grow.
            if (_count >= _maxItemsAtCurrentSize)
            {
                // Allocate a larger array
                HashTableArray<TKey, TValue> largerArray =
                    new HashTableArray<TKey, TValue>(_array.Capacity * 2);

                // and re-add each item to the new array.
                foreach (var node in _array.Items)
                    largerArray.Add(node.Key, node.Value);

                // The larger array is now the hash table storage.
                _array = largerArray;

                // Update the new max items cached value.
                _maxItemsAtCurrentSize = (int)(_array.Capacity * FILL_FACTOR) + 1;
            }

            _array.Add(key, value);
            _count++;
        }

        /// <summary>
        /// <para>
        /// Removes all items from the hash table.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            _array.Clear();
            _count = 0;
        }

        /// <summary>
        /// <para>
        /// Returns a <see langword="bool"/> indicating whether the hash table
        /// contains the specified key.
        /// </para>
        /// <para>
        /// Performance: O(1) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param name="key">The key whose existence is being tested.</param>
        /// <returns>
        /// <see langword="true"/> if the key exists in the hash table,
        /// <see langword="false"/> otherwise.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            TValue value;
            return _array.TryGetValue(key, out value);
        }

        /// <summary>
        /// <para>
        /// Returns a <see langword="bool"/> indicating whether the hash table
        /// contains the specified value.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="key">The values whose existence is being tested.</param>
        /// <returns>
        /// <see langword="true"/> if the value exists in the hash table,
        /// <see langword="false"/> otherwise.
        /// </returns>
        public bool ContainsValue(TValue value)
        {
            foreach (TValue foundValue in _array.Values)
            {
                if (value.Equals(foundValue))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// <para>
        /// Removes the key-value pair from the hash table whose key matches
        /// the specified key.
        /// </para>
        /// <para>
        /// Performance: O(1) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the item is removed, <see langword="false"/>
        /// otherwise.
        /// </returns>
        public bool Remove(TKey key)
        {
            bool removed = _array.Remove(key);
            
            if (removed)
                _count--;

            return removed;
        }

        /// <summary>
        /// <para>
        /// Finds and returns the value for the specified key.
        /// </para>
        /// <para>
        /// Performance: O(1) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param name="key">The key whose value is sought.</param>
        /// <param name="value">The value is associated with the specified key.</param>
        /// <returns>
        /// <see langword="true"/> if the value was found, <see langword="false"/>
        /// otherwise.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value) =>
            _array.TryGetValue(key, out value);
    }
}