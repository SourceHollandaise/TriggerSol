using System;
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    public static class DataStoreProvider
    {
        public static IDataStore DataStore
        {
            get
            {
                var store = TypeProvider.Current.GetSingle<IDataStore>();
                if (store == null)
                    throw new ArgumentNullException("Store", "No datastore registered!");

                return store;
            }
        }

        public static void ExplicitCaching<T>() where T: IPersistentBase
        {
            CacheProvider.StartCaching<T>();
        }

        public static void ExplicitCaching(Type type)
        {
            CacheProvider.StartCaching(type);
        }

        public static void RegisterStore<T>() where T: IDataStore, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStore>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStore>(instance);
            }
        }

        public static void RegisterDirectoryHandler<T>() where T: IDataStoreDirectoryHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreDirectoryHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreDirectoryHandler>(instance);
            }
        }

        public static void RegisterDeleteHandler<T>() where T: IDataStoreDeleteHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreDeleteHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreDeleteHandler>(instance);
            }
        }

        public static void RegisterSaveHandler<T>() where T: IDataStoreSaveHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreSaveHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreSaveHandler>(instance);
            }
        }

        public static void RegisterLoadHandler<T>() where T: IDataStoreLoadHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreLoadHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreLoadHandler>(instance);
            }
        }

        public static void RegisterLoadAllHandler<T>() where T: IDataStoreLoadAllHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreLoadAllHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreLoadAllHandler>(instance);
            }
        }
    }
}