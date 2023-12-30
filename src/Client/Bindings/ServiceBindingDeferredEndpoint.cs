using TypedRest.Endpoints;
using TypedRest.Endpoints.Reactive;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Represents a Service Binding for a specific Service Instance with potentially deferred (asynchronous) operations.
/// </summary>
/// <param name="referrer">The endpoint used to navigate to this one.</param>
/// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
public class ServiceBindingDeferredEndpoint(IEndpoint referrer, Uri relativeUri)
    : ServiceBindingEndpointBase(referrer, relativeUri, acceptsIncomplete: true), IServiceBindingDeferredEndpoint
{
    public async Task<ServiceBindingAsyncOperation> BindAsync(ServiceBindingRequest request)
    {
        var response = await HandleAsync(() => HttpClient.PutAsync(Uri, request, Serializer));
        return await ParseDeferredResponseAsync<ServiceBinding, ServiceBindingAsyncOperation>(response);
    }

    public async Task<AsyncOperation> UnbindAsync(string serviceId, string planId)
    {
        var response = await HandleAsync(() => HttpClient.DeleteAsync(GetDeleteUri(serviceId, planId)));
        return await ParseDeferredResponseAsync(response);
    }

    public IPollingEndpoint<LastOperationResource> LastOperation(string? serviceId = null, string? planId = null, string? operation = null)
        => new LastOperationEndpoint(this, serviceId, planId, operation);
}
