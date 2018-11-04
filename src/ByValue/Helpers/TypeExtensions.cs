using System;
using System.Reflection;
using System.Text;

namespace ByValue
{
    internal static class TypeExtensions
    {
        public static string GetFriendlyName(this Type type)
        {
            return GetFriendlyNameInner(type, t => t.Name);
        }

        public static string GetFriendlyFullName(this Type type)
        {
            return GetFriendlyNameInner(type, t => t.FullName);
        }

        private static string GetFriendlyNameInner(Type type, Func<Type, string> nameSelector)
        {
            // GetTypeInfo() not need for net461

            if (type.GetTypeInfo().IsGenericType)
            {
                // always use Name for generics, it is probably collection type
                var genericName = type.Name;
                var iBacktick = genericName.IndexOf('`');
                if (iBacktick > 0)
                {
                    genericName = genericName.Remove(iBacktick);
                }

                var builder = new StringBuilder();
                builder.Append(genericName);
                builder.Append("<");
                var typeArguments = type.GetTypeInfo().GenericTypeArguments;
                for (var i = 0; i < typeArguments.Length; ++i)
                {
                    var typeArgumentName = GetFriendlyNameInner(typeArguments[i], nameSelector);
                    builder.Append(i == 0 ? typeArgumentName : "," + typeArgumentName);
                }
                builder.Append(">");
                return builder.ToString();
            }

            return nameSelector(type);
        }
    }
}
