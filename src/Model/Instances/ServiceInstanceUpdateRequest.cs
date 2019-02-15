using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceUpdateRequest : ServiceInstanceBase, IEquatable<ServiceInstanceUpdateRequest>
    {
        /// <summary>
        /// Contextual data under which the Service Instance is created.
        /// </summary>
        [JsonProperty("context")]
        public JObject Context { get; set; }

        /// <summary>
        /// If present, MUST be the ID of a <see cref="Catalogs.Plan"/> from the service that has been requested. If this field is not present in the request message, then the Service Broker MUST NOT change the plan of the instance as a result of this request.
        /// </summary>
        [JsonProperty("plan_id")]
        public override string PlanId { get; set; }

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
