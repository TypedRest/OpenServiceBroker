using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// Manages Service Instances with potentially deferred (asynchronous) operations.
    /// </summary>
    public interface IServiceInstanceDeferred : IServiceInstanceBase
    {
        /// <summary>
        /// Provisions a Service Instance.
        /// </summary>
        /// <param name="context">The id of instance to provision</param>
        /// <param name="request">Parameters for the requested Service Instance provision</param>
        /// <returns>A potentially deferred (asynchronous) operation.</returns>
        /// <exception cref="ConflictException">instance with the same id already exists but with different attributes</exception>
        Task<ServiceInstanceAsyncOperation> ProvisionAsync(ServiceInstanceContext context, ServiceInstanceProvisionRequest request);

        /// <summary>
        /// Updates a Service Instance.
        /// </summary>
        /// <param name="context">The id of instance to update.</param>
        /// <param name="request">Parameters for the requested Service Instance update</param>
        /// <returns>A potentially deferred (asynchronous) operation.</returns>
        /// <exception cref="BrokerException">The requested change is not supported.</exception>
        Task<ServiceInstanceAsyncOperation> UpdateAsync(ServiceInstanceContext context, ServiceInstanceUpdateRequest request);

        /// <summary>
        /// Deprovisions/deletes a Service Instance.
        /// </summary>
        /// <param name="context">The id of instance being deleted.</param>
        /// <param name="serviceId">The id of the service associated with the instance being deleted.</param>
        /// <param name="planId">The id of the plan associated with the instance being deleted.</param>
        /// <returns>A potentially deferred (asynchronous) operation.</returns>
        /// <exception cref="GoneException">The instance does not exist (anymore).</exception>
        Task<AsyncOperation> DeprovisionAsync(ServiceInstanceContext context, string? serviceId = null, string? planId = null);

        /// <summary>
        /// Gets the state of the last requested deferred (asynchronous) operation for a Service Instance.
        /// </summary>
        /// <param name="context">The id of instance to find last operation applied to it</param>
        /// <param name="serviceId">The id of the service associated with the instance.</param>
        /// <param name="planId">The id of the plan associated with the instance.</param>
        /// <param name="operation">The value provided in <see cref="AsyncOperation.Operation"/>.</param>
        /// <exception cref="GoneException">The instance requested to be deleted does not exist (anymore).</exception>
        Task<LastOperationResource> GetLastOperationAsync(ServiceInstanceContext context, string? serviceId = null, string? planId = null, string? operation = null);
    }
}
