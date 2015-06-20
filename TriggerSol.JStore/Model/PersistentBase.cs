//
// PersistentBase.cs
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
    public abstract class PersistentBase : NotifyPropertyChangedBase, IPersistentBase, IDependencyObject
    {
        protected PersistentBase()
        {
            if (MappingId == null)
                Initialize();
        }

        object _MappingId;

        public object MappingId
        {
            get
            {
                return _MappingId;
            }
            set
            {
                SetPropertyValue(ref _MappingId, value);
            }
        }

        public bool IsNewObject
        {
            get
            {
                return MappingId == null;
            }
        }

        public virtual void Initialize()
        {
            if (MappingId != null)
                return;
        }

        public virtual void Save(bool allowSaving = true)
        {
            if (allowSaving)
                DataStore.Save(GetType(), this);
        }

        public IPersistentBase Clone(bool withId = false)
        {
            var clone = this.CloneObject();

            if (!withId)
                clone.MappingId = null;

            return clone;
        }

        public virtual void Delete(bool allowDeleting = true)
        {
            if (allowDeleting)
                DataStore.Delete(GetType(), this);
        }

        public virtual IPersistentBase Reload()
        {
            return MappingId == null ? null : DataStore.Load(GetType(), MappingId);
        }

        public ITypeResolver TypeResolver
        {
            get
            {
                return TypeProvider.Current;
            }
        }

        protected IDataStore DataStore
        {
            get
            {
                return DataStoreProvider.DataStore;
            }
        }

        public virtual IList<T> GetAssociatedCollection<T>(string associatedProperty) where T: IPersistentBase
        {
            var pInfo = GetProperty(typeof(T).GetTypeInfo(), associatedProperty);

            if (pInfo != null)
            {
                return DataStore.LoadAll<T>(p => pInfo.GetValue(p) != null && ((IPersistentBase)pInfo.GetValue(p)).MappingId != null && ((IPersistentBase)pInfo.GetValue(p)).MappingId.Equals(MappingId)).ToList();
            }

            return Enumerable.Empty<T>().ToList();
        }

        protected virtual PropertyInfo GetProperty(TypeInfo typeInfo, string propertyName)
        {
            var pInfo = typeInfo.GetDeclaredProperty(propertyName);

            if (pInfo == null && typeInfo.BaseType != null)
            {
                pInfo = GetProperty(typeInfo.BaseType.GetTypeInfo(), propertyName);
            }

            return pInfo;
        }
    }
}
