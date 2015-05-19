using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class DataStoreSaveHandler : DependencyObject, IDataStoreSaveHandler
    {
        public void SaveInternal(Type type, IPersistentBase persistent)
        {
            string targetDirectory = TypeResolver.GetSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                persistent.MappingId = persistent.MappingId ?? TypeResolver.GetObject<IMappingIdGenerator>().GetId();

                try
                {
                    var settings = (JsonSerializerSettings)TypeResolver.GetObject<IJsonSerializerSettings>();

                    var json = JsonConvert.SerializeObject(persistent, type, settings);
                    var path = Path.Combine(targetDirectory, persistent.MappingId + ".json");

                    File.WriteAllText(path, json);

                    TypeResolver.GetSingle<ILogger>().Log("Item saved: " + type.Name + " ID: " + persistent.MappingId.ToString());
                }
                catch (Exception ex)
                {
                    TypeResolver.GetSingle<ILogger>().LogException(ex);

                    throw new JStoreException("Saving object failed!", ex, this);
                }
            }
        }
    }
}