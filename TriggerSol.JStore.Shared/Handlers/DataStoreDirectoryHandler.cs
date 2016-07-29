//
// DataStoreDirectoryHandler.cs
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

namespace TriggerSol.JStore
{
    public class DataStoreDirectoryHandler : DependencyObject, IDataStoreDirectoryHandler
    {
        IDataStoreConfiguration _StoreConfig;
        protected IDataStoreConfiguration StoreConfig
        {
            get
            {
                if (_StoreConfig == null)
                    _StoreConfig = DependencyResolver.ResolveSingle<IDataStoreConfiguration>();
                return _StoreConfig;
            }
        }

        public string GetTypeDirectory(Type type)
        {
            if (!Directory.Exists(StoreConfig.DataStoreLocation))
                return string.Empty;

            var typeDir = Path.Combine(StoreConfig.DataStoreLocation, GetTargetFolder(type));

            if (!Directory.Exists(typeDir))
                Directory.CreateDirectory(typeDir);

            return typeDir;
        }

        string GetTargetFolder(Type type)
        {
            var folder = type.FullName;

            var persistentAttribute = type.FindAttribute<PersistentNameAttribute>();

            if (persistentAttribute != null && !string.IsNullOrWhiteSpace(persistentAttribute.PersistentName))
                folder = persistentAttribute.PersistentName;

            return folder;
        }
    }
}