using System.Collections.Generic;
using System;
using Newtonsoft.Json.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace TriggerSol.JStore
{
    public class JStoreException : Exception
    {
        IDataStoreExecutionHandlerBase _Handler;

        public IDataStoreExecutionHandlerBase Handler
        {
            get
            {
                return _Handler;
            }
        }

        public JStoreException()
        {
        }

        public JStoreException(string message) : base(message)
        {
        }

        public JStoreException(string message, Exception inner) : base(message, inner)
        {
        }

        public JStoreException(string message, Exception inner, IDataStoreExecutionHandlerBase handler) : base(message, inner)
        {
            this._Handler = handler;
        }
    }
}
