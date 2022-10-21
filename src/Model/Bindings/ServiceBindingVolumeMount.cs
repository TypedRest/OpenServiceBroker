using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenServiceBroker.Bindings;

public class ServiceBindingVolumeMount
{
    /// <summary>
    /// Name of the volume driver plugin which manages the device.
    /// </summary>
    [JsonProperty("driver", Required = Required.Always)]
    public string Driver { get; set; }

    /// <summary>
    /// The path in the application container onto which the volume will be mounted. This specification does not mandate what action the Platform is to take if the path specified already exists in the container.
    /// </summary>
    [JsonProperty("container_dir", Required = Required.Always)]
    public string ContainerDir { get; set; }

    /// <summary>
    /// The access mode to mount the device in.
    /// </summary>
    [JsonProperty("mode", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter))]
    public ServiceBindingVolumeMountMode Mode { get; set; }

    /// <summary>
    /// The type of device to mount.
    /// </summary>
    [JsonProperty("device_type", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter))]
    public ServiceBindingVolumeMountDeviceType DeviceType { get; set; }

    /// <summary>
    /// Device object containing specific details.
    /// </summary>
    [JsonProperty("device", Required = Required.Always)]
    public ServiceBindingVolumeMountDevice Device { get; set; } = new();
}
