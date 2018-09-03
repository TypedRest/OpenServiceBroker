using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingVolumeMount
    {
        [JsonProperty("driver", Required = Required.Always)]
        public string Driver { get; set; }

        [JsonProperty("container_dir", Required = Required.Always)]
        public string ContainerDir { get; set; }

        [JsonProperty("mode", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceBindingVolumeMountMode Mode { get; set; }

        [JsonProperty("device_type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceBindingVolumeMountDeviceType DeviceType { get; set; }

        [JsonProperty("device", Required = Required.Always)]
        public ServiceBindingVolumeMountDevice Device { get; set; } = new ServiceBindingVolumeMountDevice();
    }
}
