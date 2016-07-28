using System;
using System.Linq;
using Newtonsoft.Json;
using TriggerSol.Dependency;
using Newtonsoft.Json.Serialization;

namespace TriggerSol.JStore
{
    public class JsonObjectCloner
    {
        //public static T CloneObject<T>(T obj)
        //{
        //    var clone = CloneObject(obj);
        //    return clone;
        //}

        public static object CloneObject(object obj)
        {
            var settings = (JsonSerializerSettings)DependencyResolverProvider.Current.GetObject<IJsonSerializerSettings>();

            var json = JsonConvert.SerializeObject(obj, obj.GetType(), settings);
            var clone = JsonConvert.DeserializeObject(json, obj.GetType());

            return clone;
        }
    }
}
