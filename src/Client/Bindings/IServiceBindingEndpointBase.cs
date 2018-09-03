using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    public interface IServiceBindingEndpointBase : IEndpoint
    {
        /// <summary>
        /// fetches a service binding
        /// </summary>
        /// <exception cref="NotFoundException">binding does not exist or a binding operation is still in progress</exception>
        Task<ServiceBindingResource> FetchAsync();
    }
}
