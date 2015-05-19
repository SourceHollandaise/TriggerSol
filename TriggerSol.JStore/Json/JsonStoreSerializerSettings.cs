
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TriggerSol.Dependency;

namespace TriggerSol.JStore
{
    public class JsonStoreSerializerSettings : JsonSerializerSettings, IJsonSerializerSettings
    {
        public JsonStoreSerializerSettings()
        {
            this.ContractResolver = TypeProvider.Current.GetObject<IContractResolver>();
            this.Formatting = Formatting.None;
            this.MissingMemberHandling = MissingMemberHandling.Ignore;
            this.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
