using System;

namespace TriggerSol.JStore
{
    public interface IDataStoreDeleteHandler : IDataStoreExecutionHandlerBase
    {
        void DeleteInternal(Type type, object mappingId);
    }
}
