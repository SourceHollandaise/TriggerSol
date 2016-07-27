//
// FileDataService.cs
//
// Author:
//       Jörg Egger <joerg.egger@outlook.de>
//
// Copyright (c) 2015 Jörg Egger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TriggerSol.Dependency;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class FileDataService : DependencyObject, IFileDataService
    {
        public bool Exists(IFileData fileData)
        {
            if (fileData == null || string.IsNullOrEmpty(fileData.FileName))
                return false;
            
            var targetPath = Path.Combine(StoreConfig.DocumentStoreLocation, fileData.FileName);

            return File.Exists(targetPath);
        }

        public string Get(Stream stream, string extension, string file = null)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            var fileName = file == null ? System.Guid.NewGuid() + extension : file + extension;

            var targetPath = Path.Combine(StoreConfig.DocumentStoreLocation, fileName);

            using (var fileStream = File.Create(targetPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Flush();
            }
            return fileName;
        }

        public async Task<string> GetAsync(Stream stream, string extension, string file = null) => await Task.Run(() => Get(stream, extension, file));
        
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
                    Logger.LogException(ex);
                    throw ex;
                }
            }

            return null;
        }

        public bool Delete(IFileData fileData, bool deleteEntry = true)
        {
            try
            {
                var path = fileData.GetFullDocumentPath();

                if (!string.IsNullOrEmpty(path))
                    File.Delete(path);

                if (deleteEntry)
                    DataStoreProvider.DataStore.DeleteById(fileData.GetType(), (fileData as IPersistentBase).MappingId);

                return true;
            }
            catch (Exception ex)
            {
                DependencyResolver.GetSingle<ILogger>().LogException(ex);
                return false;
            }
        }

        public void Write(string path, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
                
            File.AppendAllText(path, content + "\r\n");
        }

        IDataStoreConfiguration StoreConfig => DependencyResolver.GetSingle<IDataStoreConfiguration>();
    }
}