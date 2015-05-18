using System.Collections.Generic;
using System.Reflection;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> entities, string propertyName)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = entities.First().GetType().GetRuntimeProperty(propertyName);
            return entities.OrderBy(e => propertyInfo.GetValue(e, null));
        }

        public static IEnumerable<T> OrderByPropertyDescending<T>(this IEnumerable<T> entities, string propertyName)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = entities.First().GetType().GetRuntimeProperty(propertyName);
            return entities.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }
    }
}

