//
// MemoryDataStore.cs
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
    public class MemoryDataStore : IDataStore, IMemoryStore
    {
        readonly Dictionary<Type, Dictionary<object, object>> _repository = new Dictionary<Type, Dictionary<object, object>>();

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
            DeleteInternal(typeof(T), item);
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
            return LoadAllInternal(type);
        }

        internal protected virtual void SaveInternal(Type type, object item)
        {
            var valueStore = GetValueStoreOfType(type);

            if (item.MappingId == null)
                item.MappingId = new GuidIdGenerator().GetId();

            var clone = item.Clone();
            clone.MappingId = item.MappingId;

            if (valueStore.ContainsKey(clone.MappingId))
                valueStore[clone.MappingId] = clone;
            else
                valueStore.Add(clone.MappingId, clone);
        }

        internal protected virtual void DeleteInternal(Type type, object itemId)
        {
            if (!_repository.ContainsKey(type))
                return;

            if (_repository[type].ContainsKey(itemId))
                _repository[type].Remove(itemId);
        }

        internal protected virtual IEnumerable<object> LoadAllInternal(Type type)
        {
            return !_repository.ContainsKey(type) ? Enumerable.Empty<object>() : _repository[type].Values;
        }

        internal protected virtual object LoadInternal(Type type, object itemId)
        {
            if (!_repository.ContainsKey(type))
                return null;

            return _repository[type].ContainsKey(itemId) ? _repository[type][itemId] : null;
        }

        internal protected virtual Dictionary<object, object> GetValueStoreOfType(Type type)
        {
            if (!_repository.ContainsKey(type))
                _repository.Add(type, new Dictionary<object, object>());

            return _repository[type];
        }
    }
}