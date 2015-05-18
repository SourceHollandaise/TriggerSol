using System;
using System.Linq;
using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public class CachedFileDataStore : FileDataStore
    {
        public CachedFileDataStore()
        {
            
        }

        internal protected override IPersistentBase LoadInternal(Type type, object mappingId)
        {
            object addLocker = new object();
            lock (addLocker)
            {
                var valueStore = CacheProvider.GetTypeRepo(type);

                if (!valueStore.ContainsKey(mappingId))
                {
                    var item = base.LoadInternal(type, mappingId);

                    if (item == null)
                        return null;
                   
                    valueStore.Add(item.MappingId, item);
                }
              
                return valueStore[mappingId];
            }
        }

        internal protected override IEnumerable<IPersistentBase> LoadAllInternal(Type type)
        {
            if (!CacheProvider.TypesMap.Contains(type))
                CacheProvider.StartCaching(type);

            return CacheProvider.GetTypeRepo(type).Values.Where(p => p.GetType() == type);
        }

        internal protected override void SaveInternal(Type type, IPersistentBase item)
        {
            object saveLocker = new object();

            lock (saveLocker)
            {
                base.SaveInternal(type, item);

                var clone = item.Clone();
                clone.MappingId = item.MappingId;

                var valueStore = CacheProvider.GetTypeRepo(type);

                if (valueStore.ContainsKey(clone.MappingId))
                    valueStore[clone.MappingId] = clone;
                else
                    valueStore.Add(clone.MappingId, clone);
            }
        }

        internal protected override void DeleteInternal(Type type, object mappingId)
        {
            object deleteLocker = new object();
            lock (deleteLocker)
            {
                var valueStore = CacheProvider.GetTypeRepo(type);

                if (valueStore.ContainsKey(mappingId))
                    valueStore.Remove(mappingId);

                base.DeleteInternal(type, mappingId);
            }
        }

        internal protected override string GetTargetLocation(Type type)
        {
            return base.GetTargetLocation(type);
        }
    }
}