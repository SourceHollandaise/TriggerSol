using System.IO;
using System.Threading.Tasks;
using System;

namespace TriggerSol.JStore
{
    public interface IFileDataService
    {
        string Get(Stream stream, string mimeType);

        Task<string> GetAsync(Stream stream, string mimeType);

        IFileData GetFileData<T>(string sourcePath, bool copy = true) where T: IFileData;

        bool Delete(IFileData fileData, bool deleteEntry = true);
    }
}
