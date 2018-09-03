using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingBlockingEndpoint : ServiceBindingEndpointBase, IServiceBindingEndpoint
    {
        /// <summary>
        /// Creates a new service binding endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        public ServiceBindingBlockingEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri)
        {}

        public async Task<ServiceBinding> BindAsync(ServiceBindingRequest request)
        {
            var response = await HandleResponseAsync(HttpClient.PutAsync(Uri, request, Serializer));
            var result = await FromContentAsync<ServiceBinding>(response);
            result.Unchanged = (response.StatusCode == HttpStatusCode.OK);
            return result;
        }

        public Task UnbindAsync(string serviceId, string planId)
            => HandleResponseAsync(HttpClient.DeleteAsync(GetDeleteUri(serviceId, planId)));
    }
}
