using System;
using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using TypedRest;

namespace OpenServiceBroker.Instances
{
    public abstract class ServiceInstanceEndpointBase<TServiceBindingEndpointInterface, TServiceBindingEndpoint> : ServiceBrokerEndpointBase, IServiceInstanceEndpointBase<TServiceBindingEndpointInterface>
        where TServiceBindingEndpoint : class, TServiceBindingEndpointInterface
        where TServiceBindingEndpointInterface : IServiceBindingEndpointBase
    {
        /// <summary>
        /// Creates a new Service Instance endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
        protected ServiceInstanceEndpointBase(IEndpoint referrer, Uri relativeUri, bool acceptsIncomplete = false)
            : base(referrer, relativeUri, acceptsIncomplete)
        {}

        public async Task<ServiceInstanceResource> FetchAsync()
        {
            var response = await HandleResponseAsync(HttpClient.GetAsync(Uri));
            return await FromContentAsync<ServiceInstanceResource>(response);
        }

        public IIndexerEndpoint<TServiceBindingEndpointInterface> ServiceBindings
            => new IndexerEndpoint<TServiceBindingEndpoint>(this, relativeUri: "./service_bindings");
    }
}
