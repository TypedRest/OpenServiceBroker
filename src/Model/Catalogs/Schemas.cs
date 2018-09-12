using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// Schema definitions for Service Instances and bindings for a plan.
    /// </summary>
    public class Schemas
    {
        /// <summary>
        /// The schema definitions for creating and updating a Service Instance.
        /// </summary>
        [JsonProperty("service_instance")]
        public ServiceInstanceSchema ServiceInstance { get; set; }

        /// <summary>
        /// The schema definition for creating a Service Binding. Used only if the Service Plan is bindable.
        /// </summary>
        [JsonProperty("service_binding")]
        public ServiceBindingSchema ServiceBinding { get; set; }
    }
}
