using System;

namespace TriggerSol.JStore
{
    public interface IDataStoreDirectoryHandler : IDataStoreExecutionHandlerBase
    {
        string GetTypeDirectory(Type type);
    }
}
