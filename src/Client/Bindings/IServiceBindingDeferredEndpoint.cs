using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    public interface IServiceBindingDeferredEndpoint : IServiceBindingEndpointBase
    {
        /// <summary>
        /// generates a service binding
        /// </summary>
        /// <param name="request">parameters for the requested service binding</param>
        /// <returns>the generated binding or an async operation (<see cref="LastOperation"/>)</returns>
        /// <exception cref="ConflictException">binding with the same id already exists but with different attributes</exception>
        Task<ServiceBindingAsyncOperation> BindAsync(ServiceBindingRequest request);

        /// <summary>
        /// deletes a service binding
        /// </summary>
        /// <param name="serviceId">id of the service associated with the binding being deleted</param>
        /// <param name="planId">id of the plan associated with the binding being deleted</param>
        /// <returns>completion indicator or an async operation (<see cref="LastOperation"/>)</returns>
        /// <exception cref="GoneException">binding does not exist</exception>
        Task<AsyncOperation> UnbindAsync(string serviceId, string planId);

        /// <summary>
        /// get last requested operation state for service binding
        /// </summary>
        /// <param name="serviceId">id of the service associated with the binding</param>
        /// <param name="planId">id of the plan associated with the binding</param>
        /// <param name="operation">a provided identifier for the operation</param>
        IPollingEndpoint<LastOperationResource> LastOperation(string serviceId = null, string planId = null, string operation = null);
    }
}
