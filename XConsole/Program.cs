//
// Program.cs
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
using TriggerSol.Boost;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace XConsole
{
    class Programm
    {
        static void InitBooster()
        {
            var booster = new Booster(LogLevel.OnlyException);
            booster.RegisterLogger<FileLogger>();
            booster.InitDataStore<CachedFileDataStore>(Environment.SpecialFolder.MyDocuments.ToString());
        }

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                if (e.ExceptionObject is Exception)
                    DependencyResolverProvider.Current.GetSingle<ILogger>().LogException(e.ExceptionObject as Exception);
            };
            
            Console.WriteLine("Init Booster...");
            TriggerSol.Console.Spinner.Start(150);
            System.Threading.Thread.Sleep(1000);
            TriggerSol.Console.Spinner.Stop();

            InitBooster();

            Console.WriteLine("Datastore is ready!");

            Console.ReadKey();
        }
    }
}
