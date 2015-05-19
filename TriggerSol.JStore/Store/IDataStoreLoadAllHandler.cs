using System;
using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public interface IDataStoreLoadAllHandler : IDataStoreExecutionHandlerBase
    {
        IEnumerable<IPersistentBase> LoadAllInternal(Type type);
    }
}
