using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Metadata for a Service Binding. This is mainly used to manage the Service Binding itself and SHOULD NOT contain any data that is needed to connect to the Service Instance.
/// </summary>
public class ServiceBindingMetadata : IEquatable<ServiceBindingMetadata>
{
    /// <summary>
    /// The date and time when the Service Binding becomes invalid and SHOULD NOT or CANNOT be used anymore.
    /// </summary>
    [JsonProperty("expires_at", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    /// The date and time before the Service Binding SHOULD be renewed. Applications or Platforms MAY use this field to initiate a Service Binding rotation (see <see cref="ServiceBindingRequest.PredecessorBindingId"/>) or create a new Service Binding on time.
    /// If <see cref="ExpiresAt"/> is also present, this MUST be before or equal to <see cref="ExpiresAt"/>.
    /// </summary>
    [JsonProperty("renew_before", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? RenewBefore { get; set; }

    public bool Equals(ServiceBindingMetadata other)
        => other != null
        && ExpiresAt == other.ExpiresAt
        && RenewBefore == other.RenewBefore;

    public override bool Equals(object obj) => obj is ServiceBindingMetadata other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (ExpiresAt.GetHashCode() * 397) ^ RenewBefore.GetHashCode();
        }
    }
}
