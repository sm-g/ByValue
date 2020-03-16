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
            return new CollectionByValue<T>(collection, new Options<T>(false, null));
        }

        public static ICollectionByValue ByValue<T>(
            this IReadOnlyCollection<T> collection,
            Action<OptionsBuilder<T>> optionsAction)
        {
            var builder = new OptionsBuilder<T>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new CollectionByValue<T>(collection, options);
        }

        public static ICollectionByValue ByValue<T>(this ISet<T> collection)
        {
            return new SetByValue<T>(collection, new Options<T>(false, null));
        }

        public static ICollectionByValue ByValue<T>(
            this ISet<T> collection,
            Action<SetOptionsBuilder<T>> optionsAction)
        {
            var builder = new SetOptionsBuilder<T>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new SetByValue<T>(collection, options);
        }

        public static ICollectionByValue ByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new DictionaryByValue<TKey, TValue>(dictionary, new DictionaryOptions<TKey, TValue>(null, null));
        }

        public static ICollectionByValue ByValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Action<DictionaryOptionsBuilder<TKey, TValue>> optionsAction)
        {
            var builder = new DictionaryOptionsBuilder<TKey, TValue>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new DictionaryByValue<TKey, TValue>(dictionary, options);
        }

        public static ICollectionByValue ByValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            return new DictionaryByValue<TKey, TValue>(dictionary, new DictionaryOptions<TKey, TValue>(null, null));
        }

        public static ICollectionByValue ByValue<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            Action<DictionaryOptionsBuilder<TKey, TValue>> optionsAction)
        {
            var builder = new DictionaryOptionsBuilder<TKey, TValue>();
            optionsAction.Invoke(builder);
            var options = builder.Build();
            return new DictionaryByValue<TKey, TValue>(dictionary, options);
        }
    }
}