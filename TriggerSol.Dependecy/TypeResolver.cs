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

        public T GetSingle<T>()
        {
            return (T)GetSingle(typeof(T));
        }

        public object GetSingle(Type type)
        {
            return _registeredInstances.ContainsKey(type) ? _registeredInstances[type] : null;
        }

        public void ClearSingle<T>()
        {
            ClearSingle(typeof(T));
        }

        public void ClearSingle(Type type)
        {
            if (_registeredInstances.ContainsKey(type))
                _registeredInstances.Remove(type);
        }

        public void ClearRegisteredSingles()
        {
            _registeredInstances.Clear();
        }

        public void RegisterObjectType<T, U>()
        {
            RegisterObjectType(typeof(T), typeof(U));
        }

        public void RegisterObjectType(Type interfaceType, Type classType)
        {
            if (_registeredTypes.ContainsKey(interfaceType))
                _registeredTypes.Remove(interfaceType);

            _registeredTypes.Add(interfaceType, classType);
        }

        public T GetObject<T>(params object[] args)
        {
            return (T)GetObject(typeof(T), args);
        }

        public object GetObject(Type type, params object[] args)
        {
            if (_registeredTypes.ContainsKey(type))
            {
                var targetType = _registeredTypes[type];

                var instance = Activator.CreateInstance(targetType, args);

                return instance;
            }

            throw new ArgumentException(string.Format("Cannot create an instance from type '{0}'", type));
        }

        public void UnregisterObjectType<T>()
        {
            UnregisterObjectType(typeof(T));
        }

        public void UnregisterObjectType(Type type)
        {
            if (_registeredTypes.ContainsKey(type))
                _registeredTypes.Remove(type);
        }

        public void ClearObjectTypes()
        {
            _registeredTypes.Clear();
        }
    }
}
