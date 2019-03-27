using System;
using System.Collections;
using System.Collections.Generic;

namespace SkipListClass
{
    public class SkipList<T> : ICollection<T>
        where T : IComparable<T>
    {
        //* Private Properties

        /// <summary>
        /// The number of items currently in the list.
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// There is always one level of depth (the base list).
        /// </summary>
        private int _levels = 1;

        /// <summary>
        /// Used to determine the random height of the node links.
        /// </summary>
        private readonly Random _rand = new Random();

        /// <summary>
        /// The non-data node which starts the list.
        /// </summary>
        private SkipListNode<T> _head;

        //* Public Properties

        /// <summary>
        /// <para>
        /// Returns a value indicating if the skip list is read only.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <value></value>
        public bool IsReadOnly => false;

        /// <summary>
        /// <para>
        /// Returns the current number of items in the skip list (0 if empty).
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count => _count;

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the specific value to the skip list.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            int level = pickRandomLevel();

            SkipListNode<T> newNode = new SkipListNode<T>(item, level + 1);
            SkipListNode<T> current = _head;

            for (int i = _levels - 1; i >= 0; i--)
            {
                while (current.Next[i] != null)
                {
                    if (current.Next[i].Value.CompareTo(item) > 0)
                        break;

                    current = current.Next[i];
                }

                if (i <= level)
                {
                    // Adding "c" to the list: a -> b -> d -> e.
                    // Current is node b and current.Next[i] is d.

                    // 1. Link the new node (c) to the existing node (d):
                    // c.Next = d
                    newNode.Next[i] = current.Next[i];

                    // Insert c into the list after b:
                    // b.Next = c
                    current.Next[i] = newNode;
                }
            }

            _count++;
        }

        /// <summary>
        /// <para>
        /// Removes all the entries in the list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            _head = new SkipListNode<T>(default(T), 32 + 1);
            _count = 0;
        }

        /// <summary>
        /// <para>
        /// Copies the contents of the skip list into the provided array starting
        /// at the specified array index.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <exception cref="ArgumentNullException"/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            int offset = 0;
            foreach (T item in this)
                array[arrayIndex + offset++] = item;
        }

        /// <summary>
        /// <para>
        /// Returns <see langword="true"/> if the value being sought exists in
        /// the skip list.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            SkipListNode<T> current = _head;

            for (int level = _levels; level >= 0; level--)
            {
                while (current.Next[level] != null)
                {
                    int comparison = current.Next[level].Value.CompareTo(item);

                    if (comparison > 0)
                    {
                        // The value is too large, so go down one level and take
                        // smaller steps.
                        break;
                    }

                    if (comparison == 0)
                    {
                        // Found it!
                        return true;
                    }

                    current = current.Next[level];
                }
            }

            return false;
        }

        /// <summary>
        /// <para>
        /// Removes the first node with the indicated value from the skip list.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            SkipListNode<T> current = _head;

            bool removed = false;

            // Walk down each level in the list (make big jumps).
            for (int level = _levels - 1; level >= 0; level--)
            {
                // While we're not at the end of the list:
                while (current.Next[level] != null)
                {
                    int comparison = current.Next[level].Value.CompareTo(item);

                    // If we found our node,
                    if (comparison == 0)
                    {
                        // remove the node,
                        current.Next[level] = current.Next[level].Next[level];
                        removed = true;

                        // and go down to the next level (where we will find our
                        // node again if we're not at the bottom level).
                        break;
                    }

                    // If we went too far, go down a level.
                    if (comparison > 0)
                        break;

                    current = current.Next[level];
                }
            }

            if (removed)
                _count--;

            return removed;
        }

        /// <summary>
        /// <para>
        /// Returns an <see cref="IEnumerator{T}"/> instance that can be used to
        /// enumerate the items in the skip list in sorted order.
        /// </para>
        /// <para>
        /// Performance: O(1) to return the enumerator; O(n) to perform
        /// enumeration (caller cost).
        /// </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            SkipListNode<T> current = _head.Next[0];
            while (current != null)
            {
                yield return current.Value;
                current = current.Next[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        //* Private Methods
        private int pickRandomLevel()
        {
            int rand = _rand.Next();
            int level = 0;

            // We're using the bit mask of a random integer to determine if the
            // max level should increase or not.
            // Say the 8 LSBs of the int are 00101100. In that case, when the
            // LSB is compared against 1, it tests to 0 and the while loop is
            // never entered so the level stays the same. That should happen 1/2
            // of the time. Later, if the _levels field is set to 3 and the rand
            // value is 01101111, the while loop will run 4 times and on the last
            // iteration will run another 4 times, creating a node with a skip
            // list height of 4. This should only happen 1/16 of the time.
            while ((rand & 1) == 1)
            {
                if (level == _levels)
                {
                    _levels++;
                    break;
                }

                rand >>= 1;
                level++;
            }

            return level;
        }
    }
}