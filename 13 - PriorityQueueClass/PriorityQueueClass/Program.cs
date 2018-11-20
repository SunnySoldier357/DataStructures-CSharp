using System;
using System.Threading;

namespace PriorityQueueClass
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = new PriorityQueue<Data>();
            var random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                int priority = random.Next() % 3;
                queue.Enqueue(new Data(string.Format("This is message {0}", i),
                    priority));

                Thread.Sleep(priority);
            }

            while (queue.Count > 0)
                Console.WriteLine(queue.Dequeue().ToString());
        }
    }
}