using System;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Identifies a specific Service Instance to apply an operation to.
/// </summary>
public class ServiceInstanceContext : IEquatable<ServiceInstanceContext>
{
    public string InstanceId { get; }

    /// <summary>
    /// Describes the identity of the user that initiated a request from the Platform. Optional.
    /// </summary>
    public OriginatingIdentity? OriginatingIdentity { get; }

    /// <summary>
    /// Creates a new Service Instance context.
    /// </summary>
    /// <param name="instanceId">The ID of the Service Instance.</param>
    public ServiceInstanceContext(string? instanceId)
        : this(instanceId, null)
    {}

    /// <summary>
    /// Creates a new Service Instance context.
    /// </summary>
    /// <param name="instanceId">The ID of the Service Instance.</param>
    /// <param name="originatingIdentity">Describes the identity of the user that initiated a request from the Platform.</param>
    public ServiceInstanceContext(string? instanceId, OriginatingIdentity? originatingIdentity)
    {
        InstanceId = instanceId ?? throw new ArgumentNullException(nameof(instanceId));
        OriginatingIdentity = originatingIdentity;
    }

    public bool Equals(ServiceInstanceContext? other)
    {
        if (other == null) return false;
        return InstanceId == other.InstanceId && OriginatingIdentity == other.OriginatingIdentity;
    }

    public override bool Equals(object? obj)
        => !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((ServiceInstanceContext)obj);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InstanceId.GetHashCode();
            if (OriginatingIdentity != null) hashCode = (hashCode * 397) ^ OriginatingIdentity.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(ServiceInstanceContext? left, ServiceInstanceContext? right) => Equals(left, right);

    public static bool operator !=(ServiceInstanceContext? left, ServiceInstanceContext? right) => !Equals(left, right);
}
