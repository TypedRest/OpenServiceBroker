using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingResource : ServiceBindingBase, IEquatable<ServiceBindingResource>
    {
        /// <summary>
        /// Configuration parameters for the Service Binding.
        /// </summary>
        [JsonProperty("parameters")]
        public JObject Parameters { get; set; }

        public bool Equals(ServiceBindingResource other) => base.Equals(other);

        public override bool Equals(object obj) => obj is ServiceBindingResource other && Equals(other);

        public override int GetHashCode() => base.GetHashCode();
    }
}
