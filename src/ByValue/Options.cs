using System.Collections.Generic;

namespace ByValue
{
    public readonly struct Options<T>
    {
        public Options(bool inOrder, IEqualityComparer<T> equalityComparer)
        {
            InOrder = inOrder;
            EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public bool InOrder { get; }

        public IEqualityComparer<T> EqualityComparer { get; }
    }
}