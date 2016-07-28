//
// Session.cs
//
// Author:
//       Jörg Egger <joerg.egger@outlook.de>
//
// Copyright (c) 2015 Jörg Egger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    public class Session : DependencyObject, ISession, IDisposable
    {
        IDataStore _DataStore;
        protected IDataStore DataStore
        {
            get
            {
                if (_DataStore == null)
                    _DataStore = DependencyResolver.GetSingle<IDataStore>();
                return _DataStore;
            }
        }

        protected bool RollbackTransaction { get; set; }

        protected IList<IPersistentBase> ObjectsInTransaction = new List<IPersistentBase>();

        public Action<IPersistentBase> ObjectCommiting { get; set; }

        public Action<IPersistentBase> ObjectRollingback { get; set; }

        public T CreateObject<T>() where T: IPersistentBase => (T)CreateObject(typeof(T));
        
        public T LoadObject<T>(Func<T, bool> criteria) where T: IPersistentBase
        {
            if (!RollbackTransaction)
            {
                T storeObject = DataStore.Load(criteria);

                if (storeObject != null)
                {
                    storeObject.Session = this;

                    if (!ObjectsInTransaction.Contains(storeObject))
                        ObjectsInTransaction.Add(storeObject);

                    return storeObject;
                }

                T repoObject = ObjectsInTransaction.OfType<T>().FirstOrDefault(criteria);

                if (repoObject != null)
                    return repoObject;

                return default(T);
            }

            return default(T);
        }

        public void AddObject(IPersistentBase persistent)
        {
            if (!RollbackTransaction)
            {
                if (!ObjectsInTransaction.Contains(persistent))
                {
                    persistent.Session = this;
                    ObjectsInTransaction.Add(persistent);
                }
            }
        }

        public IList<IPersistentBase> GetObjects() => ObjectsInTransaction;
    
        public void RemoveObject(IPersistentBase persistent)
        {
            if (!RollbackTransaction)
            {
                if (ObjectsInTransaction.Contains(persistent))
                {
                    ObjectsInTransaction.Remove(persistent);
                    persistent.Session = null;
                    persistent = null;
                }
            }
        }

        public void Commit()
        {
            if (!RollbackTransaction)
            {
                foreach (var item in ObjectsInTransaction)
                {
                    ObjectCommiting?.Invoke(item);

                    var refTypes = item.GetType().GetRuntimeProperties().Where(p => p.FindAttribute<ReferenceAttribute>() != null);

                    foreach (var type in refTypes)
                    {
                        (type.GetValue(item) as IPersistentBase)?.Save();
                    }

                    item.Save();
                }
            }
        }

        public void Rollback()
        {
            RollbackTransaction = true;
            IList<IPersistentBase> reloadedObjects = new List<IPersistentBase>();

            object _storeLock = new object();

            lock (_storeLock)
            {
                foreach (var item in ObjectsInTransaction)
                {
                    ObjectRollingback?.Invoke(item);

                    IPersistentBase reloadedObject = null;

                    if (item.MappingId != null)
                        reloadedObject = item.Reload();
                    else
                        reloadedObject = CreateObject(item.GetType(), false);

                    if (reloadedObject != null)
                        reloadedObjects.Add(reloadedObject);
                }
            }
                
            ClearContainerCollection();

            ObjectsInTransaction = reloadedObjects;

            for (int i = reloadedObjects.Count - 1; i >= 0; i--)
                reloadedObjects[i] = null;

            reloadedObjects = null;

            RollbackTransaction = false;
        }

        protected IPersistentBase CreateObject(Type type, bool addToContainer = true)
        {
            if (!RollbackTransaction)
            {
                var instance = Activator.CreateInstance(type, this) as IPersistentBase;

                if (instance == null)
                    throw new ArgumentNullException("instance");

                instance.Initialize();

                if (addToContainer)
                {
                    if (!ObjectsInTransaction.Contains(instance))
                        ObjectsInTransaction.Add(instance);
                }

                return instance;
            }

            return null;
        }

        protected void ClearContainerCollection()
        {
            for (int i = ObjectsInTransaction.Count - 1; i >= 0; i--)
                ObjectsInTransaction[i] = null;

            ObjectsInTransaction.Clear();
        }

        bool disposed = false;

        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 

            if (disposing)
            {
                ClearContainerCollection();

                ObjectsInTransaction = null;
            }

            disposed = true;
        }
    }
}