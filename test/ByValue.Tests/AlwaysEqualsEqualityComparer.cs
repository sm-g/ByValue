using System.Collections.Generic;

namespace ByValue
{
    public class AlwaysEqualsEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return true;
        }

        public int GetHashCode(T obj)
        {
            return 1;
        }
    }
}