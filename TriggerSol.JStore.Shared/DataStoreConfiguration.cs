using System;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using System.IO;

namespace TriggerSol.JStore
{
    public class DataStoreConfiguration : IDataStoreConfiguration
    {
        public DataStoreConfiguration(string dataStorePath)
        {
            InitStore(dataStorePath);
        }

        private string _DataStoreLocation;

        public string DataStoreLocation
        {
            get
            {
                return _DataStoreLocation;
            }
        }

        private string _DocumentStoreLocation;

        public string DocumentStoreLocation
        {
            get
            {
                return _DocumentStoreLocation;
            }
        }

        void InitStore(string dataStorePath)
        {
            _DataStoreLocation = Path.Combine(dataStorePath, "DATA");
            _DocumentStoreLocation = Path.Combine(dataStorePath, "DOC");

            if (!Directory.Exists(_DataStoreLocation))
                Directory.CreateDirectory(_DataStoreLocation);

            if (!Directory.Exists(_DocumentStoreLocation))
                Directory.CreateDirectory(_DocumentStoreLocation);
        }
    }
}
