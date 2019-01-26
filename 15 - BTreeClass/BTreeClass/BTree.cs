using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BTreeClass
{
    public class BTree<T> : ICollection<T> where T : IComparable<T>
    {
        //* Constants
        public const int MINIMUM_DEGREE = 2;

        //* Private Properties
        private BTreeNode<T> root;

        //* Public Properties

        /// <summary>
        /// Returns a value indicating whether the tree is read-only.
        /// </summary>
        /// <value></value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Returns the number of values in the tree (0 if the tree is empty).
        /// </summary>
        /// <value></value>
        public int Count { get; private set; }

        //* Internal Static Methods
        internal static void FindPotentialPath(BTreeNode<T> node, T value,
            out int valueIndex, out int childIndex)
        {
            // We want to find out which child the value we are searching for
            // (value) would be in if the value were in the tree.
            childIndex = node.Children.Count - 1;
            valueIndex = node.Values.Count - 1;

            // Start at the rightmost child and value indexes and work backward
            // until we are at less than the value we want.
            while (valueIndex > 0)
            {
                int compare = value.CompareTo(node.Values[valueIndex]);

                if (compare > 0)
                    break;

                childIndex--;
                valueIndex--;
            }

            // If we make it all the way to the last value...
            if (valueIndex == 0)
            {
                // If the value we are searching for is less than the first value
                // in the node, than the child is the 0 index child, not the 1
                // index.
                if (value.CompareTo(node.Values[valueIndex]) < 0)
                    childIndex--;
            }
        }

        internal static bool RemoveValue(BTreeNode<T> node, T value)
        {
            if (node.Leaf)
            {
                // Deletion case 1...

                // By the time we are in a leaf node, we have either pushed down
                // values such that the leaf node has minimum degree children
                // and can therefore have one node removed, OR the root node is
                // also a leaf node and we can freely violate the minimum rule.
                return node.DeleteKeyFromLeafNode(value);
            }

            int valueIndex;
            if (tryGetIndexOf(node, value, out valueIndex))
            {
                // Deletion case 2...

                // We have found the non-leaf node the value is in. Since we can
                // only delete values from a lead node, we need to push the value
                // to delete down into a child.

                // If the child that precedes the value to delete (the "left"
                // child) has at least the minimum degree of children...
                if (node.Children[valueIndex].Values.Count >=
                    node.Children[valueIndex].MinimumDegree)
                {
                    //     [3       6         10]
                    // [1 2]  [4 5]   [7 8 9]    [11 12]

                    // Deleting 10.

                    // Find the largest value in the child node that contains
                    // smaller values than what is being deleted (this is the
                    // value 9)...
                    T valuePrime = findPredecessor(node, valueIndex);

                    // and REPLACE the value to delete with the next largest
                    // value (the one we just found--swapping 9 & 10).
                    node.ReplaceValue(valueIndex, valuePrime);

                    // After the swap...

                    //     [3      6         9]
                    // [1 2] [4 5]   [7 8 9]    [11 12]

                    // notice that 9 is in the tree twice. This is not a typo.
                    // We are about to delete it from the child we took it from.

                    // Delete the value we moved up (9) from the child (this
                    // may in turn push it down to subsequent children until it
                    // is in a leaf).
                    return RemoveValue(node.Children[valueIndex], valuePrime);

                    // Final tree:

                    //     [3       6        9]
                    // [1 2]  [4 5]   [7 8 ]   [11 12]
                }
                else
                {
                    // If the left child did not have enough values to move one
                    // of its values up, check whether the right child does.
                    if (node.Children[valueIndex + 1].Values.Count >=
                        node.Children[valueIndex + 1].MinimumDegree)
                    {
                        // See the previous algorithm and do the opposite...

                        //     [3       6         10]
                        // [1 2]  [4 5]   [7 8 9]    [11 12]

                        // Deleting 6.

                        // Successor = 7.
                        T valuePrime = findSuccessor(node, valueIndex);
                        node.ReplaceValue(valueIndex, valuePrime);

                        // After replacing 6 with 7, the tree is:

                        //     [3       7         10]
                        // [1 2]  [4 5]   [7 8 9]    [11 12]

                        // Now remove 7 from the child.
                        return RemoveValue(node.Children[valueIndex + 1],
                            valuePrime);

                        // Final tree:

                        //     [3       7         10]
                        // [1 2]  [4 5]   [8 9]    [11 12]
                    }
                    else
                    {
                        // If neither child has the minimum degree of children,
                        // it means they both have (minimum - 1) children. Since
                        // a node can have (2 * <minimum degree> - 1) children,
                        // we can safely merge the 2 nodes into a single child.

                        //     [3     6     9]
                        // [1 2] [4 5] [7 8] [10 11]

                        // Deleting 6.

                        // [4 5] and [7 8] are merged into a single node with
                        // [6] pushed down into it.

                        //     [3          9]
                        // [1 2] [4 5 6 7 8] [10 11]

                        BTreeNode<T> newChildNode = node.PushDown(valueIndex);

                        // Now that we've pushed the value down a level, we can
                        // call remove on the new child node [4 5 6 7 8].
                        return RemoveValue(newChildNode, value);
                    }
                }
            }
            else
            {
                // Deletion case 3...

                // We are at an internal node which does not contain the value
                // we want to delete. First, find the child path that the value
                // we want to delete would be in. If it exists in the tree...
                int childIndex;
                FindPotentialPath(node, value, out valueIndex, out childIndex);

                // Now that we know where the value should be, we need to ensure
                // that the node we are getting to has the minimum number of
                // values necessary to delete from.
                if (node.Children[childIndex].Values.Count ==
                    node.Children[childIndex].MinimumDegree - 1)
                {
                    // Since the node does not have enough values, what we want
                    // to do is borrow a value from a sibling that has enough
                    // values to share.

                    // Determine id the left or right sibling has the most
                    // children.
                    int indexOfMaxSibling = getIndexOfMaxSibling(childIndex,
                        node);

                    // If a sibling with values exists (maybe we're at the root
                    // node and don't have one) and that sibling has enough
                    // values...
                    if (indexOfMaxSibling >= 0 &&
                        node.Children[indexOfMaxSibling].Values.Count >=
                            node.Children[indexOfMaxSibling].MinimumDegree)
                    {
                        // Rotate the appropriate value from the sibling through
                        // the parent and into the current node so that we have
                        // enough values in the current node to push a value down
                        // into the child we are going to check next.

                        //     [3      7]
                        // [1 2] [4 5 6]  [8 9]

                        // The node we want to travel through is [1 2], but we
                        // need another node in it. So we rotate the 4 up to the
                        // root and push the 3 down into the [1 2] node.

                        //       [4     7]
                        // [1 2 3] [5 6]  [8 9]
                        RotateAndPushDown(node, childIndex, indexOfMaxSibling);
                    }
                    else
                    {
                        // Merge (which may push the only node in the root down
                        // --so new root).
                        BTreeNode<T> pushedDownNode = node.PushDown(valueIndex);

                        // Now we find the node we just pushed down.
                        childIndex = 0;
                        while (pushedDownNode != node.Children[childIndex])
                            childIndex++;
                    }
                }

                return RemoveValue(node.Children[childIndex], value);
            }
        }

        internal static void RotateAndPushDown(BTreeNode<T> node, int childIndex,
            int indexOfMaxSibling)
        {
            int valueIndex;

            if (childIndex < indexOfMaxSibling)
                valueIndex = childIndex;
            else
                valueIndex = childIndex - 1;

            if (indexOfMaxSibling > childIndex)
            {
                // We are moving the leftmost key from the right sibling into the
                // parent and pushing the parent down into the child.

                //    [6      10]
                // [1]  [7 8 9] [11]

                // Deleting something less than 6.

                //    [7   10]
                // [1 6] [8 9] [11]

                // Grab the 7.
                T valueToMoveToX = node.Children[indexOfMaxSibling].Values.First();

                // Get 7's left child if it has one (not a leaf).
                BTreeNode<T> childToMoveToNode =
                    node.Children[indexOfMaxSibling].Leaf ? null :
                        node.Children[indexOfMaxSibling].Children.First();

                // Record the 6 (the push down value).
                T valueToMoveDown = node.Values[valueIndex];

                // Move the 7 into the parent.
                node.ReplaceValue(valueIndex, valueToMoveToX);

                // Move the 6 into the child.
                node.Children[childIndex].AddEnd(valueToMoveDown, childToMoveToNode);

                // Remove the first value and child from the sibling now that
                // they've been moved.
                node.Children[indexOfMaxSibling].RemoveFirst();
            }
            else
            {
                // We are moving the rightmost key from the left sibling into the
                // parent and pushing the parent down into the child.

                //    [6      10]
                // [1]  [7 8 9] [11]

                // Deleting something greater than 10.

                //    [6     9]
                // [1]  [7 8] [10 11]

                // Grab the 9.
                T valueToMoveToX = node.Children[indexOfMaxSibling].Values.Last();

                // Get 9's right child if it has one (not a leaf node).
                BTreeNode<T> childToMoveToNode =
                    node.Children[indexOfMaxSibling].Leaf ? null :
                        node.Children[indexOfMaxSibling].Children.Last();

                // Record the 10 (the push down value).
                T valueToMoveDown = node.Values[valueIndex];

                // Move the 9 into the parent.
                node.ReplaceValue(valueIndex, valueToMoveToX);

                // Move the 10 into the child.
                node.Children[childIndex].AddFront(valueToMoveDown,
                    childToMoveToNode);

                // Remove the last value and child from the sibling so that
                // they've been moved.
                node.Children[indexOfMaxSibling].RemoveLast();
            }
        }

        //* Private Static Methods

        // Finds the value of the predecessor value of a specific value in a
        // node.
        //
        // Example:
        //
        //     [3     6]
        // [1 2] [4 5] [7 8]
        //
        // The predecessor of 3 is 2.
        private static T findPredecessor(BTreeNode<T> node, int index)
        {
            node = node.Children[index];

            while (!node.Leaf)
                node = node.Children.Last();

            return node.Values.Last();
        }

        // Finds the value of the successor value of a specific value in a node.
        //
        // Example:
        //
        //     [3     6]
        // [1 2] [4 5] [7 8]
        //
        // The predecessor of 3 is 4.
        private static T findSuccessor(BTreeNode<T> node, int index)
        {
            node = node.Children[index + 1];

            while (!node.Leaf)
                node = node.Children.First();

            return node.Values.First();
        }

        // Returns the index (to the left or right) of the child node that has
        // the most values in it.
        //
        // Example
        //
        //     [3      7]
        // [1 2] [4 5 6] [8 9]
        //
        // If we pass in the [3 7] node with index 0, the left child [1 2] and
        // right child [4 5 6] would be checked and the index 1 for child node
        // [4 5 6] would be returned.
        //
        // If we checked [3 7] with index 1, the left child [4 5 6] and the right
        // child [8 9] would be checked and the value 1 would be returned.
        private static int getIndexOfMaxSibling(int index, BTreeNode<T> node)
        {
            int indexOfMaxSibling = -1;

            BTreeNode<T> leftSibling = null;
            if (index > 0)
                leftSibling = node.Children[index - 1];

            BTreeNode<T> rightSibling = null;
            if (index + 1 < node.Children.Count)
                rightSibling = node.Children[index + 1];

            if (leftSibling != null || rightSibling != null)
            {
                if (leftSibling != null && rightSibling != null)
                {
                    indexOfMaxSibling = leftSibling.Values.Count >
                       rightSibling.Values.Count ? index - 1 : index + 1;
                }
                else
                {
                    indexOfMaxSibling = leftSibling != null ?
                        index - 1 : index + 1;
                }
            }

            return indexOfMaxSibling;
        }

        private static bool tryGetIndexOf(BTreeNode<T> node, T value,
            out int valueIndex)
        {
            for (int index = 0; index < node.Values.Count; index++)
            {
                if (value.CompareTo(node.Values[index]) == 0)
                {
                    valueIndex = index;
                    return true;
                }
            }

            valueIndex = -1;
            return false;
        }

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the specific value to the tree.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            if (root == null)
            {
                root = new BTreeNode<T>(null, true, MINIMUM_DEGREE,
                    new[] { value }, new BTreeNode<T>[] { });
            }
            else
            {
                if (root.Full)
                    root = root.SplitFullRootNode();

                insertNonFull(root, value);
            }
        }

        /// <summary>
        /// <para>
        /// Removes all values from the tree and sets the count to 0.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public void Clear()
        {
            root = null;
            Count = 0;
        }

        /// <summary>
        /// <para>
        /// Returns true if the specified value exists in the tree. Otherwise it
        /// returns false.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            BTreeNode<T> node;
            int valueIndex;
            return TryFindNodeContainingValue(value, out node, out valueIndex);
        }

        /// <summary>
        /// <para>
        /// Copies every value in the tree into the target array, starting at the
        /// specified index.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T value in InOrderEnumerator(root))
                array[arrayIndex++] = value;
        }

        /// <summary>
        /// <para>
        /// Removes the first occurrence of the specified value from the tree.
        /// Returns true if a value was found and removed. Otherwise it returns
        /// false.
        /// </para>
        /// <para>
        /// Performance: O(logn)
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Remove(T value)
        {
            bool removed = false;

            if (Count > 0)
            {
                removed = RemoveValue(root, value);
                if (removed)
                {
                    Count--;

                    if (Count == 0)
                        root = null;
                    else if (root.Values.Count == 0)
                        root = root.Children[0];
                }
            }

            return removed;
        }

        public IEnumerator<T> GetEnumerator() =>
            inOrderEnumerator(root).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //* Internal Methods

        /// <summary>
        /// Searches the node and its children, looking for the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryFindNodeContainingValue(T value, out BTreeNode<T> node,
            out int valueIndex)
        {
            BTreeNode<T> current = root;

            // If the current node is null, then we never found the value.
            // Otherwise, we still have hope.
            while (current != null)
            {
                int index = 0;

                // Check each value in the node.
                while (index < current.Values.Count)
                {
                    int compare = value.CompareTo(current.Values[index]);

                    // Did we find it?
                    if (compare == 0)
                    {
                        // Awesome!
                        node = current;
                        valueIndex = index;
                        return true;
                    }

                    // If the value to find is less than the current node's
                    // value, then we want to go left (which is where we are).
                    if (compare < 0)
                        break;

                    // Otherwise, move on to the next value in the node.
                    index++;
                }

                if (current.Leaf)
                {
                    // If we are at the leaf node, there is no child to go down
                    // to.
                    break;
                }
                else
                {
                    // Otherwise, go into the child we determined must contain
                    // the value we want to find.
                    current = current.Children[index];
                }
            }

            node = null;
            valueIndex = -1;
            return false;
        }

        //* Private Methods
        private IEnumerable<T> inOrderEnumerator(BTreeNode<T> node)
        {
            if (node != null)
            {
                if (node.Leaf)
                {
                    foreach (T value in node.Values)
                        yield return value;
                }
                else
                {
                    IEnumerator<BTreeNode<T>> children =
                        node.Children.GetEnumerator();
                    IEnumerator<T> values = node.Values.GetEnumerator();

                    while (children.MoveNext())
                    {
                        foreach (T childValue in inOrderEnumerator(children.Current))
                            yield return childValue;
                        
                        if (values.MoveNext())
                            yield return values.Current;
                    }
                }
            }
        }

        private void insertNonFull(BTreeNode<T> node, T value)
        {
            if (node.Leaf)
                node.InsertKeyToLeafNode(value);
            else
            {
                int index = node.Values.Count - 1;

                while (index >= 0 && value.CompareTo(node.Values[index]) < 0)
                    index--;

                index++;

                if (node.Children[index].Full)
                {
                    node.SplitFullChild(index);

                    if (value.CompareTo(node.Values[index]) > 0)
                        index++;
                }

                insertNonFull(node.Children[index], value);
            }
        }
    }
}