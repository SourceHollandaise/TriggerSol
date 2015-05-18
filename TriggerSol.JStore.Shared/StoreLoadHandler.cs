using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class StoreLoadHandler : DependencyObject, IStoreLoadHandler
    {
        public IPersistentBase LoadInternal(Type type, object itemId)
        {
            string targetDirectory = TypeResolver.GetSingle<IStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var path = Path.Combine(targetDirectory, itemId + ".json");

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
                        return null;
                    }
                }
            }

            return null;
        }
    }
}