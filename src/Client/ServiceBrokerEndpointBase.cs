using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TypedRest;
using TypedRest.Endpoints;

namespace OpenServiceBroker
{
    /// <summary>
    /// Common base class for Open Service Broker endpoints.
    /// </summary>
    public abstract class ServiceBrokerEndpointBase : EndpointBase
    {
        /// <summary>
        /// Creates a new service endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s. Prefix <c>./</c> to append a trailing slash to the <paramref name="referrer"/> URI if missing.</param>
        /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
        protected ServiceBrokerEndpointBase(IEndpoint referrer, Uri relativeUri, bool acceptsIncomplete = false)
            : base(referrer, acceptsIncomplete
                ? relativeUri.Join("./?accepts_incomplete=true")
                : relativeUri)
        {
            SetDefaultLinkTemplate("delete", acceptsIncomplete
                ? "./?accepts_incomplete=true{&service_id,plan_id}"
                : "./{?service_id,plan_id}");
        }

        protected Uri GetDeleteUri(string? serviceId, string? planId)
            => LinkTemplate("delete", new
            {
                service_id = serviceId,
                plan_id = planId
            });

        protected async Task<AsyncOperation> ParseDeferredResponseAsync(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new AsyncOperation().Complete();

                case HttpStatusCode.Accepted:
                    return await FromContentAsync<AsyncOperation>(response);

                default:
                    throw new HttpRequestException($"DELETE {Uri} returned unexpected status code: {response.StatusCode}");
            }
        }

        protected async Task<TDeferred> ParseDeferredResponseAsync<TComplete, TDeferred>(HttpResponseMessage response)
            where TComplete : class, IUnchangedFlag
            where TDeferred : AsyncOperation, ICompletableWithResult<TComplete>, new()
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var result = await FromContentAsync<TComplete>(response);
                    if (result != null)
                        result.Unchanged = true;
                    return new TDeferred().Complete(result);
                }

                case HttpStatusCode.Created:
                {
                    var result = await FromContentAsync<TComplete>(response);
                    return new TDeferred().Complete(result);
                }

                case HttpStatusCode.Accepted:
                    return await FromContentAsync<TDeferred>(response);

                default:
                    throw new HttpRequestException($"{Uri} returned unexpected status code: {response.StatusCode}");
            }
        }

        protected Task<T> FromContentAsync<T>(HttpResponseMessage message)
            => message.Content.ReadAsAsync<T>(new[] {Serializer});
    }
}
