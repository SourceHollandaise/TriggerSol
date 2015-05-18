using System;

namespace TriggerSol.JStore
{
    public interface IStoreDeleteHandler : IStoreBaseHandler
    {
        void DeleteInternal(Type type, object itemId);
    }
}
