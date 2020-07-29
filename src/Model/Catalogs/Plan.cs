using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// Describes a plan for a service available on the Service Broker.
    /// </summary>
    public class Plan : IEquatable<Plan>
    {
        /// <summary>
        /// An identifier used to correlate this plan in future requests to the Service Broker. This MUST be globally unique such that Platforms (and their users) MUST be able to assume that seeing the same value (no matter what Service Broker uses it) will always refer to this plan and for the same service. MUST be a non-empty string. Using a GUID is RECOMMENDED.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// The CLI-friendly name of the plan. MUST only contain alphanumeric characters, periods, and hyphens (no spaces). MUST be unique within the service. MUST be a non-empty string.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// A short description of the plan. MUST be a non-empty string.
        /// </summary>
        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        /// <summary>
        /// An opaque object of metadata for a Service Plan. It is expected that Platforms will treat this as a blob. Note that there are conventions in existing Service Brokers and Platforms for fields that aid in the display of catalog data.
        /// </summary>
        [JsonProperty("metadata")]
        public JObject Metadata { get; set; }

        /// <summary>
        /// When false, Service Instances of this plan have a cost. The default is true.
        /// </summary>
        [JsonProperty("free")]
        [DefaultValue(true)]
        public bool Free { get; set; } = true;

        /// <summary>
        /// Specifies whether Service Instances of the Service Plan can be bound to applications. This field is OPTIONAL. If specified, this takes precedence over the bindable attribute of the service. If not specified, the default is derived from the service.
        /// </summary>
        [JsonProperty("bindable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Bindable { get; set; }

        /// <summary>
        /// Whether the Plan supports upgrade/downgrade/sidegrade to another version. This field is OPTIONAL. If specified, this takes precedence over <see cref="Service.PlanUpdateable"/>.
        /// </summary>
        [JsonProperty("plan_updateable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PlanUpdateable { get; set; }

        /// <summary>
        /// Schema definitions for Service Instances and bindings for the plan.
        /// </summary>
        [JsonProperty("schemas")]
        public Schemas Schemas { get; set; }

        /// <summary>
        /// A duration, in seconds, that the Platform SHOULD use as the Service's maximum polling duration.
        /// </summary>
        [JsonProperty("maximum_polling_duration", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaximumPollingDuration { get; set; }

        /// <summary>
        /// Maintenance information for a Service Instance which is provisioned using the Service Plan.
        /// </summary>
        [JsonProperty("maintenance_info", NullValueHandling = NullValueHandling.Ignore)]
        public MaintenanceInfo MaintenanceInfo { get; set; }

        public bool Equals(Plan other)
            => other != null
            && Id == other.Id
            && Name == other.Name
            && Description == other.Description
            && Free == other.Free
            && Bindable == other.Bindable
            && PlanUpdateable == other.PlanUpdateable
            && Equals(Schemas, other.Schemas)
            && Equals(MaintenanceInfo, other.MaintenanceInfo);

        public override bool Equals(object obj) => obj is Plan other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Free.GetHashCode();
                hashCode = (hashCode * 397) ^ Bindable.GetHashCode();
                hashCode = (hashCode * 397) ^ PlanUpdateable.GetHashCode();
                return hashCode;
            }
        }
    }
}
