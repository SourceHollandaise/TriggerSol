using System.IO;
using System.Linq;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using System.Threading.Tasks;
using System;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class FileDataService : DependencyObject, IFileDataService
    {
        IDataStoreConfiguration StoreConfig
        {
            get
            {
                return  TypeResolver.GetSingle<IDataStoreConfiguration>();
            }
        }

        public string Get(Stream stream, string mimeType)
        {
            var fileName = System.Guid.NewGuid() + mimeType;

            var targetPath = Path.Combine(StoreConfig.DocumentStoreLocation, fileName);

            using (var fileStream = File.Create(targetPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Flush();
            }
            return fileName;
        }

        public Task<string> GetAsync(Stream stream, string mimeType)
        {
            var result = Task.Run(() => Get(stream, mimeType));

            return result;
        }

        public IFileData GetFileData<T>(string sourcePath, bool copy = true) where T : IFileData
        {
            if (File.Exists(sourcePath))
            {
                var fileInfo = new FileInfo(sourcePath);

                var fileName = fileInfo.Name;

                var targetPath = Path.Combine(StoreConfig.DocumentStoreLocation, fileName);

                try
                {
                    if (sourcePath.Equals(targetPath))
                        return null;

                    bool dataEntryCreated = false;

                    var fileData = Activator.CreateInstance<T>();

                    fileData.FileName = new FileInfo(targetPath).Name;
                    fileData.Subject = fileData.FileName;

                    dataEntryCreated = true;
                 

                    if (dataEntryCreated)
                    {
                        if (copy)
                            File.Copy(sourcePath, targetPath);
                        else
                            File.Move(sourcePath, targetPath);
                    }

                    fileData.MimeType = fileInfo.Extension.Contains("pdf") ? "application/pdf" : "application/octet-stream";

                    return fileData;
                }
                catch (Exception ex)
                {
                    TypeResolver.GetSingle<ILogger>().LogException(ex);
                    return null;
                }
            }

            return null;
        }

        public bool Delete(IFileData fileData, bool deleteEntry = true)
        {
            try
            {
                var path = fileData.GetDcoumentFilePathAbsolute();

                if (!string.IsNullOrEmpty(path))
                    File.Delete(path);

                if (deleteEntry)
                    DataStoreProvider.DataStore.DeleteById(fileData.GetType(), (fileData as IPersistentBase).MappingId);

                return true;
            }
            catch (Exception ex)
            {
                TypeResolver.GetSingle<ILogger>().LogException(ex);
                return false;
            }
        }
    }

}