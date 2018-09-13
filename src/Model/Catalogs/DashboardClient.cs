using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// Contains the data necessary to activate the Dashboard SSO feature for this service.
    /// </summary>
    public class DashboardClient : IEquatable<DashboardClient>
    {
        /// <summary>
        /// The id of the OAuth client that the dashboard will use. If present, MUST be a non-empty string.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A secret for the dashboard client. If present, MUST be a non-empty string.
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// A URI for the service dashboard. Validated by the OAuth token server when the dashboard requests a token.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public Uri RedirectUri { get; set; }

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
                int hashCode = Id?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Secret?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (RedirectUri != null ? RedirectUri.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
