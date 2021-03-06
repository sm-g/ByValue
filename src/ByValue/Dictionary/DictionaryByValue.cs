﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ByValue
{
    internal class DictionaryByValue<TKey, TValue> : IEquatable<DictionaryByValue<TKey, TValue>>, ICollectionByValue
    {
        // common interface for IDictionary and IReadOnlyDictionary
        private readonly IEnumerable<KeyValuePair<TKey, TValue>>? _collection;

        private readonly int? _count;

        public DictionaryByValue(IDictionary<TKey, TValue>? dictionary, DictionaryOptions<TKey, TValue> options)
        {
            _collection = dictionary;
            _count = dictionary?.Count;

            options.EnsureIsNotDefault();
            Options = options;
        }

        public DictionaryByValue(IReadOnlyDictionary<TKey, TValue>? dictionary, DictionaryOptions<TKey, TValue> options)
        {
            _collection = dictionary;
            _count = dictionary?.Count;

            options.EnsureIsNotDefault();
            Options = options;
        }

        public DictionaryOptions<TKey, TValue> Options { get; }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((DictionaryByValue<TKey, TValue>)obj);
        }

        public bool Equals(DictionaryByValue<TKey, TValue> other)
        {
            if (other is null)
                return false;

            return MultiSetEqualityComparer.Equals(
                _collection,
                other._collection,
                Options.KeysEqualityComparer,
                Options.ValuesEqualityComparer);
        }

        public override int GetHashCode()
        {
            if (_collection is null || _count == 0)
                return 0;

            return _collection
                .Aggregate(17, (sum, value) =>
                {
                    unchecked
                    {
                        var keyHashCode = Options.KeysEqualityComparer.GetHashCode(value.Key);
                        return sum ^ (keyHashCode & 0x7FFFFFFF);
                    }
                });
        }

        public override string ToString()
        {
            if (_collection == null)
                return "<null> of KeyValuePair<" + typeof(TKey).GetFriendlyName() + "," + typeof(TValue).GetFriendlyName() + ">";

            return $"{_count}:{{{string.Join(",", _collection)}}}";
        }
    }
}