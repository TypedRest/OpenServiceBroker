using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// manages bindings for Service Instances with potentially deferred (asynchronous) operations
    /// </summary>
    public interface IServiceBindingDeferred : IServiceBindingBase
    {
        /// <summary>
        /// Generates a Service Binding.
        /// </summary>
        /// <param name="context">The id of binding to create.</param>
        /// <param name="request">Parameters for the requested Service Binding.</param>
        /// <returns>A potentially deferred (asynchronous) operation.</returns>
        /// <exception cref="ConflictException">A binding with the same id already exists but with different attributes.</exception>
        Task<ServiceBindingAsyncOperation> BindAsync(ServiceBindingContext context, ServiceBindingRequest request);

        /// <summary>
        /// Deletes a Service Binding.
        /// </summary>
        /// <param name="context">The id of the binding being deleted.</param>
        /// <param name="serviceId">The id of the service associated with the binding being deleted.</param>
        /// <param name="planId">The id of the plan associated with the binding being deleted.</param>
        /// <returns>A potentially deferred (asynchronous) operation.</returns>
        /// <exception cref="GoneException">The binding does not exist (anymore).</exception>
        Task<AsyncOperation> UnbindAsync(ServiceBindingContext context, string serviceId, string planId);

        /// <summary>
        /// Gets the state of the last requested deferred (asynchronous) operation for a Service Binding.
        /// </summary>
        /// <param name="context">The id of Service Binding to find last operation applied to it</param>
        /// <param name="serviceId">The id of the service associated with the binding.</param>
        /// <param name="planId">The id of the plan associated with the binding.</param>
        /// <param name="operation">The value provided in <see cref="AsyncOperation.Operation"/>.</param>
        /// <exception cref="GoneException">The binding requested to be deleted does not exist (anymore).</exception>
        Task<LastOperationResource> GetLastOperationAsync(ServiceBindingContext context, string serviceId = null, string planId = null, string operation = null);
    }
}
