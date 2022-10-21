using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Common base for <see cref="IServiceInstanceBlocking"/> and <see cref="IServiceInstanceDeferred"/>; do not use directly!
/// </summary>
public interface IServiceInstanceBase
{
    /// <summary>
    /// Fetches a Service Instance.
    /// </summary>
    /// <param name="instanceId">The id of instance to fetch.</param>
    /// <exception cref="NotFoundException">The instance does not exist or a provisioning operation is still in progress.</exception>
    /// <exception cref="ConcurrencyException">The instance is being updated and therefore cannot be fetched at this time.</exception>
    Task<ServiceInstanceResource> FetchAsync(string instanceId);
}
