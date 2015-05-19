//
// Transaction.cs
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
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    public class Transaction : DependencyObject, ITransaction, IDisposable
    {
        bool _rollbackTransaction { get; set; }

        IList<IPersistentBase> _persistents = new List<IPersistentBase>();

        public Action<IPersistentBase> ObjectCommiting { get; set; }

        public Action<IPersistentBase> ObjectRollback { get; set; }

        public T CreateObject<T>() where T: IPersistentBase
        {
            return (T)CreateObject(typeof(T));
        }

        protected IPersistentBase CreateObject(Type type, bool addToContainer = true)
        {
            if (!_rollbackTransaction)
            {
                var instance = Activator.CreateInstance(type) as IPersistentBase;

                if (instance == null)
                    throw new ArgumentNullException("instance");

                instance.Initialize();

                if (addToContainer)
                {
                    if (!_persistents.Contains(instance))
                        _persistents.Add(instance);
                }

                return instance;
            }

            return null;
        }

        public T LoadObject<T>(Func<T, bool> criteria) where T: IPersistentBase
        {
            if (!_rollbackTransaction)
            {
                T storeObject = DataStoreProvider.DataStore.Load<T>(criteria);

                if (storeObject != null)
                {
                    if (!_persistents.Contains(storeObject))
                        _persistents.Add(storeObject);

                    return storeObject;
                }

                T repoObject = _persistents.OfType<T>().FirstOrDefault(criteria);

                if (repoObject != null)
                    return repoObject;

                return default(T);
            }

            return default(T);
        }

        public void AddTo(IPersistentBase persistent)
        {
            if (!_rollbackTransaction)
            {
                if (!_persistents.Contains(persistent))
                    _persistents.Add(persistent);
            }
        }

        public IList<IPersistentBase> GetObjects()
        {
            return _persistents;
        }

        public void RemoveFrom(IPersistentBase persistent)
        {
            if (!_rollbackTransaction)
            {
                if (_persistents.Contains(persistent))
                {
                    _persistents.Remove(persistent);
          
                    persistent = null;
                }
            }
        }

        public void Commit()
        {
            if (!_rollbackTransaction)
            {
                foreach (var item in _persistents)
                {
                    if (ObjectCommiting != null)
                        ObjectCommiting(item);

                    item.Save();
                }
            }
        }

        public void Rollback()
        {
            _rollbackTransaction = true;
            IList<IPersistentBase> reloadedObjects = new List<IPersistentBase>();

            object _storeLock = new object();

            lock (_storeLock)
            {
                foreach (var item in _persistents)
                {
                    if (ObjectRollback != null)
                        ObjectRollback(item);

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

            _persistents = reloadedObjects;

            for (int i = reloadedObjects.Count - 1; i >= 0; i--)
            {
                reloadedObjects[i] = null;
            }

            reloadedObjects = null;

            _rollbackTransaction = false;
        }

        void ClearContainerCollection()
        {
            for (int i = _persistents.Count - 1; i >= 0; i--)
            {
                _persistents[i] = null;
            }

            _persistents.Clear();
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

                _persistents = null;
            }

            disposed = true;
        }
    }
}