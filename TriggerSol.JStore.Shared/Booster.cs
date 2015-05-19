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

        public Booster()
        {
            RegisterLogger(new DebugLogger { Level = LogLevel.OnlyException });
        }

        public void InitDataStore<T>(string dataStorePath) where T: IDataStore, new()
        {
            if (StartBoosting != null)
                StartBoosting();

            if (typeof(T).GetInterface(typeof(IMemoryStore).FullName) == null)
                SetStoreConfiguration(dataStorePath);

            InitializeDataStore<T>();

            if (FinishedBoosting != null)
                FinishedBoosting();
        }

        public void RegisterLogger(ILogger logger)
        {
            TypeResolver.ClearSingle<ILogger>();
            TypeResolver.RegisterSingle<ILogger>(logger);
        }

        protected virtual void SetStoreConfiguration(string dataStorePath)
        {
            TypeResolver.RegisterSingle<IDataStoreConfiguration>(new DataStoreConfiguration(dataStorePath));
        }

        protected virtual void InitializeDataStore<T>() where T: IDataStore, new()
        {
            RegisterPersistentIdGenerator();

            RegisterFileDataService();

            RegisterJsonSettings();

            DataStoreManager.RegisterStore<T>();
        }

        protected virtual void RegisterPersistentIdGenerator()
        {
            TypeResolver.RegisterObjectType<IMappingIdGenerator, GuidIdGenerator>();
        }

        protected virtual void RegisterFileDataService()
        {
            TypeResolver.RegisterObjectType<IFileDataService, FileDataService>();
        }

        protected virtual void RegisterJsonSettings()
        {
            TypeResolver.RegisterObjectType<IContractResolver, JsonWritablePropertiesContractResolver>();
            TypeResolver.RegisterObjectType<IJsonSerializerSettings, JsonStoreSerializerSettings>();
        }
    }
}
