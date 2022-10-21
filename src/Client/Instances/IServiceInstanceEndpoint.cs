using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Represents a specific Service Instance with blocking operations.
/// </summary>
/// <remarks>What a Service Instance represents can vary by service. Examples include a single database on a multi-tenant server, a dedicated cluster, or an account on a web application.</remarks>
public interface IServiceInstanceEndpoint : IServiceInstanceEndpointBase<IServiceBindingEndpoint>
{
    /// <summary>
    /// Provisions the Service Instance.
    /// </summary>
    /// <param name="request">Parameters for the requested Service Instance provision.</param>
    /// <exception cref="ConflictException">An instance with the same id already exists but with different attributes.</exception>
    /// <returns>The provisioned instance.</returns>
    Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceProvisionRequest request);

    /// <summary>
    /// Updates the Service Instance.
    /// </summary>
    /// <param name="request">Parameters for the requested Service Instance update.</param>
    /// <exception cref="BrokerException">The requested change is not supported.</exception>
    Task UpdateAsync(ServiceInstanceUpdateRequest request);

    /// <summary>
    /// Deprovision the Service Instance.
    /// </summary>
    /// <param name="serviceId">The id of the service associated with the instance being deleted.</param>
    /// <param name="planId">The id of the plan associated with the instance being deleted.</param>
    /// <exception cref="GoneException">The instance does not exist (anymore).</exception>
    Task DeprovisionAsync(string serviceId, string planId);
}
