using System.Collections.Generic;

namespace ByValue
{
    public readonly struct DictionaryOptions<TKey, TValue>
    {
        public DictionaryOptions(IEqualityComparer<TKey>? keysEqualityComparer, IEqualityComparer<TValue>? valuesEqualityComparer)
        {
            KeysEqualityComparer = keysEqualityComparer ?? EqualityComparer<TKey>.Default;
            ValuesEqualityComparer = valuesEqualityComparer ?? EqualityComparer<TValue>.Default;
        }

        public IEqualityComparer<TKey> KeysEqualityComparer { get; }
        public IEqualityComparer<TValue> ValuesEqualityComparer { get; }
    }
}