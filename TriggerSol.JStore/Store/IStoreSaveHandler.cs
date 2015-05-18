using System;

namespace TriggerSol.JStore
{
    public interface IStoreSaveHandler : IStoreBaseHandler
    {
        void SaveInternal(Type type, IPersistentBase item);
    }
}
