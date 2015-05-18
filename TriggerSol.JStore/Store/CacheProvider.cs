using System;
using System.Collections.Generic;
using System.Linq;

namespace TriggerSol.JStore
{
    internal static class CacheProvider
    {
        static Dictionary<Type, Dictionary<object, IPersistentBase>> repository = new Dictionary<Type, Dictionary<object, IPersistentBase>>();

        internal static List<Type> TypesMap = new List<Type>();

        public static Dictionary<object, IPersistentBase> GetTypeRepo(Type type)
        {
            if (!repository.ContainsKey(type))
                repository.Add(type, new Dictionary<object, IPersistentBase>());

            return repository[type];
        }

        public static void StartCaching<T>() where T : IPersistentBase
        {
            StartCaching(typeof(T));
        }

        public static void StartCaching(Type type)
        {
            if (TypesMap.Contains(type))
                return;

            var valueStore = GetTypeRepo(type);

            object addLocker = new object();

            lock (addLocker)
            {
                var result = DataStoreProvider.DataStore.InitializeAll(type).Where(p => p != null && p.MappingId != null).ToList();

                foreach (var item in result)
                {
                    if (valueStore.ContainsKey(item.MappingId))
                        continue;
                        
                    valueStore.Add(item.MappingId, item);  
                }
            }

            TypesMap.Add(type);
        }
    }
}