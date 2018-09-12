using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// Common base for <see cref="IServiceBindingEndpoint"/> and <see cref="IServiceBindingDeferredEndpoint"/>; do not use directly!
    /// </summary>
    public interface IServiceBindingEndpointBase : IEndpoint
    {
        /// <summary>
        /// Fetches the Service Binding.
        /// </summary>
        /// <exception cref="NotFoundException">The binding does not exist or a binding operation is still in progress.</exception>
        Task<ServiceBindingResource> FetchAsync();
    }
}
