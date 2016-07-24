//
// TypeResolver.cs
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

namespace TriggerSol.Dependency
{
    internal sealed class TypeResolver : ITypeResolver
    {
        Dictionary<Type, object> _registeredInstances = new Dictionary<Type, object>();
        Dictionary<Type, Type> _registeredTypes = new Dictionary<Type, Type>();

        public void RegisterSingle<T>(object instance)
        {
            RegisterSingle(typeof(T), instance);
        }

        public void RegisterSingle(Type type, object instance)
        {
            if (instance == null)
                throw new NullReferenceException("Instance to register is null!");

            if (!_registeredInstances.ContainsKey(type))
                _registeredInstances.Add(type, instance);
        }

        public T GetSingle<T>() => (T)GetSingle(typeof(T));
        
        public object GetSingle(Type type) => _registeredInstances.ContainsKey(type) ? _registeredInstances[type] : null;
 
        public void ClearSingle<T>() => ClearSingle(typeof(T));
        
        public void ClearSingle(Type type)
        {
            if (_registeredInstances.ContainsKey(type))
                _registeredInstances.Remove(type);
        }

        public void ClearRegisteredSingles() => _registeredInstances.Clear();
        
        public void RegisterObjectType<T, U>() => RegisterObjectType(typeof(T), typeof(U));
        
        public void RegisterObjectType(Type interfaceType, Type classType)
        {
            if (_registeredTypes.ContainsKey(interfaceType))
                _registeredTypes.Remove(interfaceType);

            _registeredTypes.Add(interfaceType, classType);
        }

        public T GetObject<T>(params object[] args) =>  (T)GetObject(typeof(T), args);

        public object GetObject(Type type, params object[] args)
        {
            if (_registeredTypes.ContainsKey(type))
            {
                var targetType = _registeredTypes[type];

                return Activator.CreateInstance(targetType, args);
            }

            throw new ArgumentException(string.Format("Cannot create an instance from type '{0}'", type));
        }

        public void UnregisterObjectType<T>() => UnregisterObjectType(typeof(T));
        
        public void UnregisterObjectType(Type type)
        {
            if (_registeredTypes.ContainsKey(type))
                _registeredTypes.Remove(type);
        }

        public void ClearObjectTypes() => _registeredTypes.Clear();
    }
}
