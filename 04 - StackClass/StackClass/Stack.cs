using System;
using SinglyLinkedList;

namespace StackClass
{
    public class Stack<T>
    {
        //* Private Properties
        private LinkedList<T> _items = new LinkedList<T>();

        //* Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of items in the stack.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count => _items.Count;

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds an item to the top of the stack.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value) => _items.Add(value);

        /// <summary>
        /// <para>
        /// Removes and returns the last item added to the stack. If the stack
        /// is empty, an <see cref="InvalidOperationException"/> is thrown.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        public T Pop()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("The stack is empty");

            T result = _items.Tail.Value;
            _items.RemoveLast();

            return result;
        }

        /// <summary>
        /// <para>
        /// Returns the last item added to the stack but leaves the item on the
        /// stack. If the stack is empty, an <see cref="InvalidOperationException"/>
        /// is thrown.
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