using System;

namespace TriggerSol.JStore
{
    public interface IStoreDirectoryHandler : IStoreBaseHandler
    {
        string GetTypeDirectory(Type type);
    }
}
