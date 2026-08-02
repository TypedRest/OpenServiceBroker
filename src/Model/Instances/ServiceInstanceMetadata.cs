using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Metadata for a Service Instance.
/// </summary>
public class ServiceInstanceMetadata : IEquatable<ServiceInstanceMetadata>
{
    /// <summary>
    /// Broker specified key-value pairs specifying attributes of Service Instances that are meaningful and relevant to Platform users, but do not directly imply behaviour changes by the Platform.
    /// </summary>
    [JsonProperty("labels", NullValueHandling = NullValueHandling.Ignore)]
    public JObject Labels { get; set; }

    public bool Equals(ServiceInstanceMetadata other)
        => other != null
        && JToken.DeepEquals(Labels, other.Labels);

    public override bool Equals(object obj) => obj is ServiceInstanceMetadata other && Equals(other);

    public override int GetHashCode() => Labels?.GetHashCode() ?? 0;
}
