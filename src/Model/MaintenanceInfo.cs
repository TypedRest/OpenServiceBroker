using System;
using Newtonsoft.Json;

namespace OpenServiceBroker;

/// <summary>
/// This field can be used to ensure that the end-user of a Platform is provisioning what they are expecting since maintenance information can be used to describe important information (such as the version of the operating system the Service Instance will run on).
/// </summary>
public class MaintenanceInfo : IEquatable<MaintenanceInfo>
{
    /// <summary>
    /// This MUST be a string conforming to a semantic version 2.0. The Platform MAY use this field to determine whether a maintenance update is available for a Service Instance.
    /// </summary>
    [JsonProperty("version", Required = Required.Always)]
    public string Version { get; set; }

    /// <summary>
    /// This SHOULD be a string describing the impact of the maintenance update, for example, important version changes, configuration changes, default value changes, etc. The Platform MAY present this information to the user before they trigger the maintenance update.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    public bool Equals(MaintenanceInfo other)
        => other != null
        && Version == other.Version
        && Description == other.Description;

    public override bool Equals(object obj) => obj is MaintenanceInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Version != null ? Version.GetHashCode() : 0) * 397) ^ (Description != null ? Description.GetHashCode() : 0);
        }
    }
}
