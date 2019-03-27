using System;
using SinglyLinkedList;

namespace BinaryTreeClass
{
    class Program
    {
        static void Main(string[] args)
        {
            // Binary Tree Rules
            //   1) Each node can have 0, 1, or 2 children.
            //   2) Any value less than the node value's goes to the left child
            //      (or a child of the left child)
            //   3) Any value greater than, or equal to, the node's value goes to
            //      the right child (or a child thereof).

            BinaryTree<int> tree = new BinaryTree<int>();

            //       +---+
            //       | 8 |
            //       +---+
            tree.Add(8);

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      ----
            //      |
            //    +---+
            //    | 4 |
            //    +---+
            tree.Add(4);

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      ----
            //      |
            //    +---+
            //    | 4 |
            //    +---+
            //      |
            //   ----
            //   |
            // +---+
            // | 2 |
            // +---+
            tree.Add(2);


            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      ----
            //      |
            //    +---+
            //    | 4 |
            //    +---+
            //      |
            //   ----
            //   |
            // +---+
            // | 2 |
            // +---+
            //   |
            //   ----
            //      |
            //    +---+
            //    | 3 |
            //    +---+
            tree.Add(3);

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 4 | | 9 |
            //    +---+ +---+
            //      |
            //   ----
            //   |
            // +---+
            // | 2 |
            // +---+
            //   |
            //   ----
            //      |
            //    +---+
            //    | 3 |
            //    +---+
            tree.Add(9);

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 4 | | 9 |
            //    +---+ +---+
            //      |
            //   -------
            //   |     |
            // +---+ +---+
            // | 2 | | 6 |
            // +---+ +---+
            //   |
            //   ----
            //      |
            //    +---+
            //    | 3 |
            //    +---+
            tree.Add(6);

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 4 | | 9 |
            //    +---+ +---+
            //      |
            //   -------
            //   |     |
            // +---+ +---+
            // | 2 | | 6 |
            // +---+ +---+
            //   |     |
            //   ----  ----
            //      |     |
            //    +---+ +---+
            //    | 3 | | 7 |
            //    +---+ +---+
            tree.Add(7);

            //* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //* ~~ Removing items from a Binary Tree ~~
            //* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //* Case 1: The node to be removed has no right child.

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    @---@ +---+
            //    | 5 | | 9 |
            //    @---@ +---+
            //      |
            //   ----
            //   |
            // +---+
            // | 2 |
            // +---+
            tree = new BinaryTree<int>();
            tree.Add(8);
            tree.Add(5);
            tree.Add(2);
            tree.Add(9);

            // The removal operation can simply move the left child, if there is
            // one, into the place of the removed node.

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 2 | | 9 |
            //    +---+ +---+
            tree.Remove(5);

            //* Case 2: The node to be removed has a right child which, in turn,
            //*         has no left child.

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    @---@ +---+
            //    | 5 | | 9 |
            //    @---@ +---+
            //      |
            //   -------
            //   |     |
            // +---+ +---+
            // | 2 | | 6 |
            // +---+ +---+
            //         |
            //         ----
            //            |
            //          +---+
            //          | 7 |
            //          +---+
            tree = new BinaryTree<int>();
            tree.Add(8);
            tree.Add(5);
            tree.Add(2);
            tree.Add(9);
            tree.Add(6);
            tree.Add(7);

            // In this case, we want to move the removed node's right child (6)
            // into the place of the removed node.

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 6 | | 9 |
            //    +---+ +---+
            //      |
            //   -------
            //   |     |
            // +---+ +---+
            // | 2 | | 7 |
            // +---+ +---+
            tree.Remove(5);

            //* Case 3: The node to be removed has a right child which, in turn,
            //*         has a left child.

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    @---@ +---+
            //    | 5 | | 9 |
            //    @---@ +---+
            //      |
            //   -------
            //   |     |
            // +---+ +---+
            // | 2 | | 7 |
            // +---+ +---+
            //         |
            //      ----
            //      |
            //    +---+
            //    | 6 |
            //    +---+
            tree = new BinaryTree<int>();
            tree.Add(8);
            tree.Add(5);
            tree.Add(2);
            tree.Add(9);
            tree.Add(7);
            tree.Add(6);

            // In this case, the left-most child of the removed node's right child
            // must be placed into the removed node's slot.

            //       +---+
            //       | 8 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 6 | | 9 |
            //    +---+ +---+
            //      |
            //   -------
            //   |     |
            // +---+ +---+
            // | 2 | | 7 |
            // +---+ +---+
            tree.Remove(5);

            //* ~~~~~~~~~~~~~~~~
            //* ~~ Traversals ~~
            //* ~~~~~~~~~~~~~~~~

            //       +---+
            //       | 4 |
            //       +---+
            //         |
            //      -------
            //      |     |
            //    +---+ +---+
            //    | 2 | | 5 |
            //    +---+ +---+
            //      |     |
            //   -------  ----
            //   |     |     |
            // +---+ +---+ +---+
            // | 1 | | 3 | | 7 |
            // +---+ +---+ +---+
            //               |
            //            -------
            //            |     |
            //          +---+ +---+
            //          | 6 | | 8 |
            //          +---+ +---+
            tree = new BinaryTree<int>()
            {
                4, 2, 1, 3, 5, 7, 6, 8
            };

            //* 1) Preorder Traversal

            // Order: 4, 2, 1, 3, 5, 7, 6, 8

            var copy = new BinaryTree<int>();

            // Useful for copying the entire tree with the same node values &
            // hierarchy
            tree.PreOrderTraversal(value => copy.Add(value));

            //* 2) Postorder Traversal

            // Order: 1, 3, 2, 6, 8, 7, 5, 4

            // Delete in a way that makes the least amount of work for Remove()
            copy.PostOrderTraversal(value => copy.Remove(value));

            //* 3) Inorder Traversal

            // Order: 1, 2, 3, 4, 5, 6, 7, 8

            var sorted = new LinkedList<int>();

            tree.InOrderTraversal(value => sorted.Add(value));
        }
    }
}