using System;
using System.Collections.Generic;

namespace ByValue
{
    public static class CollectionExtensions
    {
        public static ICollectionByValue ByValue<T>(this IReadOnlyCollection<T> collection, Ordering ordering)
        {
            return new CollectionByValue<T>(collection, new Options<T>(ordering == Ordering.Strict, null));
        }

        public static ICollectionByValue ByValue<T>(this IReadOnlyCollection<T> collection)
        {
            return new CollectionByValue<T>(collection, default);
        }

        public static ICollectionByValue ByValue<T>(this IReadOnlyCollection<T> collection, Action<OptionsBuilder<T>> optionsAction)
        {
            var builder = new OptionsBuilder<T>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new CollectionByValue<T>(collection, options);
        }

        // TODO support CollectionByValue of ISet

        ////public static ICollectionByValue ByValue<T>(this ISet<T> collection)
        ////{
        ////    return new CollectionByValue<T>(collection, ordering);
        ////}

        public static ICollectionByValue ByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new DictionaryByValue<TKey, TValue>(dictionary, default);
        }

        public static ICollectionByValue ByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<DictionaryOptionsBuilder<TKey, TValue>> optionsAction)
        {
            var builder = new DictionaryOptionsBuilder<TKey, TValue>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new DictionaryByValue<TKey, TValue>(dictionary, options);
        }

        public static ICollectionByValue ByValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            return new DictionaryByValue<TKey, TValue>(dictionary, default);
        }

        public static ICollectionByValue ByValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, Action<DictionaryOptionsBuilder<TKey, TValue>> optionsAction)
        {
            var builder = new DictionaryOptionsBuilder<TKey, TValue>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new DictionaryByValue<TKey, TValue>(dictionary, options);
        }
    }
}