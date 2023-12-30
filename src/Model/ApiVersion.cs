namespace OpenServiceBroker;

public struct ApiVersion(int major, int minor) : IEquatable<ApiVersion>
{
    public const string HttpHeaderName = "X-Broker-API-Version";

    public static ApiVersion Parse(string input)
    {
        var split = input.Split('.');
        return new ApiVersion(int.Parse(split[0]), int.Parse(split[1]));
    }

    public int Major { get; } = major;

    public int Minor { get; } = minor;

    public override string ToString() => Major + "." + Minor;

    public bool Equals(ApiVersion other) => Major == other.Major && Minor == other.Minor;

    public override bool Equals(object obj) => obj is ApiVersion other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Major * 397) ^ Minor;
        }
    }

    public static bool operator ==(ApiVersion left, ApiVersion right) => left.Equals(right);

    public static bool operator !=(ApiVersion left, ApiVersion right) => !left.Equals(right);
}
