using System;

namespace TriggerSol.JStore
{
    public interface IFileData
    {
        string Subject { get; set; }

        string Information { get; }

        string FileName { get; set; }

        long Size { get; set; }

        string MimeType { get; set; }

        DateTime CreationDate { get; set; }
    }
}
