using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker
{
    /// <summary>
    /// Describes the identity of the user that initiated a request from the Platform.
    /// </summary>
    public class OriginatingIdentity : IEquatable<OriginatingIdentity>
    {
        public const string HttpHeaderName = "X-Broker-API-Originating-Identity";

        /// <summary>
        /// The Platform from which the request is being sent.
        /// </summary>
        public string Platform { get; }

        /// <summary>
        /// A JSON object describing the user.
        /// </summary>
        public JObject Value { get; }

        public OriginatingIdentity(string platform, JObject value)
        {
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
            if (platform.Any(char.IsWhiteSpace)) throw new ArgumentException("Platform name may not contain any whitespace characters.", nameof(platform));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static OriginatingIdentity Parse(string value)
        {
            var parts = value.Split(' ');
            return new OriginatingIdentity(parts[0], JObject.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]))));
        }

        public override string ToString()
            => Platform + " " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Value.ToString(Formatting.None)));

        public bool Equals(OriginatingIdentity other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Platform == other.Platform && Value.ToString(Formatting.None) == other.Value.ToString(Formatting.None);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((OriginatingIdentity)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Platform != null ? Platform.GetHashCode() : 0) * 397) ^ (Value != null ? Value.ToString(Formatting.None).GetHashCode() : 0);
            }
        }

        public static bool operator ==(OriginatingIdentity left, OriginatingIdentity right) => Equals(left, right);

        public static bool operator !=(OriginatingIdentity left, OriginatingIdentity right) => !Equals(left, right);
    }
}
