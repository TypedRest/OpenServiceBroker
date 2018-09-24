using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceProvision : IUnchangedFlag, IEquatable<ServiceInstanceProvision>
    {
        /// <summary>
        /// The URL of a web-based management user interface for the Service Instance; we refer to this as a service dashboard. The URL MUST contain enough information for the dashboard to identify the resource being accessed.
        /// </summary>
        [JsonProperty("dashboard_url")]
        public Uri DashboardUrl { get; set; }

        [JsonIgnore]
        public bool Unchanged { get; set; }

        public bool Equals(ServiceInstanceProvision other)
            => other != null
            && DashboardUrl == other.DashboardUrl
            && Unchanged == other.Unchanged;

        public override bool Equals(object obj) => obj is ServiceInstanceProvision other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DashboardUrl != null ? DashboardUrl.GetHashCode() : 0) * 397) ^ Unchanged.GetHashCode();
            }
        }
    }
}
