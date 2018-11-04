using System.Collections.Generic;
using System.Linq;

namespace ByValue
{
    internal static class MultiSetEqualityComparer
    {
        public static bool Equals<T>(IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer = null)
        {
            var byPreconditions = EqualsByPreconditions(first, second);
            if (byPreconditions.HasValue)
                return byPreconditions.Value;

            comparer = comparer ?? EqualityComparer<T>.Default;

            var itemsCount = new Dictionary<T, int>(comparer);
            int firstNullsCount = 0, secondNullsCount = 0;
            foreach (var item in first)
            {
                if (item == null)
                {
                    firstNullsCount++;
                }
                else
                {
                    if (itemsCount.ContainsKey(item))
                    {
                        itemsCount[item]++;
                    }
                    else
                    {
                        itemsCount[item] = 1;
                    }
                }
            }
            foreach (var item in second)
            {
                if (item == null)
                {
                    secondNullsCount++;
                }
                else
                {
                    if (itemsCount.ContainsKey(item))
                    {
                        itemsCount[item]--;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return firstNullsCount == secondNullsCount &&
                   itemsCount.Values.All(c => c == 0);
        }

        private static bool? EqualsByPreconditions<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first is null)
                return second is null;
            if (second is null)
                return false;

            if (ReferenceEquals(first, second))
                return true;

            if (first is ICollection<T> firstCollection && second is ICollection<T> secondCollection)
            {
                if (firstCollection.Count != secondCollection.Count)
                    return false;

                if (firstCollection.Count == 0)
                    return true;
            }

            return null;
        }
    }
}