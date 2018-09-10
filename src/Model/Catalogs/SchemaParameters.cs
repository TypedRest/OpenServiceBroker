using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace OpenServiceBroker.Catalogs
{
    public class SchemaParameters
    {
        [JsonProperty("parameters")]
#pragma warning disable 618
        public JsonSchema Parameters { get; set; }
    }
}
