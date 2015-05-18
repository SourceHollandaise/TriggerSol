using System.Collections.Generic;
using System.Reflection;

namespace System
{
    public static class FuncExtensions
    {
        public enum GroupType
        {
            And,
            Or
        }

        public static Func<T, bool> AndAlso<T>(this Func<T, bool> func1, Func<T, bool> func2)
        {
            if (func1 == null)
                throw new ArgumentNullException("func1");

            if (func2 == null)
                throw new ArgumentNullException("func2");

            return a => func1(a) && func2(a);
        }

        public static Func<T, bool> OrElse<T>(this Func<T, bool> func1, Func<T, bool> func2)
        {
            if (func1 == null)
                throw new ArgumentNullException("func1");

            if (func2 == null)
                throw new ArgumentNullException("func2");

            return a => func1(a) || func2(a);
        }

        public static Func<T, bool> Grouped<T>(Func<T, bool>[] functions, GroupType groupType = GroupType.Or)
        {
            if (functions == null)
                throw new ArgumentNullException("functions");

            if (functions.Length == 0)
                throw new ArgumentOutOfRangeException("functions");

            Func<T, bool> result = a => groupType == GroupType.And;

            if (groupType == GroupType.And)
            {
                foreach (var func in functions)
                {
                    result = result.AndAlso(func);
                }
            }

            if (groupType == GroupType.Or)
            {
                foreach (var func in functions)
                {
                    result = result.OrElse(func);
                }
            }

            return result;
        }
    }
}
   
