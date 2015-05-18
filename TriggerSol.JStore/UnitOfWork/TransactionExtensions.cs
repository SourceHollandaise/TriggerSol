using System;
using System.Linq;
using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public static class TransactionExtensions
    {
        public static T FindObject<T>(this ITransaction transaction, Func<T, bool> criteria) where T: IPersistentBase
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction", "Transaction is null!");

            return transaction.GetObjects().OfType<T>().FirstOrDefault(criteria);
        }

        public static IEnumerable<T> FindObjects<T>(this ITransaction transaction, Func<T, bool> criteria) where T: IPersistentBase
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction", "Transaction is null!");

            return transaction.GetObjects().OfType<T>().Where(criteria);
        }
    }
}