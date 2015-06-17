//
// JsonFileDataStore.cs
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
using TriggerSol.Dependency;
using TriggerSol.Logging;

namespace TriggerSol.JStore
{
    public class JsonFileDataStore : DependencyObject, IDataStore
    {
        public JsonFileDataStore()
        {
            
        }

        public void Save(Type type, object item)
        {
            SaveInternal(type, item);
        }

        public void Save<T>(T item) where T: object
        {
            SaveInternal(typeof(T), item);
        }

        public void DeleteById(Type type, object itemId)
        {
            DeleteInternal(type, itemId);
        }

        public void DeleteById<T>(object itemId) where T: object
        {
            DeleteInternal(typeof(T), itemId);
        }

        public void Delete(Type type, object item)
        {
            DeleteInternal(type, item.MappingId);
        }

        public void Delete<T>(T item) where T: object
        {
            DeleteInternal(typeof(T), item.MappingId);
        }

        public void Delete<T>(Func<T, bool> criteria) where T : object
        {
            foreach (var item in LoadAll<T>().Where(criteria).ToList())
                DeleteInternal(typeof(T), item.MappingId);
        }

        public void Delete(Type type, Func<object, bool> criteria)
        {
            foreach (var item in LoadAll(type).Where(criteria).ToList())
                DeleteInternal(type, item.MappingId);
        }

        public object Load(Type type, object itemId)
        {
            return LoadInternal(type, itemId);
        }

        public T Load<T>(object itemId) where T: object
        {
            return (T)LoadInternal(typeof(T), itemId);
        }

        public T Load<T>(Func<T, bool> criteria) where T : object
        {
            return LoadAllInternal(typeof(T)).OfType<T>().FirstOrDefault(criteria);
        }

        public IEnumerable<object> LoadAll(Type type)
        {
            return LoadAllInternal(type);
        }

        public IEnumerable<T> LoadAll<T>() where T: object
        {
            return LoadAllInternal(typeof(T)).OfType<T>();
        }

        public IEnumerable<T> LoadAll<T>(Func<T, bool> criteria) where T: object
        {
            return LoadAllInternal(typeof(T)).OfType<T>().Where(criteria);
        }

        public IEnumerable<object> InitializeAll(Type type)
        {
            return TypeResolver.GetSingle<IDataStoreLoadAllHandler>().LoadAllInternal(type);
        }

        internal protected virtual void SaveInternal(Type type, object item)
        {
            TypeResolver.GetSingle<IDataStoreSaveHandler>().SaveInternal(type, item);
        }

        internal protected virtual void DeleteInternal(Type type, object mappingId)
        {
            TypeResolver.GetSingle<IDataStoreDeleteHandler>().DeleteInternal(type, mappingId);
        }

        internal protected virtual IEnumerable<object> LoadAllInternal(Type type)
        {
            return TypeResolver.GetSingle<IDataStoreLoadAllHandler>().LoadAllInternal(type);
        }

        internal protected virtual object LoadInternal(Type type, object mappingId)
        {
            return TypeResolver.GetSingle<IDataStoreLoadHandler>().LoadInternal(type, mappingId);
        }

        internal protected virtual string GetTargetLocation(Type type)
        {
            return TypeResolver.GetSingle<IDataStoreDirectoryHandler>().GetTypeDirectory(type);
        }
    }
}