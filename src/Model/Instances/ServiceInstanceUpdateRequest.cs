using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceUpdateRequest : ServiceInstanceProvisionRequest, IEquatable<ServiceInstanceUpdateRequest>
    {
        /// <summary>
        /// Information about the Service Instance prior to the update.
        /// </summary>
        [JsonProperty("previous_values")]
        public ServiceInstancePreviousValues PreviousValues { get; set; }

        public bool Equals(ServiceInstanceUpdateRequest other)
            => other != null
            && base.Equals(other)
            && Equals(PreviousValues, other.PreviousValues);

        public override bool Equals(object obj) => obj is ServiceInstanceUpdateRequest other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (PreviousValues?.GetHashCode() ?? 0);
            }
        }
    }
}
