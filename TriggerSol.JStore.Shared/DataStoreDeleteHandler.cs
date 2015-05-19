using System;
using System.IO;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class DataStoreDeleteHandler : DependencyObject, IDataStoreDeleteHandler
    {
        public void DeleteInternal(Type type, object mappingId)
        {
            string targetDirectory = TypeResolver.GetSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var path = Path.Combine(targetDirectory, mappingId + ".json");

                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);

                        TypeResolver.GetSingle<ILogger>().Log("Item deleted: " + type.Name + " ID: " + mappingId.ToString());
                    }
                    catch (Exception ex)
                    {
                        TypeResolver.GetSingle<ILogger>().LogException(ex);

                        throw new JStoreException("Deleting object failed!", ex, this);
                    }
                }
            }
        }
    }
}