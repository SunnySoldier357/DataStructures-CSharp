using System;

namespace SinglyLinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            // +-----+------+
            // |  3  | null +
            // +-----+------+
            Node first = new Node { Value = 3 };

            // +-----+------+    +-----+------+
            // |  3  | null +    |  5  | null +
            // +-----+------+    +-----+------+
            Node middle = new Node { Value = 5 };

            // +-----+------+    +-----+------+
            // |  3  |  *---+--->|  5  | null +
            // +-----+------+    +-----+------+
            first.Next = middle;

            // +-----+------+    +-----+------+    +-----+------+
            // |  3  |  *---+--->|  5  | null +    |  7  | null +
            // +-----+------+    +-----+------+    +-----+------+
            Node last = new Node { Value = 7 };

            // +-----+------+    +-----+------+    +-----+------+
            // |  3  |  *---+--->|  5  |  *---+--->|  7  | null +
            // +-----+------+    +-----+------+    +-----+------+
            middle.Next = last;

            PrintList(first);

            Console.WriteLine("\n=> Using LinkedList<T>\n");


            Console.ReadLine();
        }

        private static void PrintList(Node node)
        {
            while (node != null)
            {
                Console.WriteLine(node.Value);
                node = node.Next;
            }
        }
    }
}