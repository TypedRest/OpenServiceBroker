using System.Reactive.Linq;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;
using TypedRest.Endpoints;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Represents a specific Service Instance. Uses potentially deferred (asynchronous) operations and automatically handles polling to make them appear blocking.
/// </summary>
/// <param name="referrer">The endpoint used to navigate to this one.</param>
/// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
public class ServiceInstancePollingEndpoint(IEndpoint referrer, Uri relativeUri)
    : ServiceInstanceEndpointBase<IServiceBindingEndpoint, ServiceBindingPollingEndpoint>(referrer, relativeUri), IServiceInstanceEndpoint
{
    private readonly IServiceInstanceDeferredEndpoint _inner = new ServiceInstanceDeferredEndpoint(referrer, relativeUri);

    public async Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceProvisionRequest request)
    {
        var response = await _inner.ProvisionAsync(request);

        if (string.IsNullOrEmpty(response.Operation))
            return response.Result;

        await LastOperationWaitAsync(request.ServiceId, request.PlanId, response.Operation);
        return new ServiceInstanceProvision {DashboardUrl = response.DashboardUrl};
    }

    public async Task UpdateAsync(ServiceInstanceUpdateRequest request)
    {
        var response = await _inner.UpdateAsync(request);

        if (string.IsNullOrEmpty(response.Operation))
            return;

        await LastOperationWaitAsync(request.ServiceId, request.PlanId, response.Operation);
    }

    public async Task DeprovisionAsync(string serviceId, string planId)
    {
        var response = await _inner.DeprovisionAsync(serviceId, planId);

        if (string.IsNullOrEmpty(response.Operation))
            return;

        await LastOperationWaitAsync(serviceId, planId, response.Operation);
    }

    private async Task LastOperationWaitAsync(string serviceId, string planId, string operation)
    {
        var result = await _inner.LastOperation(serviceId, planId, operation).GetObservable().LastAsync();
        if (result?.State != LastOperationResourceState.Succeeded)
            throw new BrokerException(result?.Description ?? "Asynchronous operation failed.", "AsyncFailed");
    }
}
