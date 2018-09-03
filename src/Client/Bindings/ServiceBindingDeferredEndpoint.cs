using System;
using System.Net.Http;
using System.Threading.Tasks;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingDeferredEndpoint : ServiceBindingEndpointBase, IServiceBindingDeferredEndpoint
    {
        /// <summary>
        /// Creates a new service binding endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        public ServiceBindingDeferredEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri, acceptsIncomplete: true)
        {}

        public async Task<ServiceBindingAsyncOperation> BindAsync(ServiceBindingRequest request)
        {
            var response = await HandleResponseAsync(HttpClient.PutAsync(Uri, request, Serializer));
            return await ParseDeferredResponseAsync<ServiceBinding, ServiceBindingAsyncOperation>(response);
        }

        public async Task<AsyncOperation> UnbindAsync(string serviceId, string planId)
        {
            var response = await HandleResponseAsync(HttpClient.DeleteAsync(GetDeleteUri(serviceId, planId)));
            return await ParseDeferredResponseAsync(response);
        }

        public IPollingEndpoint<LastOperationResource> LastOperation(string serviceId = null, string planId = null, string operation = null)
            => new LastOperationEndpoint(this, serviceId, planId, operation);
    }
}
