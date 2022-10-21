using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Manages Service Instances with blocking operations.
/// </summary>
public interface IServiceInstanceBlocking : IServiceInstanceBase
{
    /// <summary>
    /// Provisions a Service Instance.
    /// </summary>
    /// <param name="context">The id of instance to provision.</param>
    /// <param name="request">Parameters for the requested Service Instance provision</param>
    /// <exception cref="ConflictException">An instance with the same id already exists but with different attributes.</exception>
    /// <returns>the provisioned instance</returns>
    Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceContext context, ServiceInstanceProvisionRequest request);

    /// <summary>
    /// Updates a Service Instance.
    /// </summary>
    /// <param name="context">The id of instance to update.</param>
    /// <param name="request">Parameters for the requested Service Instance update</param>
    /// <exception cref="BrokerException">The requested change is not supported.</exception>
    Task UpdateAsync(ServiceInstanceContext context, ServiceInstanceUpdateRequest request);

    /// <summary>
    /// Deprovisions/deletes a Service Instance.
    /// </summary>
    /// <param name="context">The id of instance being deleted.</param>
    /// <param name="serviceId">The id of the service associated with the instance being deleted.</param>
    /// <param name="planId">The id of the plan associated with the instance being deleted.</param>
    /// <exception cref="GoneException">The instance does not exist (anymore).</exception>
    Task DeprovisionAsync(ServiceInstanceContext context, string serviceId, string planId);
}
