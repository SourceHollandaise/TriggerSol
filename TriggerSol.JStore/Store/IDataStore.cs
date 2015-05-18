using System.Collections.Generic;
using System;

namespace TriggerSol.JStore
{
    public interface IDataStore
    {
        void Save(Type type, IPersistentBase item);

        void Save<T>(T item) where T: IPersistentBase;

        void DeleteById(Type type, object itemId);

        void DeleteById<T>(object itemId) where T: IPersistentBase;

        void Delete(Type type, IPersistentBase item);

        void Delete<T>(T item) where T: IPersistentBase;

        void Delete<T>(Func<T, bool> criteria) where T: IPersistentBase;

        void Delete(Type type, Func<IPersistentBase, bool> criteria);

        IPersistentBase Load(Type type, object itemId);

        T Load<T>(object itemId) where T: IPersistentBase;

        T Load<T>(Func<T, bool> criteria) where T: IPersistentBase;

        IEnumerable<IPersistentBase>LoadAll(Type type);

        IEnumerable<T> LoadAll<T>() where T: IPersistentBase;

        IEnumerable<T> LoadAll<T>(Func<T, bool> criteria) where T: IPersistentBase;

        IEnumerable<IPersistentBase> InitializeAll(Type type);
    }
}
