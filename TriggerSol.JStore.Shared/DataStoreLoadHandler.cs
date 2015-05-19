using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class DataStoreLoadHandler : DependencyObject, IDataStoreLoadHandler
    {
        public IPersistentBase LoadInternal(Type type, object mappingId)
        {
            string targetDirectory = TypeResolver.GetSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var path = Path.Combine(targetDirectory, mappingId + ".json");

                if (File.Exists(path))
                {
                    try
                    {
                        var content = File.ReadAllText(path);
                        var item = Newtonsoft.Json.JsonConvert.DeserializeObject(content, type) as IPersistentBase;

                        TypeResolver.GetSingle<ILogger>().Log("Item loaded: " + type.Name + " ID: " + item.MappingId.ToString());

                        return item;
                    }
                    catch (Exception ex)
                    {
                        TypeResolver.GetSingle<ILogger>().LogException(ex);

                        throw new JStoreException("Loading object failed!", ex, this);
                    }
                }
            }

            return null;
        }
    }
}