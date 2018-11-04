using System.Collections.Generic;

namespace ByValue
{
    public static class CollectionExtensions
    {
        public static ICollectionByValue ByValue<T>(this IReadOnlyCollection<T> collection, Ordering ordering)
        {
            return new CollectionByValue<T>(collection, ordering);
        }

        // TODO support CollectionByValue of ISet

        //public static ICollectionByValue ByValue<T>(this ISet<T> collection)
        //{
        //    return new CollectionByValue<T>(collection, ordering);
        //}

        public static ICollectionByValue ByValue<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            return new DictionaryByValue<TKey, TValue>(collection);
        }

        public static ICollectionByValue ByValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict)
        {
            return new DictionaryByValue<TKey, TValue>(dict);
        }

        // TODO support CollectionByValue with custom IEqualityComparer

        //public static ICollectionByValue WithComparer<T>(this ICollectionByValue collectionByValue, IEqualityComparer<T> comparer)
        //{
        //    return collectionByValue;
        //}
    }
}