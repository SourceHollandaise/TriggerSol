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

        public static void RegisterDirectoryHandler<T>() where T: IStoreDirectoryHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IStoreDirectoryHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IStoreDirectoryHandler>(instance);
            }
        }

        public static void RegisterDeleteHandler<T>() where T: IStoreDeleteHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IStoreDeleteHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IStoreDeleteHandler>(instance);
            }
        }

        public static void RegisterSaveHandler<T>() where T: IStoreSaveHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IStoreSaveHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IStoreSaveHandler>(instance);
            }
        }

        public static void RegisterLoadHandler<T>() where T: IStoreLoadHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IStoreLoadHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IStoreLoadHandler>(instance);
            }
        }

        public static void RegisterLoadAllHandler<T>() where T: IStoreLoadAllHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IStoreLoadAllHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IStoreLoadAllHandler>(instance);
            }
        }
    }
}