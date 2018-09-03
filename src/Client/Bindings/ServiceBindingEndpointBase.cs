using System;
using System.Threading.Tasks;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    public abstract class ServiceBindingEndpointBase : ServiceEndpointBase, IServiceBindingEndpointBase
    {
        /// <summary>
        /// Creates a new service binding endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        protected ServiceBindingEndpointBase(IEndpoint referrer, Uri relativeUri, bool acceptsIncomplete = false)
            : base(referrer, relativeUri, acceptsIncomplete)
        {}

        public async Task<ServiceBindingResource> FetchAsync()
        {
            var response = await HandleResponseAsync(HttpClient.GetAsync(Uri));
            return await FromContentAsync<ServiceBindingResource>(response);
        }
    }
}
