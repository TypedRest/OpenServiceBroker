using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Instances
{
    public interface IServiceInstanceEndpointBase<out TServiceBindingEndpoint> : IEndpoint
        where TServiceBindingEndpoint : IServiceBindingEndpointBase
    {
        /// <summary>
        /// fetches a service instance
        /// </summary>
        /// <exception cref="NotFoundException">instance does not exist or a provisioning operation is still in progress</exception>
        /// <exception cref="ConcurrencyException">instance is being updated and therefore cannot be fetched at this time</exception>
        Task<ServiceInstanceResource> FetchAsync();

        /// <summary>
        /// exposes bindings for this service instance
        /// </summary>
        IIndexerEndpoint<TServiceBindingEndpoint> ServiceBindings { get; }
    }
}
