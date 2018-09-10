using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    public class DashboardClient : IEquatable<DashboardClient>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        public bool Equals(DashboardClient other)
        {
            if (other == null) return false;
            return Id == other.Id && Secret == other.Secret && RedirectUri == other.RedirectUri;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((DashboardClient)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id != null ? Id.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Secret != null ? Secret.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RedirectUri != null ? RedirectUri.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
