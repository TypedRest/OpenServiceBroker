using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceResource : ServiceInstanceBase, IEquatable<ServiceInstanceResource>
    {
        /// <summary>
        /// The URL of a web-based management user interface for the Service Instance; we refer to this as a service dashboard. The URL MUST contain enough information for the dashboard to identify the resource being accessed.
        /// </summary>
        [JsonProperty("dashboard_url")]
        public Uri DashboardUrl { get; set; }

        public bool Equals(ServiceInstanceResource other)
        {
            if (other == null) return false;
            return base.Equals(other) && DashboardUrl == other.DashboardUrl;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceResource)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (DashboardUrl != null ? DashboardUrl.GetHashCode() : 0);
            }
        }
    }
}
