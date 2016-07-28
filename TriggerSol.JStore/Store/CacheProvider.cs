//
// CacheProvider.cs
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
using System.Linq;

namespace TriggerSol.JStore
{
    internal static class CacheProvider
    {
        static Dictionary<Type, Dictionary<object, IPersistentBase>> _Repository = new Dictionary<Type, Dictionary<object, IPersistentBase>>();

        internal static List<Type> TypesMap = new List<Type>();

        public static void StartCaching<T>() where T : IPersistentBase => StartCaching(typeof(T));
        
        public static void StartCaching(Type type)
        {
            if (TypesMap.Contains(type))
                return;

            var valueStore = GetRepositoryForType(type);

            object addLocker = new object();

            lock (addLocker)
            {
                var result = Dependency.DependencyResolverProvider.Current.GetSingle<IDataStore>().InitializeAll(type).Where(p => p != null && p.MappingId != null).ToList();

                foreach (var item in result)
                {
                    if (valueStore.ContainsKey(item.MappingId))
                        continue;
                        
                    valueStore.Add(item.MappingId, item);  
                }
            }

            TypesMap.Add(type);
        }

        internal static Dictionary<object, IPersistentBase> GetRepositoryForType(Type type)
        {
            if (!_Repository.ContainsKey(type))
                _Repository.Add(type, new Dictionary<object, IPersistentBase>());

            return _Repository[type];
        }
    }
}