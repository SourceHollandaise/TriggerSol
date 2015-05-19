using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PersistentNameAttribute : Attribute
    {
        string _PersistentName;

        public string PersistentName
        {
            get
            {
                return _PersistentName;
            }
        }

        public PersistentNameAttribute(string persistentName)
        {
            this._PersistentName = persistentName;
        }
    }
}
