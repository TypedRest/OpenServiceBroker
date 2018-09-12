using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingVolumeMountDevice
    {
        /// <summary>
        /// ID of the shared volume to mount on every app instance.
        /// </summary>
        [JsonProperty("volume_id", Required = Required.Always)]
        public string VolumeId { get; set; }

        /// <summary>
        /// Configuration object to be passed to the driver when the volume is mounted.
        /// </summary>
        [JsonProperty("mount_config")]
        public JObject MountConfig { get; set; }
    }
}
