using System;
using TriggerSol.Dependency;
using TriggerSol.JStore;
using System.IO;

namespace TriggerSol.JStore
{
    public class DataStoreConfiguration : IDataStoreConfiguration
    {
        private string _dataStoreLocation;
        private string _documentStoreLocation;
        private string _dataStorePath;

        public DataStoreConfiguration(string dataStorePath)
        {
            this._dataStorePath = dataStorePath;
        }

        public void InitStore()
        {
            _dataStoreLocation = Path.Combine(_dataStorePath, "DATA");
            _documentStoreLocation = Path.Combine(_dataStorePath, "DOC");

            if (!Directory.Exists(_dataStoreLocation))
                Directory.CreateDirectory(_dataStoreLocation);

            if (!Directory.Exists(_documentStoreLocation))
                Directory.CreateDirectory(_documentStoreLocation);
        }

        public string DataStoreLocation
        {
            get
            {
                return _dataStoreLocation;
            }
        }

        public string DocumentStoreLocation
        {
            get
            {
                return _documentStoreLocation;
            }
        }
    }
}
