using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceUpdateRequest : ServiceInstanceProvisionRequest, IEquatable<ServiceInstanceUpdateRequest>
    {
        [JsonProperty("previous_values")]
        public ServiceInstancePreviousValues PreviousValues { get; set; }

        public bool Equals(ServiceInstanceUpdateRequest other)
        {
            if (other == null) return false;
            return base.Equals(other) && Equals(PreviousValues, other.PreviousValues);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceUpdateRequest)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (PreviousValues != null ? PreviousValues.GetHashCode() : 0);
            }
        }
    }
}
