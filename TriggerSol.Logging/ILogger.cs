using System;

namespace TriggerSol.Logging
{
    public interface ILogger
    {
        void LogException(Exception ex);

        void Log(string text);

        LogLevel Level { get; set; }
    }
}


