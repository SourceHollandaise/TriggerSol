using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class StoreLoadAllHandler : DependencyObject, IStoreLoadAllHandler
    {
        public IEnumerable<IPersistentBase> LoadAllInternal(Type type)
        {
            string targetDirectory = TypeResolver.GetSingle<IStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var storables = new List<IPersistentBase>();

                var files = Directory.EnumerateFiles(targetDirectory, "*" + ".json", SearchOption.TopDirectoryOnly).ToList();

                foreach (var file in files)
                {
                    try
                    {
                        var content = File.ReadAllText(file);
                        var item = Newtonsoft.Json.JsonConvert.DeserializeObject(content, type) as IPersistentBase;
                        storables.Add(item);

                        TypeResolver.GetSingle<ILogger>().Log("Item loaded: " + type.Name + " ID: " + item.MappingId.ToString());
                    }
                    catch (Exception ex)
                    {
                        TypeResolver.GetSingle<ILogger>().LogException(ex);
                        continue;
                    }
                }

                return storables;
            }

            return Enumerable.Empty<IPersistentBase>();
        }
    }
}