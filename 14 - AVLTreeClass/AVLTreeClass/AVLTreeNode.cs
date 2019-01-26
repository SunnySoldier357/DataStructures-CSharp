using System;

namespace AVLTreeClass
{
    public class AVLTreeNode<TNode> : IComparable<TNode>
        where TNode : IComparable<TNode>
    {
        //* Private Properties
        private AVLTree<TNode> _tree;

        private AVLTreeNode<TNode> _left;
        private AVLTreeNode<TNode> _right;

        private int balanceFactor => rightHeight - leftHeight;
        private int leftHeight => maxChildHeight(Left);
        private int rightHeight => maxChildHeight(Right);

        private TreeState state
        {
            get
            {
                if (leftHeight - rightHeight > 1)
                    return TreeState.LeftHeavy;

                if (rightHeight - leftHeight > 1)
                    return TreeState.RightHeavy;

                return TreeState.Balanced;
            }
        }

        //* Public Properties
        public AVLTreeNode<TNode> Left
        {
            get => _left;
            internal set
            {
                _left = value;
                if (Left != null)
                    _left.Parent = this;
            }
        }
        public AVLTreeNode<TNode> Parent { get; internal set; }
        public AVLTreeNode<TNode> Right
        {
            get => _right;
            internal set
            {
                _right = value;
                if (_right != null)
                    _right.Parent = this;
            }
        }

        public TNode Value { get; private set; }

        //* Constructors
        public AVLTreeNode(TNode value, AVLTreeNode<TNode> parent,
        AVLTree<TNode> tree)
        {
            Value = value;
            Parent = parent;
            _tree = tree;
        }

        // Internal Methods
        internal void Balance()
        {
            if (state == TreeState.RightHeavy)
            {
                if (Right != null && Right.balanceFactor < 0)
                    leftRightRotation();
                else
                    leftRotation();
            }
            else if (state == TreeState.LeftHeavy)
            {
                if (Left != null && Left.balanceFactor > 0)
                    rightLeftRotation();
                else
                    rightRotation();
            }
        }

        //* Private Methods
        private void leftRightRotation()
        {
            Right.rightRotation();
            leftRotation();
        }

        private void leftRotation()
        {
            //     a
            //      \
            //       b
            //        \
            //         c
            //
            // becomes
            //       b
            //      / \
            //     a   c

            AVLTreeNode<TNode> newRoot = Right;

            // Replace the current root with the new root.
            replaceRoot(newRoot);

            // Take ownership of right's left child as right (now parent).
            Right = newRoot.Left;

            // The new root takes this as its left.
            newRoot.Left = this;
        }

        private void rightLeftRotation()
        {
            Left.leftRotation();
            rightRotation();
        }

        private void rightRotation()
        {
            //     c (this)
            //    /
            //   b
            //  /
            // a
            //
            // becomes
            //      b
            //     / \
            //    a   c

            AVLTreeNode<TNode> newRoot = Left;

            // Replace the current root with the new root.
            replaceRoot(newRoot);

            // Take ownership of left's right child as left (now parent).
            Left = newRoot.Right;

            // The new root takes this as its right.
            newRoot.Right = this;
        }

        private int maxChildHeight(AVLTreeNode<TNode> node)
        {
            if (node != null)
                return 1 + Math.Max(maxChildHeight(node.Left),
                    maxChildHeight(node.Right));

            return 0;
        }

        private void replaceRoot(AVLTreeNode<TNode> newRoot)
        {
            if (Parent != null)
            {
                if (Parent.Left == this)
                    Parent.Left = newRoot;
                else if (Parent.Right == this)
                    Parent.Right = newRoot;
            }
            else
                _tree.Head = newRoot;

            newRoot.Parent = Parent;
            Parent = newRoot;
        }

        //* Interface Implementations

        /// <summary>
        /// Compares the current node to the provided value.
        /// </summary>
        /// <param name="other">The node value to compare to.</param>
        /// <returns>
        /// 1 if the other value is greater than the provided value, -1 if less,
        /// or 0 if equal.
        /// </returns>
        public int CompareTo(TNode other) =>
            Value.CompareTo(other);

        //* Internal Classes
        enum TreeState
        {
            Balanced,
            LeftHeavy,
            RightHeavy
        }
    }
}