//
// DataStoreLoadHandler.cs
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
using Newtonsoft.Json;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class DataStoreLoadHandler : DependencyObject, IDataStoreLoadHandler
    {
        public IPersistentBase LoadInternal(Type type, object mappingId)
        {
            if (mappingId == null)
                return null;
            
            string targetDirectory = DependencyResolver.ResolveSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var path = Path.Combine(targetDirectory, mappingId + ".json");

                if (File.Exists(path))
                {
                    try
                    {
                        var content = File.ReadAllText(path);
                        var item = JsonConvert.DeserializeObject(content, type) as IPersistentBase;

                        Logger.Log("Item loaded: " + type.Name + " ID: " + item.MappingId.ToString());

                        return item;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);

                        throw new DataStoreException("Loading object failed!", ex, this);
                    }
                }
            }

            return null;
        }
    }
}