using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingVolumeMountDevice
    {
        [JsonProperty("volume_id", Required = Required.Always)]
        public string VolumeId { get; set; }

        [JsonProperty("mount_config")]
        public JObject MountConfig { get; set; }
    }
}
