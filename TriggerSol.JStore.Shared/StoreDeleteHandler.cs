using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class StoreDeleteHandler : DependencyObject, IStoreDeleteHandler
    {
        public void DeleteInternal(Type type, object itemId)
        {
            string targetDirectory = TypeResolver.GetSingle<IStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var path = Path.Combine(targetDirectory, itemId + ".json");

                if (File.Exists(path))
                {
                    File.Delete(path);

                    TypeResolver.GetSingle<ILogger>().Log("Item deleted: " + type.Name + " ID: " + itemId.ToString());

                }
            }
        }
    }
}