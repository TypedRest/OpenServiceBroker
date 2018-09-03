using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceProvisionRequest : ServiceInstanceBase, IEquatable<ServiceInstanceProvisionRequest>
    {
        [JsonProperty("context")]
        public JObject Context { get; set; }

        [JsonProperty("organization_id")]
        [Obsolete("Deprecated in favor of " + nameof(Context))]
        public string OrganizationId { get; set; }

        [JsonProperty("space_id")]
        [Obsolete("Deprecated in favor of " + nameof(Context))]
        public string SpaceId { get; set; }

        public bool Equals(ServiceInstanceProvisionRequest other) => base.Equals(other);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceProvisionRequest)obj);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
