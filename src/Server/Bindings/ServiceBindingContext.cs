using System;
using OpenServiceBroker.Instances;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// Identifies a specific service binding to apply an operation to.
    /// </summary>
    public class ServiceBindingContext : ServiceInstanceContext, IEquatable<ServiceBindingContext>
    {
        public string BindingId { get; }

        public ServiceBindingContext(string instanceId, string bindingId)
            : base(instanceId)
        {
            BindingId = bindingId ?? throw new ArgumentNullException(nameof(bindingId));
        }

        public ServiceBindingContext(string instanceId, string bindingId, OriginatingIdentity originatingIdentity)
            : base(instanceId, originatingIdentity)
        {
            BindingId = bindingId ?? throw new ArgumentNullException(nameof(bindingId));
        }

        public bool Equals(ServiceBindingContext other)
        {
            if (other == null) return false;
            return base.Equals(other) && string.Equals(BindingId, other.BindingId);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceBindingContext)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (BindingId != null ? BindingId.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ServiceBindingContext left, ServiceBindingContext right) => Equals(left, right);

        public static bool operator !=(ServiceBindingContext left, ServiceBindingContext right) => !Equals(left, right);
    }
}
