using System;

namespace TriggerSol.JStore
{
    public interface IDataStoreLoadHandler : IDataStoreExecutionHandlerBase
    {
        IPersistentBase LoadInternal(Type type, object mappingId);
    }
}
