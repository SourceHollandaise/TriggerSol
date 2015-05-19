using System.IO;
using System.Linq;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using System.Threading.Tasks;
using System;

namespace TriggerSol.JStore
{
    public static class PersistentBaseFileExtensions
    {

        public static string GetPersistentFilePathAbsolute(this IPersistentBase persistent)
        {
            var typeFolder = persistent.GetType().FullName;
            var file = persistent.MappingId.ToString();
            var folder = TypeProvider.Current.GetSingle<IDataStoreConfiguration>().DataStoreLocation;

            return Path.Combine(folder, typeFolder, file);
        }

        public static string GetDcoumentFilePathAbsolute(this IFileData fileData)
        {
            var name = fileData.FileName;

            var folder = TypeProvider.Current.GetSingle<IDataStoreConfiguration>().DataStoreLocation;

            return Path.Combine(folder, name);
        }
    }
}