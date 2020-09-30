using System;
using System.Diagnostics;

namespace ByValue
{
    /// <remarks>
    /// Not using struct for such values because every struct could be created
    /// with default values, which often invalid in domain.
    /// <para/>
    /// <see cref="IComparable"/> need because such types usually implement VO semantic and have overridden Equals.
    /// Without it we have to provide custom equality comparer for <typeparamref name="T"/>.
    /// </remarks>
    public abstract class SingleValueObject<T>
        where T : IComparable
    {
        public T Value { get; }

        protected SingleValueObject(T value)
        {
            Value = value;
        }

#pragma warning disable S3875 // "operator==" should not be overloaded on reference types

        public static bool operator ==(SingleValueObject<T> x, SingleValueObject<T> y)
        {
            if (x is null)
                return y is null;
            return x.Equals(y);
        }

        public static bool operator !=(SingleValueObject<T> x, SingleValueObject<T> y) => !(x == y);

#pragma warning restore S3875 // "operator==" should not be overloaded on reference types

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;

            var other = obj as SingleValueObject<T>;
            if (Value is null)
                return other!.Value is null;

            return Value.Equals(other!.Value);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }
    }
}