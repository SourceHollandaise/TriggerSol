using System;
using System.Collections.Generic;
using System.Linq;
using TriggerSol.Dependency;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class JsonFileDataStore : DependencyObject, IDataStore
    {
        public JsonFileDataStore()
        {
            
        }

        public void Save(Type type, IPersistentBase item)
        {
            SaveInternal(type, item);
        }

        public void Save<T>(T item) where T: IPersistentBase
        {
            SaveInternal(typeof(T), item);
        }

        public void DeleteById(Type type, object itemId)
        {
            DeleteInternal(type, itemId);
        }

        public void DeleteById<T>(object itemId) where T: IPersistentBase
        {
            DeleteInternal(typeof(T), itemId);
        }

        public void Delete(Type type, IPersistentBase item)
        {
            DeleteInternal(type, item.MappingId);
        }

        public void Delete<T>(T item) where T: IPersistentBase
        {
            DeleteInternal(typeof(T), item.MappingId);
        }

        public void Delete<T>(Func<T, bool> criteria) where T : IPersistentBase
        {
            foreach (var item in LoadAll<T>().Where(criteria).ToList())
                DeleteInternal(typeof(T), item.MappingId);
        }

        public void Delete(Type type, Func<IPersistentBase, bool> criteria)
        {
            foreach (var item in LoadAll(type).Where(criteria).ToList())
                DeleteInternal(type, item.MappingId);
        }

        public IPersistentBase Load(Type type, object itemId)
        {
            return LoadInternal(type, itemId);
        }

        public T Load<T>(object itemId) where T: IPersistentBase
        {
            return (T)LoadInternal(typeof(T), itemId);
        }

        public T Load<T>(Func<T, bool> criteria) where T : IPersistentBase
        {
            return LoadAllInternal(typeof(T)).OfType<T>().FirstOrDefault(criteria);
        }

        public IEnumerable<IPersistentBase> LoadAll(Type type)
        {
            return LoadAllInternal(type);
        }

        public IEnumerable<T> LoadAll<T>() where T: IPersistentBase
        {
            return LoadAllInternal(typeof(T)).OfType<T>();
        }

        public IEnumerable<T> LoadAll<T>(Func<T, bool> criteria) where T: IPersistentBase
        {
            return LoadAllInternal(typeof(T)).OfType<T>().Where(criteria);
        }

        internal protected virtual void SaveInternal(Type type, IPersistentBase item)
        {
            TypeResolver.GetSingle<IDataStoreSaveHandler>().SaveInternal(type, item);
        }

        internal protected virtual void DeleteInternal(Type type, object mappingId)
        {
            TypeResolver.GetSingle<IDataStoreDeleteHandler>().DeleteInternal(type, mappingId);
        }

        internal protected virtual IEnumerable<IPersistentBase> LoadAllInternal(Type type)
        {
            return TypeResolver.GetSingle<IDataStoreLoadAllHandler>().LoadAllInternal(type);
        }

        internal protected virtual IPersistentBase LoadInternal(Type type, object mappingId)
        {
            return TypeResolver.GetSingle<IDataStoreLoadHandler>().LoadInternal(type, mappingId);
        }

        internal protected virtual string GetTargetLocation(Type type)
        {
            return TypeResolver.GetSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);
        }

        public IEnumerable<IPersistentBase> InitializeAll(Type type)
        {
            return TypeResolver.GetSingle<IDataStoreLoadAllHandler>().LoadAllInternal(type);
        }
    }
}