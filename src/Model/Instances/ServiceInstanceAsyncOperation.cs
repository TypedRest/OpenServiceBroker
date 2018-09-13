using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstanceAsyncOperation : AsyncOperation, ICompletableWithResult<ServiceInstanceProvision>, IEquatable<ServiceInstanceAsyncOperation>
    {
        /// <summary>
        /// The URL of a web-based management user interface for the Service Instance; we refer to this as a service dashboard. The URL MUST contain enough information for the dashboard to identify the resource being accessed.
        /// </summary>
        [JsonProperty("dashboard_url")]
        public Uri DashboardUrl { get; set; }

        [JsonIgnore]
        public ServiceInstanceProvision Result { get; set; }

        public bool Equals(ServiceInstanceAsyncOperation other)
        {
            if (other == null) return false;
            return base.Equals(other) && DashboardUrl == other.DashboardUrl && Equals(Result, other.Result);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceAsyncOperation)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (DashboardUrl != null ? DashboardUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Result?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
