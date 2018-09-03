using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    public interface IServiceBindingEndpoint : IServiceBindingEndpointBase
    {
        /// <summary>
        /// generates a service binding
        /// </summary>
        /// <param name="request">parameters for the requested service binding</param>
        /// <exception cref="ConflictException">binding with the same id already exists but with different attributes</exception>
        Task<ServiceBinding> BindAsync(ServiceBindingRequest request);

        /// <summary>
        /// deletes a service binding
        /// </summary>
        /// <param name="serviceId">id of the service associated with the binding being deleted</param>
        /// <param name="planId">id of the plan associated with the binding being deleted</param>
        /// <exception cref="GoneException">binding does not exist</exception>
        Task UnbindAsync(string serviceId, string planId);
    }
}
