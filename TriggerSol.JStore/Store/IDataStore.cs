//
// IDataStore.cs
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

using System.Collections.Generic;
using System;

namespace TriggerSol.JStore
{
    public interface IDataStore
    {
        void Save(Type type, IPersistentBase item);

        void Save<T>(T item) where T: IPersistentBase;

        void DeleteById(Type type, object itemId);

        void DeleteById<T>(object itemId) where T: IPersistentBase;

        void Delete(Type type, IPersistentBase item);

        void Delete<T>(T item) where T: IPersistentBase;

        void Delete<T>(Func<T, bool> criteria) where T: IPersistentBase;

        void Delete(Type type, Func<IPersistentBase, bool> criteria);

        IPersistentBase Load(Type type, object itemId);

        T Load<T>(object itemId) where T: IPersistentBase;

        T Load<T>(Func<T, bool> criteria) where T: IPersistentBase;

        IEnumerable<IPersistentBase>LoadAll(Type type);

        IEnumerable<T> LoadAll<T>() where T: IPersistentBase;

        IEnumerable<T> LoadAll<T>(Func<T, bool> criteria) where T: IPersistentBase;

        IEnumerable<IPersistentBase> InitializeAll(Type type);
    }
}
