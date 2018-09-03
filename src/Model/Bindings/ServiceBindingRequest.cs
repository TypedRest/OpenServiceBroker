using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingRequest : IServicePlanReference, IEquatable<ServiceBindingRequest>
    {
        [JsonProperty("service_id", Required = Required.Always)]
        public string ServiceId { get; set; }

        [JsonProperty("plan_id", Required = Required.Always)]
        public string PlanId { get; set; }

        [JsonProperty("parameters")]
        public JObject Parameters { get; set; }

        [JsonProperty("context")]
        public JObject Context { get; set; }

        [JsonProperty("app_guid")]
        [Obsolete("Deprecated in favor of " + nameof(BindResource) + "." + nameof(ServiceBindingResouceObject.AppGuid))]
        public string AppGuid { get; set; }

        [JsonProperty("bind_resource")]
        public ServiceBindingResouceObject BindResource { get; set; }

        public bool Equals(ServiceBindingRequest other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ServiceId, other.ServiceId) && string.Equals(PlanId, other.PlanId) && Equals(BindResource, other.BindResource);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ServiceBindingRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ServiceId != null ? ServiceId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PlanId != null ? PlanId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (BindResource != null ? BindResource.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
