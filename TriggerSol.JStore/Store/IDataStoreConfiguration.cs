
namespace TriggerSol.JStore
{
    public interface IDataStoreConfiguration
    {
        string DataStoreLocation { get; }

        string DocumentStoreLocation { get; }

        void InitStore();
    }
}
