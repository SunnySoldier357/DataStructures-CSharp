namespace DoublyLinkedList
{
    public class LinkedListNode<T>
    {
        // Public Properties

        /// <summary>
        /// The next node in the linked list (<see langword="null"/> if last node).
        /// </summary>
        public LinkedListNode<T> Next { get; internal set; }
        /// <summary>
        /// The previous node in the linked list (<see langword="null"/> if first node).
        /// </summary>
        public LinkedListNode<T> Previous { get; internal set; }

        /// <summary>
        /// The node value.
        /// </summary>
        public T Value { get; internal set; }

        // Constructors
        
        /// <summary>
        /// Constructs a new node with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public LinkedListNode(T value) => 
            Value = value;
    }
}