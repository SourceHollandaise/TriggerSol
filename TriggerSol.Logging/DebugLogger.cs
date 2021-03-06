//
// DebugLogger.cs
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

            if (ex != null)
            {
                if (Level == LogLevel.OnlyException)
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                if (Level == LogLevel.Detailed)
                    System.Diagnostics.Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        public void Log(string text)
        {
            if (Level == LogLevel.Detailed)
                System.Diagnostics.Debug.WriteLine(text);
        }

        public LogLevel Level { get; set; }

        public int MaxSize { get; set; }
    }
}
