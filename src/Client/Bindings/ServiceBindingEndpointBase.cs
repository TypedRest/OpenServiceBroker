using TypedRest.Endpoints;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// A Service Binding endpoint.
/// </summary>
/// <param name="referrer">The endpoint used to navigate to this one.</param>
/// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
/// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
public abstract class ServiceBindingEndpointBase(IEndpoint referrer, Uri relativeUri, bool acceptsIncomplete = false)
    : ServiceBrokerEndpointBase(referrer, relativeUri, acceptsIncomplete), IServiceBindingEndpointBase
{
    public async Task<ServiceBindingResource> FetchAsync()
    {
        var response = await HandleAsync(() => HttpClient.GetAsync(Uri));
        return await FromContentAsync<ServiceBindingResource>(response);
    }
}
