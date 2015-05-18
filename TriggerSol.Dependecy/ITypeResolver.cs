using System;

namespace TriggerSol.Dependency
{
    public interface ITypeResolver
    {
        void RegisterSingle<T>(object instance);

        void RegisterSingle(Type type, object instance);

        T GetSingle<T>();

        object GetSingle(Type type);

        void ClearRegisteredSingles();

        void ClearSingle<T>();

        void ClearSingle(Type type);

        void RegisterObjectType<T, U>();

        void RegisterObjectType(Type interfaceType, Type classType);

        T GetObject<T>(params object[] args);

        object GetObject(Type type, params object[] args);

        void ClearObjectTypes();

        void UnregisterObjectType(Type type);

        void UnregisterObjectType<T>();
    }
}
