using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs;

/// <summary>
/// A list of all services available on the Service Broker.
/// </summary>
public class Catalog
{
    /// <summary>
    /// A list of all services available on the Service Broker.
    /// </summary>
    [JsonProperty("services", Required = Required.Always)]
    public List<Service> Services { get; } = new();
}
