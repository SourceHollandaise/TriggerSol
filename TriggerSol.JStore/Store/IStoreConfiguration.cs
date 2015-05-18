
namespace TriggerSol.JStore
{
    public interface IStoreConfiguration
    {
        string DataStoreLocation { get; }

        string DocumentStoreLocation { get; }

        void InitStore();
    }
}
