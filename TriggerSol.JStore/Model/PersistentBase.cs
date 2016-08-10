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

namespace TriggerSol.JStore
{
    public abstract class PersistentBase : NotifyPropertyChangedBase, IPersistentBase
    {
        public PersistentBase()
        {
            if (IsNewObject)
                Initialize();
        }

        public PersistentBase(Session session) : this()
        {
            Session = session;
        }

        Session _Session;
        public Session Session
        {
            get { return _Session; }
            set { SetPropertyValue(ref _Session, value); }
        }

        object _MappingId;
        public object MappingId
        {
            get { return _MappingId; }
            set { SetPropertyValue(ref _MappingId, value); }
        }

        public bool IsNewObject => MappingId == null;

        public virtual void Initialize()
        {
            if (!IsNewObject)
                return;
        }

        public virtual void Save()
        {
            foreach (var refObj in this.GetReferenceObjects())
                refObj.Key.Save();

            Session.SaveObject(this);
        }

        public IPersistentBase Clone(bool withId = false)
        {
            var clone = Session.ResolveObject<IObjectCloner>().CloneObject(this);

            if (!withId)
                clone.MappingId = null;

            return clone;
        }

        public virtual void Delete()
        {
            foreach (var refObj in this.GetReferenceObjects())
                this.DeleteReferenceObject(refObj.Key, refObj.Value);

            Session.DeleteObject(this);
        }

        public virtual IPersistentBase Reload() => IsNewObject ? null : Session.ReloadObject(this);

        protected IList<T> GetAssociatedCollection<T>(string associatedProperty) where T : IPersistentBase
        {
            var pInfo = GetProperty(typeof(T).GetTypeInfo(), associatedProperty);

            if (pInfo != null)
                return Session.LoadAll<T>(p => pInfo.GetValue(p) != null && !((IPersistentBase)pInfo.GetValue(p)).IsNewObject && ((IPersistentBase)pInfo.GetValue(p)).MappingId.Equals(MappingId)).ToList();

            return Enumerable.Empty<T>().ToList();
        }

        protected PropertyInfo GetProperty(TypeInfo typeInfo, string propertyName)
        {
            var pInfo = typeInfo.GetDeclaredProperty(propertyName);

            if (pInfo == null && typeInfo.BaseType != null)
                pInfo = GetProperty(typeInfo.BaseType.GetTypeInfo(), propertyName);

            return pInfo;
        }
    }
}
