using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Bindings
{
    public abstract class ServiceBindingBase
    {
        [JsonProperty("credentials")]
        public JObject Credentials { get; set; }

        [JsonProperty("syslog_drain_url")]
        public Uri SyslogDrainUrl { get; set; }

        [JsonProperty("route_service_url")]
        public Uri RouteServiceUrl { get; set; }

        [JsonProperty("volume_mounts")]
        public List<ServiceBindingVolumeMount> VolumeMounts { get; } = new List<ServiceBindingVolumeMount>();

        protected bool Equals(ServiceBindingBase other)
            => SyslogDrainUrl == other.SyslogDrainUrl && RouteServiceUrl == other.RouteServiceUrl;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is ServiceBindingBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SyslogDrainUrl != null ? SyslogDrainUrl.GetHashCode() : 0) * 397) ^ (RouteServiceUrl != null ? RouteServiceUrl.GetHashCode() : 0);
            }
        }
    }
}
