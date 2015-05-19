using System;

namespace TriggerSol.Logging
{
    public class DebugLogger : ILogger
    {
        public DebugLogger()
        {
            Level = LogLevel.Detailed;
        }

        public void LogException(Exception ex)
        {
            if (Level == LogLevel.None)
                return;

            if (Level == LogLevel.OnlyException)
                System.Diagnostics.Debug.WriteLine(ex.Message);

            if (Level == LogLevel.Detailed)
                System.Diagnostics.Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
        }

        public void Log(string text)
        {
            if (Level == LogLevel.Detailed)
                System.Diagnostics.Debug.WriteLine(text);
        }

        public LogLevel Level
        {
            get;
            set;
        }
    }
}
