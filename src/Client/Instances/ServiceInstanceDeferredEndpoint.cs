using OpenServiceBroker.Bindings;
using TypedRest.Endpoints;
using TypedRest.Endpoints.Reactive;
using TypedRest.Http;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Represents a specific Service Instance with potentially deferred (asynchronous) operations.
/// </summary>
/// <param name="referrer">The endpoint used to navigate to this one.</param>
/// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
public class ServiceInstanceDeferredEndpoint(IEndpoint referrer, Uri relativeUri)
    : ServiceInstanceEndpointBase<IServiceBindingDeferredEndpoint, ServiceBindingDeferredEndpoint>(referrer, relativeUri, acceptsIncomplete: true), IServiceInstanceDeferredEndpoint
{
    public async Task<ServiceInstanceAsyncOperation> ProvisionAsync(ServiceInstanceProvisionRequest request)
    {
        var response = await HandleAsync(() => HttpClient.PutAsync(Uri, request, Serializer));
        return await ParseDeferredResponseAsync<ServiceInstanceProvision, ServiceInstanceAsyncOperation>(response);
    }

    public async Task<ServiceInstanceAsyncOperation> UpdateAsync(ServiceInstanceUpdateRequest request)
    {
        var response = await HandleAsync(() => HttpClient.PatchAsync(Uri, request, Serializer));
        return await ParseDeferredResponseAsync<ServiceInstanceProvision, ServiceInstanceAsyncOperation>(response);
    }

    public async Task<AsyncOperation> DeprovisionAsync(string? serviceId = null, string? planId = null)
    {
        var response = await HandleAsync(() => HttpClient.DeleteAsync(GetDeleteUri(serviceId, planId)));
        return await ParseDeferredResponseAsync(response);
    }

    public IPollingEndpoint<LastOperationResource> LastOperation(string? serviceId = null, string? planId = null, string? operation = null)
        => new LastOperationEndpoint(this, serviceId, planId, operation);
}
