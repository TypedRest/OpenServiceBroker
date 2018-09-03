using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// manages bindings for service instances with potentially deferred (asynchronous) operations
    /// </summary>
    public interface IServiceBindingDeferred : IServiceBindingBase
    {
        /// <summary>
        /// generates a service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance to create a binding on</param>
        /// <param name="bindingId">binding id of binding to create</param>
        /// <param name="request">parameters for the requested service binding</param>
        /// <returns>the generated binding or an async operation (<see cref="GetLastOperationAsync"/>)</returns>
        /// <exception cref="ConflictException">binding with the same id already exists but with different attributes</exception>
        Task<ServiceBindingAsyncOperation> BindAsync(string instanceId, string bindingId, ServiceBindingRequest request);

        /// <summary>
        /// deletes a service binding
        /// </summary>
        /// <param name="instanceId">id of the instance associated with the binding being deleted</param>
        /// <param name="bindingId">id of the binding being deleted</param>
        /// <param name="serviceId">id of the service associated with the binding being deleted</param>
        /// <param name="planId">id of the plan associated with the binding being deleted</param>
        /// <returns>completion indicator or an async operation (<see cref="GetLastOperationAsync"/>)</returns>
        /// <exception cref="GoneException">binding does not exist</exception>
        Task<AsyncOperation> UnbindAsync(string instanceId, string bindingId, string serviceId, string planId);

        /// <summary>
        /// get last requested operation state for service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance to find last operation applied to it</param>
        /// <param name="bindingId">binding id of service binding to find last operation applied to it</param>
        /// <param name="serviceId">id of the service associated with the binding</param>
        /// <param name="planId">id of the plan associated with the binding</param>
        /// <param name="operation">a provided identifier for the operation</param>
        /// <exception cref="GoneException">result of asynchronous delete operation: binding does not exist</exception>
        Task<LastOperationResource> GetLastOperationAsync(string instanceId, string bindingId, string serviceId = null, string planId = null, string operation = null);
    }
}
