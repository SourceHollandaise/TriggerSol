using System.Linq;
using System.Reflection;

namespace System
{
    public static class ReflectionExtensions
    {
        public static T FindAttribute<T>(this Type type) where T: Attribute
        {
            var attribute = type.GetTypeInfo().GetCustomAttribute<T>();
            if (attribute != null)
                return (T)attribute;
            return null;
        }

        public static T FindAttribute<T>(this PropertyInfo info) where T: Attribute
        {
            var attribute = info.GetCustomAttributes(typeof(T), true).FirstOrDefault();
            if (attribute != null)
                return (T)attribute;
            return null;
        }
    }
}
	