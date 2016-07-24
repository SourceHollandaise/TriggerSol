//
// FileLogger.cs
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
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace XConsole
{
    public class FileLogger : DependencyObject, ILogger
    {
        IFileDataService _fileDataService;

        string _path;

        public FileLogger()
        {
            _path = Path.Combine(TypeResolver.GetSingle<IDataStoreConfiguration>().DataStoreLocation, "Log.txt");
            _fileDataService = TypeResolver.GetObject<IFileDataService>();

        }

        public void LogException(Exception ex)
        {
            if (Level == LogLevel.None)
                return;

            if (ex != null)
            {
                string hint = DateTime.Now + ": ERROR\r\n";

                _fileDataService.Write(_path, hint + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        public void Log(string text)
        {
            if (Level == LogLevel.Detailed)
            {
                string hint = DateTime.Now + ": LOG ENTRY\r\n";
                _fileDataService.Write(_path, hint + text);
            }
        }

        public LogLevel Level { get; set; }

        public int MaxSize { get; set; }
    }
}
