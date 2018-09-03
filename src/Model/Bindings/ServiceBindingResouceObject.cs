using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingResouceObject : IEquatable<ServiceBindingResouceObject>
    {
        [JsonProperty("app_guid")]
        public string AppGuid { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        public bool Equals(ServiceBindingResouceObject other)
        {
            if (other == null) return false;
            return string.Equals(AppGuid, other.AppGuid) && string.Equals(Route, other.Route);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceBindingResouceObject)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((AppGuid != null ? AppGuid.GetHashCode() : 0) * 397) ^ (Route != null ? Route.GetHashCode() : 0);
            }
        }
    }
}
