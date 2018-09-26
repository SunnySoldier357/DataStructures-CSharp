using System;

namespace DoublyLinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessListBackwards();

            Console.ReadLine();
        }

        public static void ProcessListBackwards()
        {
            LinkedList<int> list = new LinkedList<int>();
            PopulateList(list);

            LinkedListNode<int> current = list.Tail;

            while (current != null)
            {
                ProcessNode(current);
                current = current.Previous;
            }
        }

        private static void ProcessNode(LinkedListNode<int> current)
        {
            Console.WriteLine(current.Value);
        }

        private static void PopulateList(LinkedList<int> list)
        {
            for (int i = 0; i < 10; i++)
                list.Add(i * 3);
        }
    }
}