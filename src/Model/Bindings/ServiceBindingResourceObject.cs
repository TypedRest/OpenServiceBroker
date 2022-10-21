using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings;

public class ServiceBindingResourceObject : IEquatable<ServiceBindingResourceObject>
{
    /// <summary>
    /// GUID of an application associated with the binding. For credentials bindings. MUST be unique within the scope of the Platform.
    /// </summary>
    [JsonProperty("app_guid")]
    public string AppGuid { get; set; }

    /// <summary>
    /// URL of the application to be intermediated. For route services bindings.
    /// </summary>
    [JsonProperty("route")]
    public string Route { get; set; }

    public bool Equals(ServiceBindingResourceObject other)
        => other != null
        && AppGuid == other.AppGuid
        && Route == other.Route;

    public override bool Equals(object obj) => obj is ServiceBindingResourceObject other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((AppGuid?.GetHashCode() ?? 0) * 397) ^ (Route?.GetHashCode() ?? 0);
        }
    }
}
