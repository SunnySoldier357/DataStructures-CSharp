using System;

namespace SetClass
{
    class Program
    {
        static void Main(string[] args)
        {
            // ~~~~~~~~~~~~~~~~
            // ~~ Algorithms ~~
            // ~~~~~~~~~~~~~~~~

            Set<int> first = new Set<int>() { 1, 2, 3, 4 };
            Set<int> second = new Set<int>() { 3, 4, 5 , 6 };

            // Union

            // Union of 2 sets is a set that contains all of the distinct items
            // that exist in both sets.

            // [1, 2, 3, 4, 5, 6]
            Set<int> union = first.Union(second);

            // Intersection

            // Intersection is the point at which two sets have common members.

            // [3, 5]
            Set<int> intersect = first.Intersection(second);

            // Difference

            // The difference between two sets is the items that exist in the
            // first set, but do not exist in the second set.

            // [1, 2]
            Set<int> difference = first.Difference(second);

            // Symmetric Difference

            // The symmetric difference of two sets is a set whose members are
            // those items which exist in only one or the other set.

            // [1, 2, 5, 6]
            Set<int> symDifference = first.SymmetricDifference(second);
        }
    }
}