using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs;

/// <summary>
/// The schema definitions for creating and updating a Service Instance.
/// </summary>
public class ServiceInstanceSchema
{
    /// <summary>
    /// The schema definition for creating a Service Instance.
    /// </summary>
    [JsonProperty("create")]
    public SchemaParameters Create { get; set; }

    /// <summary>
    /// The schema definition for updating a Service Instance.
    /// </summary>
    [JsonProperty("update")]
    public SchemaParameters Update { get; set; }
}
