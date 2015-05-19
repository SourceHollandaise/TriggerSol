using System.Linq.Expressions;

namespace System
{
    public static class Fields<TClass>
    {
        public static string GetName<TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
                return memberExpression.Member.Name;

            throw new InvalidOperationException("Member expression expected");
        }
    }
}
	