using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker
{
    public abstract class ServiceEndpointBase : EndpointBase
    {
        /// <summary>
        /// Creates a new service endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s. Prefix <c>./</c> to append a trailing slash to the <paramref name="referrer"/> URI if missing.</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        protected ServiceEndpointBase(IEndpoint referrer, Uri relativeUri, bool acceptsIncomplete = false)
            : base(referrer, acceptsIncomplete
                ? relativeUri.Join("./?accepts_incomplete=true")
                : relativeUri)
        {
            SetDefaultLinkTemplate("delete", acceptsIncomplete
                ? "./?accepts_incomplete=true{&service_id,plan_id}"
                : "./{?service_id,plan_id}");
        }

        protected Uri GetDeleteUri(string serviceId, string planId)
            => Uri.Join(LinkTemplate("delete").Resolve(new
            {
                service_id = serviceId,
                plan_id = planId
            }));

        protected override async Task HandleErrorsAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode && response.Content != null)
            {
                Error error = null;
                try
                {
                    error = await FromContentAsync<Error>(response);
                }
                catch
                {
                    // Error responses without a response in standard format will be handled below
                }
                if (error != null) throw BrokerException.FromDto(error, response.StatusCode);
            }

            response.EnsureSuccessStatusCode();
        }

        protected async Task<AsyncOperation> ParseDeferredResponseAsync(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new AsyncOperation();

                case HttpStatusCode.Accepted:
                    return await FromContentAsync<AsyncOperation>(response);

                default:
                    throw new HttpRequestException($"DELETE {Uri} returned unexpected status code: {response.StatusCode}");
            }
        }

        protected async Task<TDeferred> ParseDeferredResponseAsync<TComplete, TDeferred>(HttpResponseMessage response)
            where TComplete : IUnchangedFlag
            where TDeferred : ICompletedResult<TComplete>, new()
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var result = await FromContentAsync<TComplete>(response);
                    if (result != null)
                        result.Unchanged = true;
                    return new TDeferred {Result = result};
                }

                case HttpStatusCode.Created:
                {
                    var result = await FromContentAsync<TComplete>(response);
                    return new TDeferred {Result = result};
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
