using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstancePreviousValues : IEquatable<ServiceInstancePreviousValues>
    {
        [JsonProperty("service_id")]
        [Obsolete("Deprecated; determined to be unnecessary as the value is immutable")]
        public string ServiceId { get; set; }

        [JsonProperty("plan_id")]
        public string PlanId { get; set; }

        [JsonProperty("organization_id")]
        [Obsolete("Deprecated as it was redundant information")]
        public string OrganizationId { get; set; }

        [JsonProperty("space_id")]
        [Obsolete("Deprecated as it was redundant information")]
        public string SpaceId { get; set; }

        public bool Equals(ServiceInstancePreviousValues other)
            => other != null && string.Equals(PlanId, other.PlanId);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstancePreviousValues)obj);
        }

        public override int GetHashCode() => (PlanId != null ? PlanId.GetHashCode() : 0);
    }
}
