using System;
using Newtonsoft.Json.Serialization;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.Boost
{
    public class Booster : DependencyObject
    {
        public Action StartBoosting { get; set; }

        public Action FinishedBoosting { get; set; }

        public Booster()
        {
            RegisterLogger(new DebugLogger { Level = LogLevel.Detailed });
        }

        public void InitDataStore<T>(string dataStorePath) where T: IDataStore, new()
        {
            if (StartBoosting != null)
                StartBoosting();

            if (typeof(T).GetInterface(typeof(IMemoryStore).FullName) == null)
                SetStoreConfiguration(dataStorePath);

            InitializeDataStore<T>();

            if (FinishedBoosting != null)
                FinishedBoosting();
        }

        public void RegisterLogger(ILogger logger)
        {
            TypeResolver.ClearSingle<ILogger>();
            TypeResolver.RegisterSingle<ILogger>(logger);
        }

        protected virtual void SetStoreConfiguration(string dataStorePath)
        {
            TypeResolver.RegisterSingle<IDataStoreConfiguration>(new DataStoreConfiguration(dataStorePath));
        }

        protected virtual void InitializeDataStore<T>() where T: IDataStore, new()
        {
            RegisterPersistentIdGenerator();

            RegisterFileDataService();

            RegisterJsonSettings();

            DataStoreManager.RegisterStore<T>();
        }

        protected virtual void RegisterPersistentIdGenerator()
        {
            TypeResolver.RegisterObjectType<IMappingIdGenerator, GuidIdGenerator>();
        }

        protected virtual void RegisterFileDataService()
        {
            TypeResolver.RegisterObjectType<IFileDataService, FileDataService>();
        }

        protected virtual void RegisterJsonSettings()
        {
            TypeResolver.RegisterObjectType<IContractResolver, JsonWritablePropertiesContractResolver>();
            TypeResolver.RegisterObjectType<IJsonSerializerSettings, JsonStoreSerializerSettings>();
        }
    }
}
