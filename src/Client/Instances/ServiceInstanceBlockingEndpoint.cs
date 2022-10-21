using System.Net;
using OpenServiceBroker.Bindings;
using TypedRest.Endpoints;
using TypedRest.Http;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Represents a specific Service Instance with blocking operations. If the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.
/// </summary>
public class ServiceInstanceBlockingEndpoint : ServiceInstanceEndpointBase<IServiceBindingEndpoint, ServiceBindingBlockingEndpoint>, IServiceInstanceEndpoint
{
    /// <summary>
    /// Creates a new blocking Service Instance endpoint.
    /// </summary>
    /// <param name="referrer">The endpoint used to navigate to this one.</param>
    /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
    public ServiceInstanceBlockingEndpoint(IEndpoint referrer, Uri relativeUri)
        : base(referrer, relativeUri)
    {}

    public async Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceProvisionRequest request)
    {
        var response = await HandleAsync(() => HttpClient.PutAsync(Uri, request, Serializer));
        var result = await FromContentAsync<ServiceInstanceProvision>(response);
        result.Unchanged = (response.StatusCode == HttpStatusCode.OK);
        return result;
    }

    public Task UpdateAsync(ServiceInstanceUpdateRequest request)
        => HandleAsync(() => HttpClient.PatchAsync(Uri, request, Serializer));

    public Task DeprovisionAsync(string serviceId, string planId)
        => HandleAsync(() => HttpClient.DeleteAsync(GetDeleteUri(serviceId, planId)));
}
