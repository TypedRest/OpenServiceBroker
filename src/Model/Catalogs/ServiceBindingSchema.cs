using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    public class ServiceBindingSchema
    {
        [JsonProperty("create")]
        public SchemaParameters Create { get; set; }
    }
}
