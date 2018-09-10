using System;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// Identifies a specific service instance to apply an operation to.
    /// </summary>
    public class ServiceInstanceContext : IEquatable<ServiceInstanceContext>
    {
        public string InstanceId { get; }

        public OriginatingIdentity OriginatingIdentity { get; }

        public ServiceInstanceContext(string instanceId, OriginatingIdentity originatingIdentity)
        {
            InstanceId = instanceId ?? throw new ArgumentNullException(nameof(instanceId));
            OriginatingIdentity = originatingIdentity;
        }

        public ServiceInstanceContext(string instanceId)
            : this(instanceId, null)
        {}

        public bool Equals(ServiceInstanceContext other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return InstanceId == other.InstanceId && OriginatingIdentity == other.OriginatingIdentity;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceContext) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = InstanceId.GetHashCode();
                hashCode = (hashCode * 397) ^ OriginatingIdentity.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ServiceInstanceContext left, ServiceInstanceContext right) => Equals(left, right);

        public static bool operator !=(ServiceInstanceContext left, ServiceInstanceContext right) => !Equals(left, right);
    }
}
