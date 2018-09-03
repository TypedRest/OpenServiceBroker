using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// manages bindings for service instances with blocking operations
    /// </summary>
    public interface IServiceBindingBlocking : IServiceBindingBase
    {
        /// <summary>
        /// generates a service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance to create a binding on</param>
        /// <param name="bindingId">binding id of binding to create</param>
        /// <param name="request">parameters for the requested service binding</param>
        /// <exception cref="ConflictException">binding with the same id already exists but with different attributes</exception>
        Task<ServiceBinding> BindAsync(string instanceId, string bindingId, ServiceBindingRequest request);

        /// <summary>
        /// deletes a service binding
        /// </summary>
        /// <param name="instanceId">id of the instance associated with the binding being deleted</param>
        /// <param name="bindingId">id of the binding being deleted</param>
        /// <param name="serviceId">id of the service associated with the binding being deleted</param>
        /// <param name="planId">id of the plan associated with the binding being deleted</param>
        /// <exception cref="GoneException">binding does not exist</exception>
        Task UnbindAsync(string instanceId, string bindingId, string serviceId, string planId);
    }
}
