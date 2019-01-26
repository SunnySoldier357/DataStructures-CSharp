using System;
using System.Collections;
using System.Collections.Generic;

namespace AVLTreeClass
{
    public class AVLTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        //* Public Properties
        public AVLTreeNode<T> Head { get; internal set; }

        public int Count { get; private set; }

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the specific value to the tree, ensuring that the tree is in a
        /// balanced state when the methd completes.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            // Case 1: The tree is empty--allocate the head.
            if (Head == null)
                Head = new AVLTreeNode<T>(value, null, this);
            // Case 2: The tree is not empty--find the right location to insert.
            else
                addTo(Head, value);

            Count++;
        }

        public void Clear()
        {
            Head = null;
            Count = 0;
        }

        /// <summary>
        /// <para>
        /// Returns true if the specific value is contained within the tree.
        /// Otherwise, it returns false.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value) =>
            find(value) != null;

        /// <summary>
        /// <para>
        /// Enumerates the values contained in the binary tree in inorder
        /// traversal order.
        /// </para>
        /// <para>
        /// Performance: O(1) to return the enumerator.true O(n) for the caller
        /// to enumerate each node.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> InOrderTraversal()
        {
            // This is a non-recursive algorithm using a stack to demonstrate
            // removing recursion to make using the yield syntax easier.
            if (Head != null)
            {
                // Store the nodes we've skipped in this stack (avoids recursion)
                var stack = new Stack<AVLTreeNode<T>>();

                AVLTreeNode<T> current = Head;

                // When removing recursion, we need to keep track of whether we
                // should be going to the left nodes or the right nodes next.
                bool goLeftNext = true;

                // Start by pushing the Head onto the stack.
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // If we're going left...
                    if (goLeftNext)
                    {
                        // Push everything but the leftmost node to the stack.
                        // We'll yield the leftmost after this block.
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // Inorder is left -> yield -> right
                    yield return current.Value;

                    // If we can go right, then do so.
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // Once we've gone right once, we need to start going
                        // left again.
                        goLeftNext = true;
                    }
                    else
                    {
                        // If we can't go right we need to pop off the parent
                        // node so we can process it and then go to its right
                        // node.
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        /// <summary>
        /// <para>
        /// If the specified value exists within the tree, the value is removed
        /// and the method returns true. Otherwise the method returns false.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if he value was removed; false otherwise.</returns>
        public bool Remove(T value)
        {
            AVLTreeNode<T> current;
            current = find(value);

            if (current == null)
                return false;

            AVLTreeNode<T> treeToBalance = current.Parent;

            Count--;

            // Case 1: If current has no right child, then current's left replaces
            //         current.
            if (current.Right == null)
            {
                if (current.Parent == null)
                {
                    Head = current.Left;
                    if (Head != null)
                        Head.Parent = null;
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // If the parent value is greater than the current value,
                        // make the current left child a left child of the parent.
                        current.Parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // If the parent value is less than the current value,
                        // make the current left child a right child of the parent.
                        current.Parent.Right = current.Left;
                    }
                }
            }
            // Case 2: If current's right child has no left child, then current's
            //         right child replaces current.
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (current.Parent == null)
                {
                    Head = current.Right;
                    if (Head != null)
                        Head.Parent = null;
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // If the parent value is greater than the current value,
                        // make the current right child a left child of the parent.
                        current.Parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // If the parent value is less than the current value,
                        // make the current right child a right child of the parent.
                        current.Parent.Right = current.Right;
                    }
                }
            }
            // Case 3: If current's right child has a left child, replace current
            //         with current's right child's leftmost child.
            else
            {
                // Find the right's leftmost child.
                AVLTreeNode<T> leftmost = current.Right.Left;

                while (leftmost.Left != null)
                    leftmost = leftmost.Left;

                // The parent's left subtree becomes the leftmost's right subtree.
                leftmost.Parent.Left = leftmost.Right;

                // Assign leftmost's left and right to current's left and right
                // children.
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (current.Parent == null)
                {
                    Head = leftmost;
                    if (Head != null)
                        Head.Parent = null;
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // If the parent value is greater than the current value,
                        // make the leftmost the parent's left child.
                        current.Parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // If the parent value is less than the current value,
                        // make the leftmost the parent's right child.
                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (treeToBalance != null)
                treeToBalance.Balance();
            else
            {
                if (Head != null)
                    Head.Balance();
            }

            return true;
        }

        /// <summary>
        /// <para>
        /// Returns an enumerator that performs an inorder traversal of the
        /// binary tree.
        /// </para>
        /// <para>
        /// Performance: O(1) to return the enumerator.true O(n) for the caller
        /// to enumerate each node.
        /// </para>
        /// </summary>
        /// <returns>The inorder enumerator.</returns>
        public IEnumerator<T> GetEnumerator() =>
            InOrderTraversal();

        /// <summary>
        /// <para>
        /// Returns an enumerator that performs an inorder traversal of the
        /// binary tree.
        /// </para>
        /// <para>
        /// Performance: O(1) to return the enumerator.true O(n) for the caller
        /// to enumerate each node.
        /// </para>
        /// </summary>
        /// <returns>The inorder enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        //* Private Methods

        // Recursive add algorithm
        private void addTo(AVLTreeNode<T> node, T value)
        {
            // Case 1: Value is less than the current node value.
            if (value.CompareTo(node.Value) < 0)
            {
                // If there is no left child, make this the new left.
                if (node.Left == null)
                    node.Left = new AVLTreeNode<T>(value, node, this);
                else
                {
                    // Else, add it to the left node.
                    addTo(node.Left, value);
                }
            }
            // Case 2: Value is equal to or greater than the current value.
            else
            {
                // If there is no right, add it to the right.
                if (node.Right == null)
                    node.Right = new AVLTreeNode<T>(value, node, this);
                else
                {
                    // Else, add it to the right node.
                    addTo(node.Right, value);
                }
            }

            node.Balance();
        }

        /// <summary>
        /// Finds and returns the first node containing the specified value. If
        /// the value is not found, it returns null. It also returns the parent
        /// of the found node (or null) which is used in Remove.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The found node (or null).</returns>
        private AVLTreeNode<T> find(T value)
        {
            // Now, try to find data in the tree.
            AVLTreeNode<T> current = Head;

            // While we don't have a match...
            while (current != null)
            {
                int result = current.CompareTo(value);

                if (result > 0)
                {
                    // If the value is greater than current, go right.
                    current = current.Right;
                }
                else if (result < 0)
                {
                    // If the value is less than current, go left.
                    current = current.Right;
                }
                else
                {
                    // We have a match!
                    break;
                }
            }

            return current;
        }
    }
}