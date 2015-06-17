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
        public static IDataStore DataStore
        {
            get
            {
                var store = TypeProvider.Current.GetSingle<IDataStore>();
                if (store == null)
                    throw new ArgumentNullException("Store", "No datastore registered!");

                return store;
            }
        }

        public static void ExplicitCaching<T>() where T: object
        {
            CacheProvider.StartCaching<T>();
        }

        public static void ExplicitCaching(Type type)
        {
            CacheProvider.StartCaching(type);
        }

        public static void RegisterStore<T>() where T: IDataStore, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStore>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStore>(instance);
            }
        }

        public static void RegisterDirectoryHandler<T>() where T: IDataStoreDirectoryHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreDirectoryHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreDirectoryHandler>(instance);
            }
        }

        public static void RegisterDeleteHandler<T>() where T: IDataStoreDeleteHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreDeleteHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreDeleteHandler>(instance);
            }
        }

        public static void RegisterSaveHandler<T>() where T: IDataStoreSaveHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreSaveHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreSaveHandler>(instance);
            }
        }

        public static void RegisterLoadHandler<T>() where T: IDataStoreLoadHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreLoadHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreLoadHandler>(instance);
            }
        }

        public static void RegisterLoadAllHandler<T>() where T: IDataStoreLoadAllHandler, new()
        {
            if (TypeProvider.Current.GetSingle<IDataStoreLoadAllHandler>() == null)
            {
                var instance = Activator.CreateInstance<T>();

                TypeProvider.Current.RegisterSingle<IDataStoreLoadAllHandler>(instance);
            }
        }
    }
}