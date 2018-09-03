using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    public class ServiceInstanceSchema
    {
        [JsonProperty("create")]
        public SchemaParameters Create { get; set; }

        [JsonProperty("update")]
        public SchemaParameters Update { get; set; }
    }
}
