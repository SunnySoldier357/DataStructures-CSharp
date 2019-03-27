namespace DequeClass
{
    public class Stack<T>
    {
        //* Private Properties
        private Deque<T> _items = new Deque<T>();

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

        //* Public Properties

        /// <summary>
        /// <para>
        /// Adds an item to the top of the stack.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value) => _items.EnqueueFirst(value);

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
        public T Pop() => _items.DequeueFirst();

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
        public T Peek() => _items.PeekFirst();
    }
}