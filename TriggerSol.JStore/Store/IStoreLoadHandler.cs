using System;

namespace TriggerSol.JStore
{
    public interface IStoreLoadHandler : IStoreBaseHandler
    {
        IPersistentBase LoadInternal(Type type, object itemId);
    }
}
