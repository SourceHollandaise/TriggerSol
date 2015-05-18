using System;
using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public interface ITransaction
    {
        Action<IPersistentBase> ObjectCommiting { get; set; }

        Action<IPersistentBase> ObjectRollback { get; set; }

        T CreateObject<T>() where T: IPersistentBase;

        T LoadObject<T>(Func<T, bool> criteria) where T: IPersistentBase;

        IList<IPersistentBase> GetObjects();

        void AddTo(IPersistentBase persistent);

        void RemoveFrom(IPersistentBase persistent);

        void Commit();

        void Rollback();
    }
}