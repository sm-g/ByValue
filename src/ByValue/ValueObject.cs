using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ByValue
{
    /// <remarks>
    /// Because it is not struct, there is no need of <see cref="System.IEquatable{T}"/>.
    /// See https://stackoverflow.com/questions/2476793/when-to-use-iequatable-and-why for details.
    /// (Also we never will have overload resolution problems: http://blog.mischel.com/2013/01/05/inheritance-and-iequatable-do-not-mix/).
    /// </remarks>
    public abstract class ValueObject
    {
#pragma warning disable S3875 // "operator==" should not be overloaded on reference types

        public static bool operator ==(ValueObject x, ValueObject y)
        {
            if (x is null)
                return y is null;
            return x.Equals(y);
        }

        public static bool operator !=(ValueObject x, ValueObject y) => !(x == y);

#pragma warning restore S3875 // "operator==" should not be overloaded on reference types

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;

            var other = obj as ValueObject;
            return Reflect().SequenceEqual(other!.Reflect());
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return Reflect().Aggregate(17, (sum, value) =>
            {
                unchecked
                {
                    return (sum * 23) + (value != null ? value.GetHashCode() : 0);
                }
            });
        }

        /// <summary>
        /// Returns object view like "{1,string,true}" (for better logging, debugging, etc.).
        /// <para/>
        /// If you want default behavior, override it: <c>public override string ToString() => GetType().ToString();</c>.
        /// </summary>
        [DebuggerStepThrough]
        public override string ToString()
        {
            using (var e = Reflect().GetEnumerator())
            {
                if (!e.MoveNext())
                    return "{}";

                var sb = new StringBuilder("{");
                sb.Append(e.Current);
                while (e.MoveNext())
                {
                    sb.Append(",");
                    sb.Append(e.Current);
                }
                sb.Append("}");
                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns all values to be used in Equals and GetHashCode.
        /// </summary>
        protected abstract IEnumerable<object> Reflect();
    }
}