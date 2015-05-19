using System.Collections.Generic;
using System;
using Newtonsoft.Json.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace TriggerSol.JStore
{
    public class JsonWritablePropertiesContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => p.Writable).ToList();
        }
    }
}
