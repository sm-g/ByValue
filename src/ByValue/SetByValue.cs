using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ByValue
{
    internal class SetByValue<T> : IEquatable<SetByValue<T>>, ICollectionByValue
    {
        private readonly ISet<T> _set;

        public SetByValue(ISet<T> set, Options<T> options)
        {
            _set = set;

            Debug.Assert(!options.Equals(default), "default options struct");
            Options = options;
        }

        public Options<T> Options { get; }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((SetByValue<T>)obj);
        }

        public bool Equals(SetByValue<T> other)
        {
            // do not use ISet.SetEquals to compare with EqualityComparer from Options
            return MultiSetEqualityComparer.Equals(_set, other._set, Options.EqualityComparer);
        }

        public override int GetHashCode()
        {
            if (_set is null || _set.Count == 0)
                return 0;

            return _set
                .Aggregate(17, (sum, value) =>
                {
                    unchecked
                    {
                        var itemHashCode = value != null ? Options.EqualityComparer.GetHashCode(value) : 0;
                        return sum ^ (itemHashCode & 0x7FFFFFFF);
                    }
                });
        }

        public override string ToString()
        {
            if (_set == null)
                return "<null> of " + typeof(T).GetFriendlyName();

            // maybe use {} for NotStrict
            return $"{_set.Count}:[{string.Join(",", _set)}]";
        }
    }
}