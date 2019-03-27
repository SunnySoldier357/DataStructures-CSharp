using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTableClass
{
    public class HashTableArrayNode<TKey, TValue>
    {
        //* Private Properties

        /// <summary>
        /// This list contains the actual data in the hash table. It chains
        /// together data collisions.
        /// </summary>
        private LinkedList<HashTableNodePair<TKey, TValue>> _items;

        //* Public Properties

        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the key-value pairs in the list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public IEnumerable<HashTableNodePair<TKey, TValue>> Items
        {
            get
            {
                if (Items != null)
                {
                    foreach (var node in _items)
                        yield return node;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the keys in the list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                if (Items != null)
                {
                !foreach (var node in _items)
                        yield return node.Key;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Returns an enumerator for all of the values in the list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                if (Items != null)
                {
                    foreach (var node in _items)
                        yield return node.Value;
                }
            }
        }

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the key-value pair to the node, lazily initialising the linked
        /// list when adding the first value. If the key being added already exists,
        /// an <see cref="ArgumentException"/> is thrown.
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
            // Lazy init the linked list
            if (_items == null)
                _items = new LinkedList<HashTableNodePair<TKey, TValue>>();
            else
            {
                // Multiple items might collide & exist in this list, but each key
                // should only be in the list once.
                foreach (var pair in _items)
                {
                    if (pair.Key.Equals(key))
                        throw new ArgumentException("The collection already contains the key");
                }
            }

            // If we made it this far, add the item.
            _items.AddFirst(new HashTableNodePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// <para>
        /// Removes all the items from the linked list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            //!  Note: This implementation simply clears the linked list; however,
            //!        it would also be possible to assign the _items reference
            //!        to null and let the garbage collector reclaim the memory.
            //!        The next call to Add would allocate a new linked list.
            
            if (_items != null)
                _items.Clear();
        }

        /// <summary>
        /// <para>
        /// Removes the item from the list whose key matches the specified key.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of values in the linked list.
        /// In general this will be an O(1) algorithm because there will not be
        /// a collision.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the item is removed; <see langword="false"/>
        /// otherwise.
        /// </returns>
        public bool Remove(TKey key)
        {
            bool removed = false;

            if (_items != null)
            {
                var current = _items.First;

                while (current != null)
                {
                    if (current.Value.Key.Equals(key))
                    {
                        _items.Remove(current);
                        removed = true;

                        break;
                    }

                    current = current.Next;
                }
            }

            return removed;
        }

        /// <summary>
        /// <para>
        /// Finds and returns the value for the specified key.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of values in the linked list.
        /// In general this will be an O(1) algorithm because there will not be
        /// a collision.
        /// </para>
        /// </summary>
        /// <param name="key">The key whose value is sought.</param>
        /// <param name="value">The value associated with the specified key.</param>
    
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            bool found = false;

            if (_items != null)
            {
                foreach (var pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;
                        found = true;

                        break;
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// <para>
        /// Finds the key-value pair with the matching key and updates the
        /// associated value. If the key is not found, an
        /// <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// <para>
        /// Performance: O(n), where n is the number of values in the linked list.
        /// In general this will be an O(1) algorithm because there will not be
        /// a collision.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the item being updated.</param>
        /// <param name="value"The updated value.></param>
        /// <exception cref="ArgumentException"/>
        public void Update(TKey key, TValue value)
        {
            bool updated = false;

            if (_items != null)
            {
                // Check each item in the list for the specified key.
                foreach (var pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        // Update the value.
                        pair.Value = value;
                        updated = true;

                        break;
                    }
                }
            }

            if (!updated)
                throw new ArgumentException("The collection does not contain the key.");
        }
    }
}