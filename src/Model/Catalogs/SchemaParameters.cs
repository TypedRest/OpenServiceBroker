using Newtonsoft.Json;
using NJsonSchema;

namespace OpenServiceBroker.Catalogs;

/// <summary>
/// A schema definition for the input parameters.
/// </summary>
public class SchemaParameters
{
    /// <summary>
    /// The schema definition for the input parameters. Each input parameter is expressed as a property within a JSON object.
    /// </summary>
    [JsonProperty("parameters")]
    public JsonSchema Parameters { get; set; }
}
