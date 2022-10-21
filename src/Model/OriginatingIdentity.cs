using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker;

/// <summary>
/// Describes the identity of the user that initiated a request from the Platform.
/// </summary>
public class OriginatingIdentity : IEquatable<OriginatingIdentity>
{
    /// <summary>
    /// The name of the HTTP header used to transmit this information.
    /// </summary>
    public const string HttpHeaderName = "X-Broker-API-Originating-Identity";

    /// <summary>
    /// The Platform from which the request is being sent.
    /// </summary>
    public string Platform { get; }

    /// <summary>
    /// A JSON object describing the user.
    /// </summary>
    public JObject Value { get; }

    /// <summary>
    /// Describes the identity of the user that initiated a request from the Platform.
    /// </summary>
    /// <param name="platform">The Platform from which the request is being sent.</param>
    /// <param name="value">A JSON object describing the user.</param>
    public OriginatingIdentity(string platform, JObject value)
    {
        if (string.IsNullOrEmpty(platform)) throw new ArgumentException("Platform must be a non-empty string.", nameof(platform));
        if (platform.Any(char.IsWhiteSpace)) throw new ArgumentException("Platform name may not contain any whitespace characters.", nameof(platform));
        Platform = platform;

        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Parses the originating identity object from its header string representation.
    /// </summary>
    public static OriginatingIdentity Parse(string value)
    {
        var parts = value.Split(' ');
        return new OriginatingIdentity(parts[0], JObject.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]))));
    }

    /// <summary>
    /// Serialized the originating identity object to its header string representation.
    /// </summary>
    public override string ToString()
        => Platform + " " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Value.ToString(Formatting.None)));

    public bool Equals(OriginatingIdentity other)
        => other != null
        && Platform == other.Platform
        && Value.ToString(Formatting.None) == other.Value.ToString(Formatting.None);

    public override bool Equals(object obj) => obj is OriginatingIdentity other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Platform?.GetHashCode() ?? 0) * 397) ^ (Value?.ToString(Formatting.None).GetHashCode() ?? 0);
        }
    }

    public static bool operator ==(OriginatingIdentity left, OriginatingIdentity right) => Equals(left, right);

    public static bool operator !=(OriginatingIdentity left, OriginatingIdentity right) => !Equals(left, right);
}
