//
// DataStoreProvider.cs
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
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    public static class DataStoreProvider
    {
        static IDependencyResolver _DependencyResolver;
        internal static IDependencyResolver DependencyResolver
        {
            get
            {
                if (_DependencyResolver == null)
                    _DependencyResolver = DependencyResolverProvider.Current;
                return _DependencyResolver;
            }
        }

        public static void ExplicitCaching<T>() where T : IPersistentBase => CacheProvider.StartCaching<T>();

        public static void ExplicitCaching(Type type) => CacheProvider.StartCaching(type);

        public static void RegisterStore<T>() where T : IDataStore, new()
        {
            if (DependencyResolver.GetSingle<IDataStore>() == null)
                DependencyResolver.RegisterSingle<IDataStore>(Activator.CreateInstance<T>());
        }

        public static void RegisterDirectoryHandler<T>() where T : IDataStoreDirectoryHandler, new()
        {
            if (DependencyResolver.GetSingle<IDataStoreDirectoryHandler>() == null)
                DependencyResolver.RegisterSingle<IDataStoreDirectoryHandler>(Activator.CreateInstance<T>());
        }

        public static void RegisterDeleteHandler<T>() where T : IDataStoreDeleteHandler, new()
        {
            if (DependencyResolver.GetSingle<IDataStoreDeleteHandler>() == null)
                DependencyResolver.RegisterSingle<IDataStoreDeleteHandler>(Activator.CreateInstance<T>());
        }

        public static void RegisterSaveHandler<T>() where T : IDataStoreSaveHandler, new()
        {
            if (DependencyResolver.GetSingle<IDataStoreSaveHandler>() == null)
                DependencyResolver.RegisterSingle<IDataStoreSaveHandler>(Activator.CreateInstance<T>());
        }

        public static void RegisterLoadHandler<T>() where T : IDataStoreLoadHandler, new()
        {
            if (DependencyResolver.GetSingle<IDataStoreLoadHandler>() == null)
                DependencyResolver.RegisterSingle<IDataStoreLoadHandler>(Activator.CreateInstance<T>());
        }

        public static void RegisterLoadAllHandler<T>() where T : IDataStoreLoadAllHandler, new()
        {
            if (DependencyResolver.GetSingle<IDataStoreLoadAllHandler>() == null)
                DependencyResolver.RegisterSingle<IDataStoreLoadAllHandler>(Activator.CreateInstance<T>());
        }
    }
}