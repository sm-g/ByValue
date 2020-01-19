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

            comparer ??= EqualityComparer<T>.Default;

            var itemsCount = new Dictionary<T, int>(comparer);
            var nullsCount = 0;
            foreach (var item in first)
            {
                if (item is null)
                {
                    nullsCount++;
                }
                else
                {
                    Inc(itemsCount, item);
                }
            }
            foreach (var item in second)
            {
                if (item is null)
                {
                    nullsCount--;
                }
                else
                {
                    if (!Dec(itemsCount, item))
                    {
                        return false;
                    }
                }
            }

            return nullsCount == 0 &&
                   itemsCount.Values.All(count => count == 0);
        }

        public static bool Equals<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> first,
            IEnumerable<KeyValuePair<TKey, TValue>> second,
            IEqualityComparer<TKey> keysComparer = null,
            IEqualityComparer<TValue> valuesComparer = null)
        {
            var byPreconditions = EqualsByPreconditions(first, second);
            if (byPreconditions.HasValue)
                return byPreconditions.Value;

            keysComparer ??= EqualityComparer<TKey>.Default;
            valuesComparer ??= EqualityComparer<TValue>.Default;

            // key is always not null in dictionary
            var itemsCount = new Dictionary<TKey, Dictionary<TValue, int>>(keysComparer);
            var nullValuesCount = new Dictionary<TKey, int>(keysComparer);
            foreach (var item in first)
            {
                if (itemsCount.ContainsKey(item.Key))
                {
                    Inc(itemsCount[item.Key], item.Value);
                }
                else
                {
                    if (item.Value is null)
                    {
                        Inc(nullValuesCount, item.Key);
                    }
                    else
                    {
                        itemsCount[item.Key] = new Dictionary<TValue, int>(valuesComparer)
                        {
                            [item.Value] = 1
                        };
                    }
                }
            }

            foreach (var item in second)
            {
                if (item.Value is null)
                {
                    if (!Dec(nullValuesCount, item.Key))
                    {
                        return false;
                    }
                }
                else if (itemsCount.ContainsKey(item.Key))
                {
                    if (!Dec(itemsCount[item.Key], item.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return nullValuesCount.Values.All(count => count == 0) &&
                   itemsCount.Values.All(dict => dict.Values.All(count => count == 0));
        }

        private static void Inc<T>(Dictionary<T, int> itemsCount, T item)
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

        private static bool Dec<T>(Dictionary<T, int> itemsCount, T item)
        {
            if (itemsCount.ContainsKey(item))
            {
                itemsCount[item]--;
                return true;
            }
            return false;
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