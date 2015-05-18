using System;

namespace TriggerSol.Dependency
{
    public static class TypeProvider
    {
        readonly static object _lock = new object();
		
        static ITypeResolver resolver;

        static Type customTypeResolver;

        public static void RegisterCustomResolver<T>() where T: ITypeResolver
        {
            if (customTypeResolver == null)
                customTypeResolver = typeof(T);
        }

        public static ITypeResolver Current
        {
            get
            {
                lock (_lock)
                {
                    if (resolver == null)
                        resolver = customTypeResolver == null ? new TypeResolver() : Activator.CreateInstance(customTypeResolver) as ITypeResolver;
                    return resolver;
                }
            }
        }

        public static void Destroy()
        {
            resolver = null;
            customTypeResolver = null;
        }
    }
}

