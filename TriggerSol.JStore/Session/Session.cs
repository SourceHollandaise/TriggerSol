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
                    _DataStore = DependencyResolver.ResolveSingle<IDataStore>();
                return _DataStore;
            }
        }

        protected bool SessionRollback { get; set; }

        protected IList<IPersistentBase> Objects = new List<IPersistentBase>();

        public Action<IPersistentBase> ObjectCommiting { get; set; }

        public Action<IPersistentBase> ObjectRollingback { get; set; }

        public T CreateObject<T>() where T : IPersistentBase => (T)CreateObject(typeof(T));

        public T LoadObject<T>(Func<T, bool> criteria) where T : IPersistentBase
        {
            if (!SessionRollback)
            {
                T obj = DataStore.Load(criteria);

                if (obj != null)
                {
                    AddObject(obj);

                    if (!Objects.Contains(obj))
                        Objects.Add(obj);

                    return obj;
                }

                T repoObject = Objects.OfType<T>().FirstOrDefault(criteria);
                if (repoObject != null)
                    return repoObject;

                return default(T);
            }

            return default(T);
        }

        public IList<IPersistentBase> GetObjects() => Objects;

        public void AddObject(IPersistentBase persistent)
        {
            if (!SessionRollback)
            {
                if (!Objects.Contains(persistent))
                {
                    persistent.Session = this;
                    Objects.Add(persistent);
                }
            }
        }

        public void RemoveObject(IPersistentBase persistent)
        {
            if (!SessionRollback)
            {
                if (Objects.Contains(persistent))
                {
                    Objects.Remove(persistent);
                    persistent.Session = null;
                    persistent = null;
                }
            }
        }

        public void Commit()
        {
            if (!SessionRollback)
            {
                foreach (var item in Objects)
                {
                    ObjectCommiting?.Invoke(item);

                    var refTypes = item.GetType().GetRuntimeProperties().Where(p => p.FindAttribute<ReferenceAttribute>() != null);

                    foreach (var type in refTypes)
                        (type.GetValue(item) as IPersistentBase)?.Save();

                    item.Save();
                }
            }
        }

        public void Rollback()
        {
            SessionRollback = true;
            IList<IPersistentBase> reloadedObjects = new List<IPersistentBase>();

            object _storeLock = new object();

            lock (_storeLock)
            {
                foreach (var obj in Objects)
                {
                    ObjectRollingback?.Invoke(obj);

                    IPersistentBase reloadedObject = null;

                    if (obj.MappingId != null)
                        reloadedObject = obj.Reload();
                    else
                        reloadedObject = CreateObject(obj.GetType(), false);

                    if (reloadedObject != null)
                        reloadedObjects.Add(reloadedObject);
                }
            }

            ClearContainerCollection();

            Objects = reloadedObjects;

            for (int i = reloadedObjects.Count - 1; i >= 0; i--)
                reloadedObjects[i] = null;

            reloadedObjects = null;

            SessionRollback = false;
        }

        protected IPersistentBase CreateObject(Type type, bool addToContainer = true)
        {
            if (!SessionRollback)
            {
                var obj = Activator.CreateInstance(type, this) as IPersistentBase;

                if (obj == null)
                    throw new ArgumentNullException("instance");

                obj.Initialize();

                if (addToContainer)
                {
                    if (!Objects.Contains(obj))
                        Objects.Add(obj);
                }

                return obj;
            }

            return null;
        }

        protected void ClearContainerCollection()
        {
            for (int i = Objects.Count - 1; i >= 0; i--)
                Objects[i] = null;

            Objects.Clear();
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

                Objects = null;
            }

            disposed = true;
        }
    }
}