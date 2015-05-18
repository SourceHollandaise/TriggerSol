using TriggerSol.Dependency;
using TriggerSol.JStore;

namespace TriggerSol.JStore
{
    public static class DataStoreManager
    {
        public static void RegisterStore<T>() where T: IDataStore, new()
        {
            DataStoreProvider.RegisterStore<T>();
            DataStoreProvider.RegisterDeleteHandler<StoreDeleteHandler>();
            DataStoreProvider.RegisterDirectoryHandler<StoreDirectoryHandler>();
            DataStoreProvider.RegisterLoadAllHandler<StoreLoadAllHandler>();
            DataStoreProvider.RegisterLoadHandler<StoreLoadHandler>();
            DataStoreProvider.RegisterSaveHandler<StoreSaveHandler>();
        }
    }
}