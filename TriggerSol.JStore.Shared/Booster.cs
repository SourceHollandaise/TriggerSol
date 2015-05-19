using System;
using System.IO;
using Newtonsoft.Json.Serialization;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.Boost
{
    public class Booster : DependencyObject
    {
        public Booster()
        {
            RegisterLogger(new DebugLogger { Level = LogLevel.Detailed });
        }

        public void InitDataStore<T>(string dataStorePath) where T: IDataStore, new()
        {
            SetStoreConfiguration(dataStorePath);

            InitializeDataStore<T>();
        }

        public void RegisterLogger(ILogger logger)
        {
            TypeResolver.ClearSingle<ILogger>();
            TypeResolver.RegisterSingle<ILogger>(logger);
        }

        protected virtual void SetStoreConfiguration(string dataStorePath)
        {
            var config = new DataStoreConfiguration(dataStorePath);
            config.InitStore();

            TypeResolver.RegisterSingle<IDataStoreConfiguration>(config);
        }

        protected virtual void InitializeDataStore<T>() where T: IDataStore, new()
        {
            TypeResolver.RegisterObjectType<IMappingIdGenerator, GuidIdGenerator>();
            TypeResolver.RegisterObjectType<IFileDataService, FileDataService>();

            TypeResolver.RegisterObjectType<IContractResolver, JsonWritablePropertiesContractResolver>();
            TypeResolver.RegisterObjectType<IJsonSerializerSettings, JsonStoreSerializerSettings>();

            DataStoreManager.RegisterStore<T>();
        }
    }
}
