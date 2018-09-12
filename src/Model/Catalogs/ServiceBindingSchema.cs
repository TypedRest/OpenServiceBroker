using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// The schema definitions for creating and updating a Service Instance.
    /// </summary>
    public class ServiceBindingSchema
    {
        /// <summary>
        /// The schema definition for creating a Service Binding.
        /// </summary>
        [JsonProperty("create")]
        public SchemaParameters Create { get; set; }
    }
}
