using Newtonsoft.Json;

namespace OpenServiceBroker.Instances;

public class ServiceInstanceResource : ServiceInstanceBase, IEquatable<ServiceInstanceResource>
{
    /// <summary>
    /// The ID of the <see cref="Catalogs.Plan"/> from the catalog that is associated with the Service Instance.
    /// </summary>
    [JsonProperty("plan_id")]
    public override string PlanId { get; set; }

    /// <summary>
    /// The URL of a web-based management user interface for the Service Instance; we refer to this as a service dashboard. The URL MUST contain enough information for the dashboard to identify the resource being accessed.
    /// </summary>
    [JsonProperty("dashboard_url")]
    public Uri DashboardUrl { get; set; }

    public bool Equals(ServiceInstanceResource other)
        => other != null
        && base.Equals(other)
        && DashboardUrl == other.DashboardUrl;

    public override bool Equals(object obj) => obj is ServiceInstanceResource other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (DashboardUrl != null ? DashboardUrl.GetHashCode() : 0);
        }
    }
}
