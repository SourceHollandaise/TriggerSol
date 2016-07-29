using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    public class JsonObjectCloner
    {
        public static IPersistentBase CloneObject(IPersistentBase obj)
        {
            var settings = (JsonSerializerSettings)DependencyResolverProvider.Instance.ResolveObject<IJsonSerializerSettings>();

            var json = JsonConvert.SerializeObject(obj, obj.GetType(), settings);
            return JsonConvert.DeserializeObject(json, obj.GetType()) as IPersistentBase;
        }
    }
}
