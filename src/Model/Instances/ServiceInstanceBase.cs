using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Instances
{
    public abstract class ServiceInstanceBase : IServicePlanReference
    {
        /// <summary>
        /// MUST be the ID of a <see cref="Catalogs.Service"/> from the <see cref="Catalogs.Catalog"/> for this Service Broker.
        /// </summary>
        [JsonProperty("service_id", Required = Required.Always)]
        public string ServiceId { get; set; }

        /// <summary>
        /// MUST be the ID of a <see cref="Catalogs.Plan"/> from the service that has been requested.
        /// </summary>
        [JsonProperty("plan_id", Required = Required.Always)]
        public string PlanId { get; set; }

        /// <summary>
        /// Configuration parameters for the Service Instance. Service Brokers SHOULD ensure that the client has provided valid configuration parameters and values for the operation.
        /// </summary>
        [JsonProperty("parameters")]
        public JObject Parameters { get; set; }

        protected bool Equals(ServiceInstanceBase other)
            => ServiceId == other.ServiceId
            && PlanId == other.PlanId;

        public override bool Equals(object obj) => obj is ServiceInstanceBase other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ServiceId?.GetHashCode() ?? 0) * 397) ^ (PlanId?.GetHashCode() ?? 0);
            }
        }
    }
}
