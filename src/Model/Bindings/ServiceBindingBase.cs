using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings;

public abstract class ServiceBindingBase
{
    /// <summary>
    /// An optional object containing metadata about this Service Binding, such as its expiration date.
    /// </summary>
    [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
    public ServiceBindingMetadata Metadata { get; set; }

    /// <summary>
    /// A free-form hash of credentials that can be used by applications or users to access the service. MUST be returned if the Service Broker supports generation of credentials.
    /// </summary>
    [JsonProperty("credentials", NullValueHandling = NullValueHandling.Ignore)]
    public JObject Credentials { get; set; }

    /// <summary>
    /// A URL to which logs MUST be streamed. <see cref="Catalogs.Service.Requires"/>: <see cref="Catalogs.Features.SyslogDrain"/> MUST be declared in the Catalog endpoint or the Platform can consider the response invalid.
    /// </summary>
    [JsonProperty("syslog_drain_url", NullValueHandling = NullValueHandling.Ignore)]
    public Uri SyslogDrainUrl { get; set; }

    /// <summary>
    /// A URL to which the Platform MUST proxy requests for the address sent with <see cref="ServiceBindingResourceObject.Route"/> in the request body. <see cref="Catalogs.Service.Requires"/>: <see cref="Catalogs.Features.RouteForwarding"/> MUST be declared in the Catalog endpoint or the Platform can consider the response invalid.
    /// </summary>
    [JsonProperty("route_service_url", NullValueHandling = NullValueHandling.Ignore)]
    public Uri RouteServiceUrl { get; set; }

    /// <summary>
    /// An array of configuration for remote storage devices to be mounted into an application container filesystem. <see cref="Catalogs.Service.Requires"/>: <see cref="Catalogs.Features.VolumeMount"/> MUST be declared in the Catalog endpoint or the Platform can consider the response invalid.
    /// </summary>
    [JsonProperty("volume_mounts")]
    public List<ServiceBindingVolumeMount> VolumeMounts { get; } = new();

    /// <summary>
    /// Do not serialize volume mounts if there are none. On some platforms (PCF for example), binding credentials to
    /// an application fails when an empty list of volume mounts is returned.
    /// </summary>
    /// <returns></returns>
    public bool ShouldSerializeVolumeMounts() => VolumeMounts.Any();

    protected bool Equals(ServiceBindingBase other)
        => SyslogDrainUrl == other.SyslogDrainUrl
        && RouteServiceUrl == other.RouteServiceUrl
        && Equals(Metadata, other.Metadata);

    public override bool Equals(object obj) => obj is ServiceBindingBase other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = SyslogDrainUrl != null ? SyslogDrainUrl.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (RouteServiceUrl != null ? RouteServiceUrl.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Metadata?.GetHashCode() ?? 0);
            return hashCode;
        }
    }
}
