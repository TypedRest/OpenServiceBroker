using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

public abstract class ServiceInstanceBase : IServicePlanReference
{
    /// <summary>
    /// MUST be the ID of a <see cref="Catalogs.Service"/> from the <see cref="Catalogs.Catalog"/> for this Service Broker.
    /// </summary>
    [JsonProperty("service_id", Required = Required.Always)]
    public string ServiceId { get; set; }

    public abstract string PlanId { get; set; }

    /// <summary>
    /// Configuration parameters for the Service Instance. Service Brokers SHOULD ensure that the client has provided valid configuration parameters and values for the operation.
    /// </summary>
    [JsonProperty("parameters")]
    public JObject Parameters { get; set; }

    /// <summary>
    /// If a Service Broker provides maintenance information for a Service Plan in its Catalog, a Platform MAY provide the same maintenance information when provisioning a Service Instance.
    /// This field can be used to ensure that the end-user of a Platform is provisioning what they are expecting since maintenance information can be used to describe important information (such as the version of the operating system the Service Instance will run on).
    /// If a Service Broker's catalog has changed and new maintenance information version is available for the Service Plan being provisioned, then the Service Broker MUST reject the request with <see cref="MaintenanceInfoConflictException"/>.
    /// </summary>
    [JsonProperty("maintenance_info", NullValueHandling = NullValueHandling.Ignore)]
    public MaintenanceInfo MaintenanceInfo { get; set; }

    protected bool Equals(ServiceInstanceBase other)
        => ServiceId == other.ServiceId
        && PlanId == other.PlanId
        && Equals(MaintenanceInfo, other.MaintenanceInfo);

    public override bool Equals(object obj) => obj is ServiceInstanceBase other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((ServiceId?.GetHashCode() ?? 0) * 397) ^ (PlanId?.GetHashCode() ?? 0);
        }
    }
}
