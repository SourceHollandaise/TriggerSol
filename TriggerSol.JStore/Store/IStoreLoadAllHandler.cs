using System;
using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public interface IStoreLoadAllHandler : IStoreBaseHandler
    {
        IEnumerable<IPersistentBase> LoadAllInternal(Type type);
    }
}
