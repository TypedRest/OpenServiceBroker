using System;
using OpenServiceBroker.Instances;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Identifies a specific Service Binding to apply an operation to.
/// </summary>
public class ServiceBindingContext : ServiceInstanceContext, IEquatable<ServiceBindingContext>
{
    /// <summary>
    /// The ID of the Service Binding.
    /// </summary>
    public string BindingId { get; }

    /// <summary>
    /// Creates a new Service Binding context.
    /// </summary>
    /// <param name="instanceId">The ID of the Service Instance.</param>
    /// <param name="bindingId">The ID of the Service Binding.</param>
    public ServiceBindingContext(string instanceId, string bindingId)
        : base(instanceId)
    {
        BindingId = bindingId ?? throw new ArgumentNullException(nameof(bindingId));
    }

    /// <summary>
    /// Creates a new Service Binding context.
    /// </summary>
    /// <param name="instanceId">The ID of the Service Instance.</param>
    /// <param name="bindingId">The ID of the Service Binding.</param>
    /// <param name="originatingIdentity">Describes the identity of the user that initiated a request from the Platform.</param>
    public ServiceBindingContext(string instanceId, string bindingId, OriginatingIdentity? originatingIdentity)
        : base(instanceId, originatingIdentity)
    {
        BindingId = bindingId ?? throw new ArgumentNullException(nameof(bindingId));
    }

    public bool Equals(ServiceBindingContext? other)
        => !ReferenceEquals(other, null) && base.Equals(other) && string.Equals(BindingId, other.BindingId);

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        return obj.GetType() == GetType() && Equals((ServiceBindingContext)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ BindingId.GetHashCode();
        }
    }

    public static bool operator ==(ServiceBindingContext? left, ServiceBindingContext? right) => Equals(left, right);

    public static bool operator !=(ServiceBindingContext? left, ServiceBindingContext? right) => !Equals(left, right);
}
