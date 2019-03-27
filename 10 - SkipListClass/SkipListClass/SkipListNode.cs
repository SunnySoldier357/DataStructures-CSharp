namespace SkipListClass
{
    public class SkipListNode<T>
    {
        //* Public Properties

        /// <summary>
        /// The array of links. The number of items is the height of the links.
        /// </summary>
        /// <value></value>
        public SkipListNode<T>[] Next { get; private set; }

        /// <summary>
        /// The contained value.
        /// </summary>
        /// <value></value>
        public T Value { get; private set; }

        //* Constructors

        /// <summary>
        /// Creates a new node with the specified value at the indicated link
        /// height.
        /// </summary>
        public SkipListNode(T value, int height)
        {
            Value = value;
            Next = new SkipListNode<T>[height];
        }
    }
}