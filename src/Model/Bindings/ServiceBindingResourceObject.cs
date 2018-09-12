using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingResourceObject : IEquatable<ServiceBindingResourceObject>
    {
        /// <summary>
        /// GUID of an application associated with the binding. For credentials bindings. MUST be unique within the scope of the Platform.
        /// </summary>
        [JsonProperty("app_guid")]
        public string AppGuid { get; set; }

        /// <summary>
        /// URL of the application to be intermediated. For route services bindings.
        /// </summary>
        [JsonProperty("route")]
        public string Route { get; set; }

        public bool Equals(ServiceBindingResourceObject other)
        {
            if (other == null) return false;
            return AppGuid == other.AppGuid && Route == other.Route;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceBindingResourceObject)obj);
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
