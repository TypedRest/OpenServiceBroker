using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceProvision : IUnchangedFlag, IEquatable<ServiceInstanceProvision>
    {
        [JsonProperty("dashboard_url")]
        public Uri DashboardUrl { get; set; }

        [JsonIgnore]
        public bool Unchanged { get; set; }

        public bool Equals(ServiceInstanceProvision other)
        {
            if (other == null) return false;
            return DashboardUrl == other.DashboardUrl && Unchanged == other.Unchanged;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceProvision)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DashboardUrl != null ? DashboardUrl.GetHashCode() : 0) * 397) ^ Unchanged.GetHashCode();
            }
        }
    }
}
