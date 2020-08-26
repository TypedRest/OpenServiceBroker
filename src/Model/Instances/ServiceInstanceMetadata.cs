using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// Metadata for a Service Instance.
    /// </summary>
    public class ServiceInstanceMetadata
    {
        /// <summary>
        /// Broker specified key-value pairs specifying attributes of Service Instances that are meaningful and relevant to Platform users, but do not directly imply behaviour changes by the Platform.
        /// </summary>
        public JObject Labels { get; set; }
    }
}
