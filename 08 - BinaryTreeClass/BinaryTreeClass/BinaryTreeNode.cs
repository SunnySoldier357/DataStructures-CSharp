using System;

namespace BinaryTreeClass
{
    public class BinaryTreeNode<TNode> : IComparable<TNode>
        where TNode : IComparable<TNode>
    {
        // Public Properties

        /// <summary>
        /// The left child node of the tree. (<see langword="null"/> if there is 
        /// no left child)
        /// </summary>
        public BinaryTreeNode<TNode> Left { get; set; }
        /// <summary>
        /// The right child node of the tree. (<see langword="null"/> if there is 
        /// no right child)
        /// </summary>
        public BinaryTreeNode<TNode> Right { get; set; }

        /// <summary>
        /// The node value.
        /// </summary>
        public TNode Value { get; private set; }

        // Constructors

        /// <summary>
        /// Constructs a new node with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public BinaryTreeNode(TNode value) =>
            Value = value;

        // Public Methods
        public int CompareTo(TNode other) =>
            Value.CompareTo(other);
    }
}