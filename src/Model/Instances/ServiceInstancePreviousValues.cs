using Newtonsoft.Json;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Information about a Service Instance prior to the update.
/// </summary>
public class ServiceInstancePreviousValues : IEquatable<ServiceInstancePreviousValues>
{
    [JsonProperty("service_id")]
    [Obsolete("Deprecated; determined to be unnecessary as the value is immutable")]
    public string ServiceId { get; set; }

    /// <summary>
    ///  	If present, it MUST be the ID of the plan prior to the update.
    /// </summary>
    [JsonProperty("plan_id")]
    public string PlanId { get; set; }

    [JsonProperty("organization_id")]
    [Obsolete("Deprecated as it was redundant information")]
    public string OrganizationId { get; set; }

    [JsonProperty("space_id")]
    [Obsolete("Deprecated as it was redundant information")]
    public string SpaceId { get; set; }

    public bool Equals(ServiceInstancePreviousValues other)
        => other != null
        && PlanId == other.PlanId;

    public override bool Equals(object obj) => obj is ServiceInstancePreviousValues other && Equals(other);

    public override int GetHashCode() => PlanId?.GetHashCode() ?? 0;
}
