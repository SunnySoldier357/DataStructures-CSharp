using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTableClass
{
    public class HashTableArray<TKey, TValue>
    {
        // Private Properties
        private HashTableArrayNode<TKey, TValue>[] _array;

        // Public Properties

        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the Items in the node array.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the total number of Items contained in
        /// the hash table array and all of its contained nodes.
        /// </para>
        /// </summary>
        public IEnumerable<HashTableNodePair<TKey, TValue>> Items
        {
            get
            {
                foreach (var node in _array.Where(node => node != null))
                {
                    foreach (var pair in node.Items)
                        yield return pair;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the keys in the node array.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the total number of items contained in
        /// the hash table array and all of its contained nodes.
        /// </para>
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var node in _array.Where(node => node != null))
                {
                    foreach (TKey key in node.Keys)
                        yield return key;
                }
            }
        }
        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the values in the node array.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the total number of items contained in
        /// the hash table array and all of its contained nodes.
        /// </para>
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var node in _array.Where(node => node != null))
                {
                    foreach (TValue value in node.Values)
                        yield return value;
                }
            }
        }

        /// <summary>
        /// <para>
        /// The capacity of the hash table array.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <value></value>
        public int Capacity => _array.Length;

        //* Note: It is important to remember that the capacity of the hash
        //*       table array is not the same as the hash table's item count.

        // Constructors

        /// <summary>
        /// Constructs a new hash table array with the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the array.</param>
        public HashTableArray(int capacity) =>
            _array = new HashTableArrayNode<TKey, TValue>[capacity];

        // Public Methods

        /// <summary>
        /// <para>
        /// Adds the key-value pair to the node array. If the key already exists
        /// in the node array, an <see cref="ArgumentException"/> will be thrown.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item being added.</param>
        /// <param name="value">The value of the item being added.</param>
        /// <exception cref="ArgumentException"/>
        public void Add(TKey key, TValue value)
        {
            int index = getIndex(key);
            HashTableArrayNode<TKey, TValue> nodes = _array[index];

            if (nodes == null)
            {
                nodes = new HashTableArrayNode<TKey, TValue>();
                _array[index] = nodes;
            }

            nodes.Add(key, value);
        }

        /// <summary>
        /// <para>
        /// Removes every item from the hash table array.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of nodes in the table that
        /// contain data.
        /// </para>
        /// </summary>
        public void Clear()
        {
            foreach (var node in _array.Where(node => node != null))
                node.Clear();
        }

        /// <summary>
        /// <para>
        /// Removes the item from the node array whose keys matches the specified
        /// key.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of items stored in the
        /// HashTableNodeArray instance. This will typically be a O(1) operation.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item to remove</param> <summary>
        /// <returns>
        /// <see langword="true"/> if the value was found, <see langword="false"/>
        /// otherwise.
        /// </returns>
        public bool Remove(TKey key)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[getIndex(key)];

            if (nodes != null)
                return nodes.Remove(key);

            return false;
        }

        /// <summary>
        /// <para>
        /// Finds and returns the value for the specified key.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of items stored in the
        /// HashTableNodeArray instance. This will typically be a O(1) operation.
        /// </para>
        /// </summary>
        /// <param name="key">The key whose value is sought.</param>
        /// <param name="value">The value associated with the specified key.</param>
        /// <returns>
        /// <see langword="true"/> if the value was found, <see langword="false"/>
        /// otherwise.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[getIndex(key)];

            if (nodes != null)
                return nodes.TryGetValue(key, out value);

            value = default(TValue);
            return false;
        }

        /// <summary>
        /// <para>
        /// Updates the value of the key-value pair whose key matches the
        /// provided key. If the key does not exist, an
        /// <see cref="ArgumentException"/> will be thrown.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of items stored in the
        /// HashTableNodeArray instance. This will typically be a O(1) operation.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item being updated.</param>
        /// <param name="value">The updated value.</param>
        /// <exception cref="ArgumentException"/>
        public void Update(TKey key, TValue value)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[getIndex(key)];

            if (nodes == null)
                throw new ArgumentException("The key does not exist in the hash table",
                    "key");

            nodes.Update(key, value);
        }

        // Private Properties

        /// <summary>
        /// <para>
        /// Maps a key to the array index based on the hash code.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int getIndex(TKey key)
            => Math.Abs(key.GetHashCode() % Capacity);
    }
}