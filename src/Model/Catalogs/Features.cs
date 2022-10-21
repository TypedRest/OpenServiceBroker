using System.Runtime.Serialization;

namespace OpenServiceBroker.Catalogs;

/// <summary>
/// Permissions for services.
/// </summary>
public enum Features
{
    /// <summary>
    /// There are a class of Service Offerings that provide aggregation, indexing, and analysis of log data. To utilize these services an application that generates logs needs information for the location to which it will stream logs. A create binding response from a Service Broker that provides one of these services MUST include a <see cref="Bindings.ServiceBindingBase.SyslogDrainUrl"/>. The Platform MUST use this value when sending logs to the service.
    /// </summary>
    [EnumMember(Value = "syslog_drain")]
    SyslogDrain,

    /// <summary>
    /// Route services are a class of Service Offerings that intermediate requests to applications, performing functions such as rate limiting or authorization. To indicate support for route services, the catalog entry for the Service MUST use this value.
    /// </summary>
    [EnumMember(Value = "route_forwarding")]
    RouteForwarding,

    /// <summary>
    /// There are a class of services that provide network storage to applications via volume mounts in the application container. A create binding response from one of these services MUST include <see cref="Bindings.ServiceBindingBase.VolumeMounts"/>.
    /// </summary>
    [EnumMember(Value = "volume_mount")]
    VolumeMount
}
