//
// InMemoryDataStore.cs
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
    public class InMemoryDataStore : DataStoreBase, IInMemoryStore
    {
        readonly Dictionary<Type, Dictionary<object, IPersistentBase>> _Repository = new Dictionary<Type, Dictionary<object, IPersistentBase>>();

        internal protected override void SaveInternal(Type type, IPersistentBase item)
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

        internal protected override void DeleteInternal(Type type, object itemId)
        {
            if (!_Repository.ContainsKey(type))
                return;

            if (_Repository[type].ContainsKey(itemId))
                _Repository[type].Remove(itemId);
        }

        internal protected override IEnumerable<IPersistentBase> LoadAllInternal(Type type) => !_Repository.ContainsKey(type) ? Enumerable.Empty<IPersistentBase>() : _Repository[type].Values;

        internal protected override IPersistentBase LoadInternal(Type type, object itemId)
        {
            if (!_Repository.ContainsKey(type))
                return null;

            return _Repository[type].ContainsKey(itemId) ? _Repository[type][itemId] : null;
        }

        internal protected Dictionary<object, IPersistentBase> GetValueStoreOfType(Type type)
        {
            if (!_Repository.ContainsKey(type))
                _Repository.Add(type, new Dictionary<object, IPersistentBase>());

            return _Repository[type];
        }

        protected internal override string GetTargetLocation(Type type) => null;
    }
}