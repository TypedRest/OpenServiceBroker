using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Catalogs;

/// <summary>
/// Describes a service available on the Service Broker.
/// </summary>
public class Service : IEquatable<Service>
{
    /// <summary>
    /// An identifier used to correlate this service in future requests to the Service Broker. This MUST be globally unique such that Platforms (and their users) MUST be able to assume that seeing the same value (no matter what Service Broker uses it) will always refer to this service. MUST be a non-empty string. Using a GUID is RECOMMENDED.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public string Id { get; set; }

    /// <summary>
    /// A CLI-friendly name of the service. MUST only contain alphanumeric characters, periods, and hyphens (no spaces). MUST be unique across all service objects returned in this response. MUST be a non-empty string.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// A short description of the service. MUST be a non-empty string.
    /// </summary>
    [JsonProperty("description", Required = Required.Always)]
    public string Description { get; set; }

    /// <summary>
    /// Tags provide a flexible mechanism to expose a classification, attribute, or base technology of a service, enabling equivalent services to be swapped out without changes to dependent logic in applications, buildpacks, or other services. E.g. mysql, relational, redis, key-value, caching, messaging, amqp.
    /// </summary>
    [JsonProperty("tags")]
    public List<string> Tags { get; } = new();

    /// <summary>
    /// A list of permissions that the user would have to give the service, if they provision it.
    /// </summary>
    [JsonProperty("requires", ItemConverterType = typeof(StringEnumConverter))]
    public List<Features> Requires { get; } = new();

    /// <summary>
    /// Specifies whether Service Instances of the service can be bound to applications. This specifies the default for all plans of this service. Plans can override this field.
    /// </summary>
    /// <seealso cref="Plan"/>
    [JsonProperty("bindable", Required = Required.Always)]
    public bool Bindable { get; set; }

    /// <summary>
    /// Specifies whether the Fetching a Service Instance endpoint is supported for all plans.
    /// </summary>
    [JsonProperty("instances_retrievable")]
    public bool InstancesRetrievable { get; set; }

    /// <summary>
    /// Specifies whether the Fetching a Service Binding endpoint is supported for all plans.
    /// </summary>
    [JsonProperty("bindings_retrievable")]
    public bool BindingsRetrievable { get; set; }

    /// <summary>
    /// An opaque object of metadata for a Service Offering. It is expected that Platforms will treat this as a blob. Note that there are conventions in existing Service Brokers and Platforms for fields that aid in the display of catalog data.
    /// </summary>
    [JsonProperty("metadata")]
    public JObject Metadata { get; set; }

    /// <summary>
    /// A Cloud Foundry extension described in Catalog Extensions. Contains the data necessary to activate the Dashboard SSO feature for this service.
    /// </summary>
    [JsonProperty("dashboard_client")]
    public DashboardClient DashboardClient { get; set; }

    /// <summary>
    /// Whether the service supports upgrade/downgrade for some plans.
    /// </summary>
    [JsonProperty("plan_updateable")]
    public bool PlanUpdateable { get; set; }

    /// <summary>
    /// A list of plans for this service. MUST contain at least one plan.
    /// </summary>
    [JsonProperty("plans", Required = Required.Always)]
    public List<Plan> Plans { get; } = new();

    public bool Equals(Service other)
        => other != null
        && Id == other.Id
        && Name == other.Name
        && Description == other.Description
        && Bindable == other.Bindable
        && Equals(DashboardClient, other.DashboardClient)
        && PlanUpdateable == other.PlanUpdateable;

    public override bool Equals(object obj) => obj is Service other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Id?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ Bindable.GetHashCode();
            hashCode = (hashCode * 397) ^ (DashboardClient?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ PlanUpdateable.GetHashCode();
            return hashCode;
        }
    }
}
