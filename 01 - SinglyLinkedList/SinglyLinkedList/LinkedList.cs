using System.Collections;
using System.Collections.Generic;

namespace SinglyLinkedList
{
    public class LinkedList<T> : ICollection<T>
    {
        // Public Properties

        /// <summary>
        /// <para>
        /// Returns <see langword="false"/> if the list is not read-only.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// <para>
        /// Returns an <see cref="int"/> indicating the number of items currently in the list.
        /// When the list is empty, the value returned is 0.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count { get; private set; }

        public LinkedListNode<T> Head => _head;
        public LinkedListNode<T> Tail => _tail;

        // Private Properties
        private LinkedListNode<T> _head;
        private LinkedListNode<T> _tail;

        // Public Methods

        /// <summary>
        /// <para>
        /// Adds the provided value to the end of the linked list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Add(T value)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(value);

            if (_head == null)
            {
                _head = node;
                _tail = node;
            }
            else
            {
                _tail.Next = node;
                _tail = node;
            }

            Count++;
        }

        /// <summary>
        /// <para>
        /// Removes all the items from the list.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            _head = null;
            _tail = null;

            Count = 0;
        }

        /// <summary>
        /// <para>
        /// Returns a <see cref="bool"/> that indicates whether the provided value exists within the
        /// linked list.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            LinkedListNode<T> current = _head;

            while (current != null)
            {
                if (current.Value.Equals(item))
                    return true;

                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// <para>
        /// Copies the contents of the linked list from start to finish into the provided
        /// array, starting at the specified array index.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            LinkedListNode<T> current = _head;

            while (current != null)
            {
                array[arrayIndex] = current.Value;
                current = current.Next;
            }
        }

        /// <summary>
        /// <para>
        /// Removes the first node in the list whose value equals the provided value. The
        /// method returns <see langword="true"/> if a value was removed. Otherwise it returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            LinkedListNode<T> previous = null;
            LinkedListNode<T> current = _head;

            // 1: Empty list: Do nothing.
            // 2: Single node: Previous is null.
            // 3: Many nodes:
            //    a: Node to remove is the first node.
            //    b: Node to remove is the middle or last.

            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    // It's a node in the middle or end.
                    if (previous != null)
                    {
                        // Case 3b.

                        // Before: Head -> 3 -> 5 -> null
                        // After:  Head -> 3 ------> null
                        previous.Next = current.Next;

                        // It was the end so update _tail.
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        // Case 2 or 3a.

                        // Before: Head -> 3 -> 5
                        // After:  Head ------> 5

                        // Head -> 3 -> null
                        // Head ------> null
                        _head = _head.Next;

                        // Is the list now empty?
                        if (_head == null)
                            _tail = null;
                    }

                    Count--;

                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// <para>
        /// Removes the first value from the list. If the list is empty, no action is taken.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void RemoveFirst()
        {
            if (Count != 0)
            {
                // Before: Head -> 3 -> 5
                // After:  Head ------> 5

                // Before: Head -> 3 -> null
                // After:  Head ------> null
                _head = _head.Next;

                Count--;

                if (Count == 0)
                    _tail = null;
            }
        }

        /// <summary>
        /// <para>
        /// Removes the last node from the list. If the list is empty, no action is performed.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        public void RemoveLast()
        {
            if (Count != 0)
            {
                if (Count == 1)
                {
                    _head = null;
                    _tail = null;
                }
                else
                {
                    LinkedListNode<T> previous = null;
                    LinkedListNode<T> current = _head;

                    while (current.Next != null)
                    {
                        previous = current;
                        current = current.Next;
                    }

                    // Before: Head -> 3 -> 5 -> 7
                    //         Tail = 7
                    // After:  Head -> 3 -> 5 -> null
                    //         Tail = 5
                    // Null out 5's Next property.
                    previous.Next = null;
                    _tail = previous;
                }

                Count--;
            }
        }

        /// <summary>
        /// <para>
        /// Returns an <see cref="IEnumerator{T}"/> instance that allows enumerating the linked list values
        /// from first to last.
        /// </para>
        /// <para>
        /// Performance: Returning the enumerator instance is an O(1) operation. Enumerating
        /// every item is an O(n) operation.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> current = _head;

            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();
    }
}