using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;

namespace TriggerSol.JStore
{
    public class StoreDirectoryHandler : DependencyObject, IStoreDirectoryHandler
    {
        IStoreConfiguration _storeConfig;

        protected IStoreConfiguration StoreConfig
        {
            get
            {
                if (_storeConfig == null)
                    _storeConfig = TypeResolver.GetSingle<IStoreConfiguration>();
                return _storeConfig;
            }
        }

        public string GetTypeDirectory(Type type)
        {
            if (!Directory.Exists(StoreConfig.DataStoreLocation))
                return string.Empty;

            var typeDir = Path.Combine(StoreConfig.DataStoreLocation, type.FullName);

            if (!Directory.Exists(typeDir))
                Directory.CreateDirectory(typeDir);

            return typeDir;
        }
    }
}