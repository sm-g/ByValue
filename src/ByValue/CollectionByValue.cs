using System;
using System.Collections.Generic;
using System.Linq;

namespace ByValue
{
    internal class CollectionByValue<T> : IEquatable<CollectionByValue<T>>, ICollectionByValue
    {
        private readonly IReadOnlyCollection<T> _collection;

        public CollectionByValue(IReadOnlyCollection<T> collection, Options<T> options)
        {
            _collection = collection;
            Options = options;
        }

        public Options<T> Options { get; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((CollectionByValue<T>)obj);
        }

        public bool Equals(CollectionByValue<T> other)
        {
            if (other is null)
                return false;

            var inOrder = Options.InOrder || other.Options.InOrder;
            if (inOrder)
            {
                if (_collection is null)
                    return other._collection is null;
                if (other._collection is null)
                    return false;

                if (_collection.Count != other._collection.Count)
                    return false;

                return _collection.SequenceEqual(other._collection, Options.EqualityComparer);
            }
            else
            {
                return MultiSetEqualityComparer.Equals(_collection, other._collection, Options.EqualityComparer);
            }
        }

        public override int GetHashCode()
        {
            if (_collection is null || _collection.Count == 0)
                return 0;

            // disable "var inOrder = InOrder || other.InOrder;" to be able to use this:
            //
            //if (InOrder)
            //{
            //    // struct-like hashing
            //    var firstItemHash = _collection.FirstOrDefault()?.GetHashCode() ?? 0;
            //    return _collection.Count ^ firstItemHash;
            //}

            return _collection.OrderBy(x => x).Aggregate(17, (sum, value) =>
            {
                unchecked
                {
                    return (sum * 23) + (value != null ? Options.EqualityComparer.GetHashCode(value) : 0);
                }
            });
        }

        public override string ToString()
        {
            if (_collection == null)
                return "<null> of " + typeof(T).GetFriendlyName();

            // maybe use {} for NotStrict
            return $"{_collection.Count}:[{string.Join(",", _collection)}]";
        }
    }
}