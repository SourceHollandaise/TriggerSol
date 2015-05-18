using System.Collections.Generic;
using TriggerSol.JStore;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static void AddToTransaction(this IEnumerable<IPersistentBase> persistents, ITransaction transaction)
        {
            foreach (var persistent in persistents)
            {
                transaction.AddTo(persistent);
            }
        }
    }
}