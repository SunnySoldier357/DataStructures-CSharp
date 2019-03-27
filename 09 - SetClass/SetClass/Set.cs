using System;
using System.Collections;
using System.Collections.Generic;

namespace SetClass
{
    public class Set<T> : IEnumerable<T>
        where T : IComparable<T>
    {
        //* Private Properties
        private readonly List<T> _items = new List<T>();

        //* Public Properties

        /// <summary>
        /// <para>
        /// Returns the number of items in the set or 0 if the set is empty.
        /// </para>
        /// <para>
        /// Performance: O(1)
        /// </para>
        /// </summary>
        public int Count => _items.Count;

        //* Constructors
        public Set() { }

        public Set(IEnumerable<T> items) => AddRange(items);

        //* Public Methods

        /// <summary>
        /// <para>
        /// Adds the item to the set. If the item already exists in the set, an
        /// <see cref="InvalidOperationException"/> is thrown.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="InvalidOperationException"/>
        public void Add(T item)
        {
            if (Contains(item))
                throw new InvalidOperationException("Item already exists in Set");

            _items.Add(item);
        }

        /// <summary>
        /// <para>
        /// Adds multiple items to the set. If any member of the input enumerator
        /// exists in the set, or if there are duplicate items in the input
        /// enumerator, an <see cref="InvalidOperationException"/> will be thrown.
        /// </para>
        /// <para>
        /// Performance: O(mn), where m is the number of items in the input
        /// enumeration and n is the number of items currently in the set.
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        /// <exception cref="InvalidOperationException"/>
        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);
        }

        /// <summary>
        /// <para>
        /// Returns <see langword="true"/> if the set contains the specified
        /// value. Otherwise it returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
            => _items.Contains(item);

        /// <summary>
        /// <para>
        /// Removes the specified value from the set if found, return
        /// <see langword="true"/>. If the set does not contain the specified
        /// value, <see langword="false"/> is returned.
        /// </para>
        /// <para>
        /// Performance: O(n)
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) =>
            _items.Remove(item);

        /// <summary>
        /// <para>
        /// Returns a new set that is the result of the union operator of the
        /// current and input set.
        /// </para>
        /// <para>
        /// Performance: O(mn), where m and n are the number of items in the
        /// provided and current sets, respectively.
        /// </para>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Set<T> Union(Set<T> other)
        {
            Set<T> result = new Set<T>(_items);

            foreach (T item in other._items)
            {
                if (!Contains(item))
                    result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// <para>
        /// Returns a new set that is the result of the intersection operation
        /// of the current and input sets.
        /// </para>
        /// <para>
        /// Performance: O(mn), where m and n are the number of items in the
        /// provided and current sets, respectively.
        /// </para>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Set<T> Intersection(Set<T> other)
        {
            Set<T> result = new Set<T>();

            foreach (T item in _items)
            {
                if (other._items.Contains(item))
                    result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// <para>
        /// Returns a new set that is the result of the difference operation
        /// of the current and input sets.
        /// </para>
        /// <para>
        /// Performance: O(mn), where m and n are the number of items in the
        /// provided and current sets, respectively.
        /// </para>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>sa
        public Set<T> Difference(Set<T> other)
        {
            Set<T> result = new Set<T>(_items);

            foreach (T item in other._items)
                result.Remove(item);

            return result;
        }

        /// <summary>
        /// <para>
        /// Returns a new set that is the result of the symmetric difference
        /// operation of the current and input sets.
        /// </para>
        /// <para>
        /// Performance: O(mn), where m and n are the number of items in the
        /// provided and current sets, respectively.
        /// </para>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Set<T> SymmetricDifference(Set<T> other)
        {
            Set<T> union = Union(other);
            Set<T> intersection = Intersection(other);

            return union.Difference(intersection);
        }

        /// <summary>
        /// <para>
        /// Returns an enumerator for all items in the set.
        /// </para>
        /// <para>
        /// Performance: O(1) to return the enumerator. Enumerating all the items
        /// has a complexity of O(n).
        /// </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() =>
            _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            _items.GetEnumerator();
    }
}