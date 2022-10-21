using OpenServiceBroker.Errors;
using TypedRest.Endpoints.Reactive;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Represents a Service Binding for a specific Service Instance with potentially deferred (asynchronous) operations.
/// </summary>
/// <remarks>What a Service Instance represents can vary by service. Examples include a single database on a multi-tenant server, a dedicated cluster, or an account on a web application.</remarks>
public interface IServiceBindingDeferredEndpoint : IServiceBindingEndpointBase
{
    /// <summary>
    /// Generates the Service Binding.
    /// </summary>
    /// <param name="request">Parameters for the requested Service Binding.</param>
    /// <returns>A potentially deferred (asynchronous) operation. If <see cref="AsyncOperation.Completed"/> is false, start polling <see cref="LastOperation"/>.</returns>
    /// <exception cref="ConflictException">A binding with the same id already exists but with different attributes.</exception>
    Task<ServiceBindingAsyncOperation> BindAsync(ServiceBindingRequest request);

    /// <summary>
    /// Unbinds/deletes the Service Binding.
    /// </summary>
    /// <param name="serviceId">The id of the service associated with the binding being deleted.</param>
    /// <param name="planId">The id of the plan associated with the binding being deleted.</param>
    /// <returns>A potentially deferred (asynchronous) operation. If <see cref="AsyncOperation.Completed"/> is false, start polling <see cref="LastOperation"/>.</returns>
    /// <exception cref="GoneException">The binding does not exist (anymore).</exception>
    Task<AsyncOperation> UnbindAsync(string serviceId, string planId);

    /// <summary>
    /// Provides an endpoint to obtain the state of the last requested deferred (asynchronous) operation.
    /// </summary>
    /// <param name="serviceId">If present, it MUST be the ID of the service being used.</param>
    /// <param name="planId">If present, it MUST be the ID of the plan for the Service Instance. If this endpoint is being polled as a result of changing the plan through a Service Instance Update, the ID of the plan prior to the update MUST be used.</param>
    /// <param name="operation">A Service Broker-provided identifier for the operation. When a value for operation is included with deferred (asynchronous) responses for Provision, Update, and Deprovision requests, the Platform MUST provide the same value using this query parameter. If present, MUST be a non-empty string.</param>
    IPollingEndpoint<LastOperationResource> LastOperation(string? serviceId = null, string? planId = null, string? operation = null);
}
