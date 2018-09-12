using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// Common base for <see cref="IServiceInstanceEndpoint"/> and <see cref="IServiceInstanceDeferredEndpoint"/>; do not use directly!
    /// </summary>
    /// <typeparam name="TServiceBindingEndpoint">The endpoint type used to represent Service Bindings for this Service Instance.</typeparam>
    public interface IServiceInstanceEndpointBase<out TServiceBindingEndpoint> : IEndpoint
        where TServiceBindingEndpoint : IServiceBindingEndpointBase
    {
        /// <summary>
        /// Fetches a Service Instance.
        /// </summary>
        /// <exception cref="NotFoundException">The instance does not exist or a provisioning operation is still in progress.</exception>
        /// <exception cref="ConcurrencyException">The instance is being updated and therefore cannot be fetched at this time.</exception>
        Task<ServiceInstanceResource> FetchAsync();

        /// <summary>
        /// Exposes bindings for this Service Instance.
        /// </summary>
        IIndexerEndpoint<TServiceBindingEndpoint> ServiceBindings { get; }
    }
}
