using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTreeClass
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable<T>
    {
        //* Private Properties
        private BinaryTreeNode<T> _head;

        private int _count;

        //* Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of values in the tree (0 if empty).
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count => _count;

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the provided value to the correct location within the tree.
        /// </para>
        /// <para>
        /// Performance: O(logn) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            // Case 1: The tree is empty. Allocate the head.
            if (_head == null)
                _head = new BinaryTreeNode<T>(value);
            // Case 2: The tree is not empty, so recursively find the right
            // location to insert the node.
            else
                addTo(_head, value);

            _count++;
        }

        /// <summary>
        /// <para>
        /// Removes all the nodes from the tree.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        /// <summary>
        /// <para>
        /// Returns <see langword="true"/> if the tree contains the provided
        /// value. Otherwise it returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// Performance: O(logn) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            // Defer to the node search helper function.
            BinaryTreeNode<T> parent;
            return findWithParent(value, out parent) != null;
        }

        /// <summary>
        /// <para>
        /// Removes the first node found with the indicated value.
        /// </para>
        /// <para>
        /// Performance: O(logn) on average; O(n) in the worst case.
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Remove(T value)
        {
            BinaryTreeNode<T> current, parent;

            // Find the node to remove.
            current = findWithParent(value, out parent);

            if (current == null)
                return false;

            _count--;

            // Case 1: If current has no right child, current's left replaces
            //         current.
            if (current.Right == null)
            {
                if (parent == null)
                    _head = current.Left;
                else
                {
                    int result = parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // If parent value is greater than current value, make
                        // the current left child a left child of parent.
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // If parent value is less than current value, make
                        // the current left child a right child of parent.
                        parent.Right = current.Left;
                    }
                }
            }
            // Case 2: If current's right child has no left child, current's right
            //         child replaces current.
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                    _head = current.Right;
                else
                {
                    int result = parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // If parent value is greater than current value, make the
                        // current right child a left child of parent.
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // If parent value is less than current value, make the
                        // current right child a right child of parent.
                        parent.Right = current.Right;
                    }
                }
            }
            // Case 3: If current's right child has a left child, replace current
            //         with current's right child's left-most child.
            else
            {
                // Find the right's left-most child.
                BinaryTreeNode<T> leftmost = current.Right.Left;
                BinaryTreeNode<T> leftmostParent = current.Right;

                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // The parent's left subtree becomes the leftmost's right subtree.
                leftmostParent.Left = leftmost.Right;

                // Assign leftmost's left & right to current's left and right children.
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                    _head = leftmost;
                else
                {
                    int result = parent.CompareTo(current.Value);

                    if (result > 0)
                    {
                        // If parent value is greater than current value, make
                        // leftmost the parent's left child.
                        parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // If parent value is less than current value, make
                        // leftmost the parent's right child.
                        parent.Right = leftmost;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// <para>
        /// Performs the provided action on each value in preorder.
        /// </para>
        /// <para>
        /// The preorder traversal processes the current node before moving to
        /// the left and then right children.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="action"></param>
        public void PreOrderTraversal(Action<T> action) =>
            preOrderTraversal(action, _head);

        /// <summary>
        /// <para>
        /// Performs the provided action on each value in postorder.
        /// </para>
        /// <para>
        /// The postorder traversal visits the left and right child of the node
        /// recursively, and then performs the action on the current node after
        /// the children are complete.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="action"></param>
        public void PostOrderTraversal(Action<T> action) =>
            postOrderTraversal(action, _head);

        /// <summary>
        /// This is a non-recursive algorithm using a stack to demonstrate removing
        /// recursion.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> InOrderTraversal()
        {
            if (_head != null)
            {
                // Store the nodes we've skipped in this stack (avoids recursion)
                var stack = new StackClass.Stack<BinaryTreeNode<T>>();

                BinaryTreeNode<T> current = _head;

                // When removing recursion, we need to keep track of whether we
                // should be going to the left node or the right nodes next.
                bool goLeftNext = true;

                // Start by pushing Head onto the stack.
                stack.Push(current);

                while (stack.Count > 0)
                {
                    // If we're heading left...
                    if (goLeftNext)
                    {
                        // Push everything but the left-most node ti the stack.
                        // We'll yield the left-most after this block.
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    // Inorder is left -> yield -> right.
                    yield return current.Value;

                    // If we can go right, do so.
                    if (current.Right != null)
                    {
                        current = current.Right;

                        // Once we've gone right once, we need to start going
                        // left again.
                        goLeftNext = true;
                    }
                    else
                    {
                        // If we can't go right, then we need to pop off the parent
                        // node so we can process it and then go to its right node.
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        /// <summary>
        /// <para>
        /// Performs the provided action on each value in inorder.
        /// </para>
        /// <para>
        /// Inorder traversal processes the nodes in the sort order.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="action"></param>
        public void InOrderTraversal(Action<T> action) =>
            inOrderTraversal(action, _head);

        /// <summary>
        /// <para>
        /// Returns an enumerator that enumerates using the InOrder traversal
        /// algorithm.
        /// </para>
        /// <para>
        /// Performance: O(1) to return the enumerator. Enumerating all the items
        /// is O(n).
        /// </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() =>
            InOrderTraversal();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        //* Private Methods

        /// <summary>
        /// Recursive add algorithm.
        /// </summary>
        private void addTo(BinaryTreeNode<T> node, T value)
        {
            // Case 1: Value is less than the current node value.
            if (value.CompareTo(node.Value) < 0)
            {
                // If there is no left child, make this the new left,
                if (node.Left == null)
                    node.Left = new BinaryTreeNode<T>(value);
                else
                {
                    // else add it to the left node.
                    addTo(node.Left, value);
                }
            }
            // Case 2: Value is equal to or greater than the current value.
            else
            {
                // If there is no right, add it to the right,
                if (node.Right == null)
                    node.Right = new BinaryTreeNode<T>(value);
                else
                {
                    // else add it to the right node.
                    addTo(node.Right, value);
                }
            }
        }

        /// <summary>
        /// Finds and returns the first node containing the specified value. If
        /// the value is not found, it returns <see langword="null"/>. Also
        /// returns the parent of the found node (or <see langword="null"/>)
        /// which is used in <see cref="Remove()"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private BinaryTreeNode<T> findWithParent(T value, out BinaryTreeNode<T> parent)
        {
            // Now, try to find data in the tree.
            BinaryTreeNode<T> current = _head;
            parent = null;

            // While we don't have a match...
            while (current != null)
            {
                int result = current.CompareTo(value);

                if (result > 0)
                {
                    // If the value is less than current, go left.
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                
                {
                    // If the value is more than current, go right.
                    parent = current;
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

        private void preOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                action(node.Value);

                preOrderTraversal(action, node.Left);
                preOrderTraversal(action, node.Right);
            }
        }

        private void postOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                postOrderTraversal(action, node.Left);
                postOrderTraversal(action, node.Right);

                action(node.Value);
            }
        }

        private void inOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                inOrderTraversal(action, node.Left);

                action(node.Value);

                inOrderTraversal(action, node.Right);
            }
        }
    }
}