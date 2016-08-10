//
// Booster.cs
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
using Newtonsoft.Json.Serialization;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.Boost
{
    public class Booster : DependencyObject
    {
        public Action StartBoosting { get; set; }

        public Action FinishedBoosting { get; set; }

        LogLevel _LogLevel;

        public Booster(LogLevel logLevel = LogLevel.OnlyException)
        {
            _LogLevel = logLevel;

            RegisterLogger<DebugLogger>();
        }

        public void InitDataStore<T>(string dbPath, string dbName) where T : IDataStore, new()
        {
            StartBoosting?.Invoke();

            if (typeof(T).GetInterface(typeof(IInMemoryStore).FullName) == null)
                SetStoreConfiguration(Path.Combine(dbPath, dbName));

            InitializeDataStore<T>();

            FinishedBoosting?.Invoke();
        }

        public void RegisterLogger<T>() where T : ILogger
        {
            FinishedBoosting += () =>
            {
                var logger = Activator.CreateInstance<T>() as ILogger;
                if (logger == null)
                {
                    logger = TryCreateFallbackLogger();
                    if (logger == null)
                        throw new ArgumentNullException(nameof(logger), "Could not create Logger!");
                }

                logger.Level = _LogLevel;

                DependencyResolver.ClearSingle<ILogger>();
                DependencyResolver.RegisterSingle<ILogger>(logger);
            };
        }

        ILogger TryCreateFallbackLogger() => new NullLogger();

        protected virtual void SetStoreConfiguration(string dataStorePath) => DependencyResolver.RegisterSingle<IDataStoreConfiguration>(new DataStoreConfiguration(dataStorePath));

        protected virtual void InitializeDataStore<T>() where T : IDataStore, new()
        {
            RegisterPersistentIdGenerator();

            RegisterFileDataService();

            RegisterJsonSettings();

            RegisterCloner();

            DataStoreManager.RegisterStore<T>();
        }

        protected virtual void RegisterPersistentIdGenerator() => DependencyResolver.RegisterObjectType<IMappingIdGenerator, GuidIdGenerator>();

        protected virtual void RegisterFileDataService() => DependencyResolver.RegisterObjectType<IFileDataService, FileDataService>();

        protected virtual void RegisterCloner() => DependencyResolver.RegisterObjectType<IObjectCloner, JsonObjectCloner>();

        protected virtual void RegisterJsonSettings()
        {
            DependencyResolver.RegisterObjectType<IContractResolver, JsonWritablePropertiesContractResolver>();
            DependencyResolver.RegisterObjectType<IJsonSerializerSettings, JsonStoreSerializerSettings>();
        }
    }
}
