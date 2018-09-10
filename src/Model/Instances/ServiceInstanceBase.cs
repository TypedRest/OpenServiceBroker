using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Instances
{
    public abstract class ServiceInstanceBase : IServicePlanReference
    {
        [JsonProperty("service_id", Required = Required.Always)]
        public string ServiceId { get; set; }

        [JsonProperty("plan_id", Required = Required.Always)]
        public string PlanId { get; set; }

        [JsonProperty("parameters")]
        public JObject Parameters { get; set; }

        protected bool Equals(ServiceInstanceBase other)
            => ServiceId == other.ServiceId && PlanId == other.PlanId;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ServiceId != null ? ServiceId.GetHashCode() : 0) * 397) ^ (PlanId != null ? PlanId.GetHashCode() : 0);
            }
        }
    }
}
