using System;

namespace TriggerSol.JStore
{
    public interface IDataStoreSaveHandler : IDataStoreExecutionHandlerBase
    {
        void SaveInternal(Type type, IPersistentBase persistent);
    }
}
