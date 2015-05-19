using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;

namespace TriggerSol.JStore
{
    public class DataStoreDirectoryHandler : DependencyObject, IDataStoreDirectoryHandler
    {
        IDataStoreConfiguration _storeConfig;

        protected IDataStoreConfiguration StoreConfig
        {
            get
            {
                if (_storeConfig == null)
                    _storeConfig = TypeResolver.GetSingle<IDataStoreConfiguration>();
                return _storeConfig;
            }
        }

        public string GetTypeDirectory(Type type)
        {
            if (!Directory.Exists(StoreConfig.DataStoreLocation))
                return string.Empty;

            var typeDir = Path.Combine(StoreConfig.DataStoreLocation, GetTargetFolder(type));

            if (!Directory.Exists(typeDir))
                Directory.CreateDirectory(typeDir);

            return typeDir;
        }

        string GetTargetFolder(Type type)
        {
            var folder = type.FullName;

            var persistentAttribute = type.FindAttribute<PersistentNameAttribute>();

            if (persistentAttribute != null && !string.IsNullOrWhiteSpace(persistentAttribute.PersistentName))
                folder = persistentAttribute.PersistentName;

            return folder;
        }
    }
}