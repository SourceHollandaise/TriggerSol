//
// DependencyResolverProvider.cs
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

namespace TriggerSol.Dependency
{
    public static class DependencyResolverProvider
    {
        readonly static object _Locker = new object();
		
        static IDependencyResolver _Instance;

        static Type _CustomResolverType;

        public static void RegisterCustomResolver<T>() where T: IDependencyResolver
        {
            if (_CustomResolverType == null)
                _CustomResolverType = typeof(T);
        }

        public static IDependencyResolver Instance
        {
            get
            {
                lock (_Locker)
                {
                    if (_Instance == null)
                        _Instance = _CustomResolverType == null ? new DependencyResolver() : Activator.CreateInstance(_CustomResolverType) as IDependencyResolver;
                    return _Instance;
                }
            }
        }

        public static void Destroy()
        {
            _Instance = null;
            _CustomResolverType = null;
        }
    }
}

