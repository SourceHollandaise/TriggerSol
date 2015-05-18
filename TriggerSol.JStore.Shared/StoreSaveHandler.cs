using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class StoreSaveHandler : DependencyObject, IStoreSaveHandler
    {
        public void SaveInternal(Type type, IPersistentBase item)
        {
            string targetDirectory = TypeResolver.GetSingle<IStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                item.MappingId = item.MappingId ?? TypeResolver.GetObject<IMappingIdGenerator>().GetId();

                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(item, type, settings);
                var path = Path.Combine(targetDirectory, item.MappingId + ".json");

                File.WriteAllText(path, json);

                TypeResolver.GetSingle<ILogger>().Log("Item saved: " + type.Name + " ID: " + item.MappingId.ToString());

            }
        }
    }
}