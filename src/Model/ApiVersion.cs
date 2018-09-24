using System;

namespace OpenServiceBroker
{
    public struct ApiVersion : IEquatable<ApiVersion>
    {
        public const string HttpHeaderName = "X-Broker-API-Version";

        public static ApiVersion Parse(string input)
        {
            var split = input.Split('.');
            return new ApiVersion(int.Parse(split[0]), int.Parse(split[1]));
        }

        public int Major { get; }

        public int Minor { get; }

        public ApiVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

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
}
