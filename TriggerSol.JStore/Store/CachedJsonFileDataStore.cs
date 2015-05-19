//
// CachedJsonFileDataStore.cs
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
using System.Linq;
using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public class CachedJsonFileDataStore : JsonFileDataStore
    {
        public CachedJsonFileDataStore()
        {
            
        }

        internal protected override IPersistentBase LoadInternal(Type type, object mappingId)
        {
            object addLocker = new object();
            lock (addLocker)
            {
                var valueStore = CacheProvider.GetTypeRepo(type);

                if (!valueStore.ContainsKey(mappingId))
                {
                    var item = base.LoadInternal(type, mappingId);

                    if (item == null)
                        return null;
                   
                    valueStore.Add(item.MappingId, item);
                }
              
                return valueStore[mappingId];
            }
        }

        internal protected override IEnumerable<IPersistentBase> LoadAllInternal(Type type)
        {
            if (!CacheProvider.TypesMap.Contains(type))
                CacheProvider.StartCaching(type);

            return CacheProvider.GetTypeRepo(type).Values.Where(p => p.GetType() == type);
        }

        internal protected override void SaveInternal(Type type, IPersistentBase item)
        {
            object saveLocker = new object();

            lock (saveLocker)
            {
                base.SaveInternal(type, item);

                var clone = item.Clone();
                clone.MappingId = item.MappingId;

                var valueStore = CacheProvider.GetTypeRepo(type);

                if (valueStore.ContainsKey(clone.MappingId))
                    valueStore[clone.MappingId] = clone;
                else
                    valueStore.Add(clone.MappingId, clone);
            }
        }

        internal protected override void DeleteInternal(Type type, object mappingId)
        {
            object deleteLocker = new object();
            lock (deleteLocker)
            {
                var valueStore = CacheProvider.GetTypeRepo(type);

                if (valueStore.ContainsKey(mappingId))
                    valueStore.Remove(mappingId);

                base.DeleteInternal(type, mappingId);
            }
        }

        internal protected override string GetTargetLocation(Type type)
        {
            return base.GetTargetLocation(type);
        }
    }
}