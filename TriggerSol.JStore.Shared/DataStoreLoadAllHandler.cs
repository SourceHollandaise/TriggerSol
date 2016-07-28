//
// DataStoreLoadAllHandler.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class DataStoreLoadAllHandler : DependencyObject, IDataStoreLoadAllHandler
    {
        public IEnumerable<IPersistentBase> LoadAllInternal(Type type)
        {
            string targetDirectory = DependencyResolver.GetSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                var storables = new List<IPersistentBase>();

                var files = Directory.EnumerateFiles(targetDirectory, "*" + ".json", SearchOption.TopDirectoryOnly).ToList();

                foreach (var file in files)
                {
                    try
                    {
                        var content = File.ReadAllText(file);
                        var item = JsonConvert.DeserializeObject(content, type) as IPersistentBase;
                        storables.Add(item);

                        Logger.Log("Item loaded: " + type.Name + " ID: " + item.MappingId.ToString());
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);

                        throw new DataStoreException("Loading objects failed!", ex, this);
                    }
                }

                return storables;
            }

            return Enumerable.Empty<IPersistentBase>();
        }
    }
}