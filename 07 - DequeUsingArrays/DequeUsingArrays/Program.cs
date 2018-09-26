using System;

namespace DequeUsingArrays
{
    class Program
    {
        static void Main(string[] args)
        {
            Deque<int> deq = new Deque<int>();

            // +---+---+---+---+
            // | 1 |   |   |   |
            // +---+---+---+---+
            //  h t
            deq.EnqueueFirst(1);

            // +---+---+---+---+
            // | 1 | 2 |   |   |
            // +---+---+---+---+
            //   h   t
            deq.EnqueueLast(2);

            // The head index has wrapped around to the end of the array.

            // +---+---+---+---+
            // | 1 | 2 |   | 0 |
            // +---+---+---+---+
            //       t       h
            deq.EnqueueFirst(0);

            // +---+---+---+---+
            // | 1 | 2 | 3 | 0 |
            // +---+---+---+---+
            //           t   h
            deq.EnqueueLast(3);

            // When another item is added, the following will occur:
            //   1) The growth policy will define the size of the new array.
            //   2) The items will be copied from head to tail into the new array.
            //   3) The new item will be added.
            //       a. EnqueueFirst - The item is added at index 0 (the copy
            //                         operation leaves this open).
            //       b. EnqueueLast - The item is added to the end of the array.

            // +---+---+---+---+---+---+---+---+
            // | 0 | 1 | 2 | 3 | 4 |   |   |   |
            // +---+---+---+---+---+---+---+---+
            //   h               t
            deq.EnqueueLast(4);

            // +---+---+---+---+---+---+---+---+
            // |   | 1 | 2 | 3 | 4 |   |   |   |
            // +---+---+---+---+---+---+---+---+
            //       h           t
            deq.DequeueFirst();

            // +---+---+---+---+---+---+---+---+
            // |   | 1 | 2 | 3 |   |   |   |   |
            // +---+---+---+---+---+---+---+---+
            //       h       t
            deq.DequeueLast();
        }
    }
}