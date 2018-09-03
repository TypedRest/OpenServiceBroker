using Newtonsoft.Json;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Instances;

namespace OpenServiceBroker.Catalogs
{
    public class Schemas
    {
        [JsonProperty("service_instance")]
        public ServiceInstanceSchema ServiceInstance { get; set; }

        [JsonProperty("service_binding")]
        public ServiceBindingSchema ServiceBinding { get; set; }
    }
}
