using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
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
            var clone = this.MemberwiseClone() as IPersistentBase;

            if (!withId)
                clone.MappingId = null;

            return clone;
        }

        public virtual void Delete()
        {
            DataStore.Delete(GetType(), this);
        }

        public virtual IPersistentBase Reload()
        {
            return MappingId == null ? null : DataStore.Load(GetType(), MappingId);
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

        [Newtonsoft.Json.JsonIgnore]
        public ITypeResolver TypeResolver
        {
            get
            {
                return TypeProvider.Current;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        protected IDataStore DataStore
        {
            get
            {
                return DataStoreProvider.DataStore;
            }
        }

        private PropertyInfo GetProperty(TypeInfo typeInfo, string propertyName)
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
