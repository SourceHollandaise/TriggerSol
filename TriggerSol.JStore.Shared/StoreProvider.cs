using TriggerSol.Dependency;
using TriggerSol.JStore;

namespace TriggerSol.JStore
{
    public static class DataStoreManager
    {
        public static void RegisterStore<T>() where T: IDataStore, new()
        {
            DataStoreProvider.RegisterStore<T>();
            DataStoreProvider.RegisterDeleteHandler<DataStoreDeleteHandler>();
            DataStoreProvider.RegisterDirectoryHandler<DataStoreDirectoryHandler>();
            DataStoreProvider.RegisterLoadAllHandler<DataStoreLoadAllHandler>();
            DataStoreProvider.RegisterLoadHandler<DataStoreLoadHandler>();
            DataStoreProvider.RegisterSaveHandler<DataStoreSaveHandler>();
        }
    }
}