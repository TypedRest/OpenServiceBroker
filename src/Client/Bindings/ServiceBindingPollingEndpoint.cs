using System.Reactive.Linq;
using OpenServiceBroker.Errors;
using TypedRest.Endpoints;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Represents a Service Binding for a specific Service Instance. Uses potentially deferred (asynchronous) operations and automatically handles polling to make them appear blocking.
/// </summary>
/// <param name="referrer">The endpoint used to navigate to this one.</param>
/// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
public class ServiceBindingPollingEndpoint(IEndpoint referrer, Uri relativeUri)
    : ServiceBindingEndpointBase(referrer, relativeUri), IServiceBindingEndpoint
{
    private readonly IServiceBindingDeferredEndpoint _inner = new ServiceBindingDeferredEndpoint(referrer, relativeUri);

    public async Task<ServiceBinding> BindAsync(ServiceBindingRequest request)
    {
        var response = await _inner.BindAsync(request);

        if (string.IsNullOrEmpty(response.Operation))
            return response.Result;

        await LastOperationWaitAsync(request.ServiceId, request.PlanId, response.Operation);

        var resource = await FetchAsync();
        var result = new ServiceBinding
        {
            Credentials = resource.Credentials,
            RouteServiceUrl = resource.RouteServiceUrl,
            SyslogDrainUrl = resource.SyslogDrainUrl
        };
        result.VolumeMounts.AddRange(resource.VolumeMounts);
        return result;
    }

    public async Task UnbindAsync(string serviceId, string planId)
    {
        var response = await _inner.UnbindAsync(serviceId, planId);

        if (string.IsNullOrEmpty(response.Operation))
            return;

        await LastOperationWaitAsync(serviceId, planId, response.Operation);
    }

    private async Task LastOperationWaitAsync(string serviceId, string planId, string operation)
    {
        var result = await _inner.LastOperation(serviceId, planId, operation).GetObservable().LastAsync();
        if (result.State != LastOperationResourceState.Succeeded)
            throw new BrokerException(result.Description, "AsyncFailed");
    }
}
