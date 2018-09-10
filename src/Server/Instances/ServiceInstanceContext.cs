using System;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// Identifies a specific service instance to apply an operation to.
    /// </summary>
    public class ServiceInstanceContext : IEquatable<ServiceInstanceContext>
    {
        public string InstanceId { get; }

        public ServiceInstanceContext(string instanceId)
        {
            InstanceId = instanceId ?? throw new ArgumentNullException(nameof(instanceId));
        }

        public bool Equals(ServiceInstanceContext other)
        {
            if (other == null) return false;
            return ReferenceEquals(this, other) || string.Equals(InstanceId, other.InstanceId);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceInstanceContext) obj);
        }

        public override int GetHashCode() => InstanceId.GetHashCode();

        public static bool operator ==(ServiceInstanceContext left, ServiceInstanceContext right) => Equals(left, right);

        public static bool operator !=(ServiceInstanceContext left, ServiceInstanceContext right) => !Equals(left, right);
    }
}
