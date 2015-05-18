
namespace TriggerSol.Dependency
{
    public abstract class DependencyObject : IDependencyObject
    {
        ITypeResolver _typeResolver;

        public ITypeResolver TypeResolver
        {
            get
            {
                if (_typeResolver == null)
                    _typeResolver = TypeProvider.Current;
                return _typeResolver;
            }
        }
    }
}
