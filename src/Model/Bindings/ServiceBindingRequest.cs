using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings;

public class ServiceBindingRequest : IServicePlanReference, IEquatable<ServiceBindingRequest>
{
    /// <summary>
    /// MUST be the ID of the service that is being used.
    /// </summary>
    [JsonProperty("service_id", Required = Required.Always)]
    public string ServiceId { get; set; }

    /// <summary>
    /// MUST be the ID of the plan from the service that is being used.
    /// </summary>
    [JsonProperty("plan_id", Required = Required.Always)]
    public string PlanId { get; set; }

    /// <summary>
    /// Configuration parameters for the Service Binding. Service Brokers SHOULD ensure that the client has provided valid configuration parameters and values for the operation.
    /// </summary>
    [JsonProperty("parameters")]
    public JObject Parameters { get; set; }

    /// <summary>
    /// Contextual data under which the Service Binding is created.
    /// </summary>
    [JsonProperty("context")]
    public JObject Context { get; set; }

    /// <summary>
    /// GUID of an application associated with the binding to be created. If present, MUST be a non-empty string.
    /// </summary>
    [JsonProperty("app_guid")]
    [Obsolete("Deprecated in favor of " + nameof(BindResource) + "." + nameof(ServiceBindingResourceObject.AppGuid))]
    public string AppGuid { get; set; }

    /// <summary>
    /// An object that contains data for Platform resources associated with the binding to be created.
    /// </summary>
    [JsonProperty("bind_resource")]
    public ServiceBindingResourceObject BindResource { get; set; }

    /// <summary>
    /// Requests the rotation of an existing Service Binding. If present, MUST be the ID of a non-expired Service Binding of the same Service Instance.
    /// The request creates a new Service Binding; both the new and the old Service Binding MUST be valid in parallel until they expire or are deleted.
    /// Only supported for plans with <see cref="Catalogs.Plan.BindingRotatable"/> set to true.
    /// </summary>
    [JsonProperty("predecessor_binding_id")]
    public string PredecessorBindingId { get; set; }

    public bool Equals(ServiceBindingRequest other)
        => other != null
        && ServiceId == other.ServiceId
        && PlanId == other.PlanId
        && Equals(BindResource, other.BindResource)
        && PredecessorBindingId == other.PredecessorBindingId;

    public override bool Equals(object obj) => obj is ServiceBindingRequest other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = ServiceId?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (PlanId?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (BindResource?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (PredecessorBindingId?.GetHashCode() ?? 0);
            return hashCode;
        }
    }
}
