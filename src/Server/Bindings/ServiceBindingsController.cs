using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Exposes bindings for Service Instances.
/// </summary>
[Route("v2/service_instances/{instance_id}/service_bindings/{binding_id}")]
public class ServiceBindingsController : BrokerControllerBase<IServiceBindingBlocking, IServiceBindingDeferred>
{
    public ServiceBindingsController(IServiceProvider provider)
        : base(provider)
    {}

    /// <summary>
    /// Fetches a Service Binding.
    /// </summary>
    /// <param name="instanceId">The id of instance associated with the binding.</param>
    /// <param name="bindingId">The binding id of binding to fetch</param>
    /// <response code="200"/>
    /// <response code="404">The binding does not exist or a binding operation is still in progress.</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(ServiceBindingResource), 200)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Fetch(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromRoute(Name = "binding_id"), Required] string bindingId)
    {
        return Do(acceptsIncomplete: true,
            blocking: async x => Ok(await x.FetchAsync(instanceId, bindingId)),
            deferred: async x => Ok(await x.FetchAsync(instanceId, bindingId)));
    }

    /// <summary>
    /// Generates a Service Binding.
    /// </summary>
    /// <param name="instanceId">The id of instance to create a binding on.</param>
    /// <param name="bindingId">The binding id of binding to create.</param>
    /// <param name="request">Parameters for the requested Service Binding.</param>
    /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
    /// <response code="200">The binding already exists and the requested parameters are identical to the existing binding.</response>
    /// <response code="201">The binding was created as a result of this request.</response>
    /// <response code="202">The binding is in progress. See <see cref="GetLastOperation"/>.</response>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="409">The binding with the same id already exists but with different attributes.</response>
    /// <response code="422">The broker only supports asynchronous processing for the requested operation and the request did not include <paramref name="acceptsIncomplete"/>=true.</response>
    [HttpPut("")]
    [ProducesResponseType(typeof(ServiceBinding), 200)]
    [ProducesResponseType(typeof(ServiceBinding), 201)]
    [ProducesResponseType(typeof(AsyncOperation), 202)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 409)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Bind(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromRoute(Name = "binding_id"), Required] string bindingId,
        [FromBody, Required] ServiceBindingRequest request,
        [FromQuery(Name = "accepts_incomplete")] bool acceptsIncomplete = false)
    {
        var context = Context(instanceId, bindingId);
        return Do(acceptsIncomplete,
            blocking: async x => SyncResult(context, await x.BindAsync(context, request)),
            deferred: async x =>
            {
                var result = await x.BindAsync(context, request);
                return result.Completed
                    ? SyncResult(context, result.Result)
                    : AsyncResult(context, result, request);
            });
    }

    /// <summary>
    /// Unbinds/deletes a Service Binding.
    /// </summary>
    /// <param name="instanceId">The id of the instance associated with the binding being deleted.</param>
    /// <param name="bindingId">The id of the binding being deleted.</param>
    /// <param name="serviceId">The id of the service associated with the binding being deleted.</param>
    /// <param name="planId">The id of the plan associated with the binding being deleted.</param>
    /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
    /// <response code="200">The binding was deleted as a result of this request.</response>
    /// <response code="202">The binding deletion is in progress. See <see cref="GetLastOperation"/>.</response>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="410">The binding does not exist.</response>
    /// <response code="422">The broker only supports asynchronous processing for the requested operation and the request did not include <paramref name="acceptsIncomplete"/>=true.</response>
    [HttpDelete("")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(AsyncOperation), 202)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 410)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Unbind(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromRoute(Name = "binding_id"), Required] string bindingId,
        [FromQuery(Name = "service_id"), Required] string serviceId,
        [FromQuery(Name = "plan_id"), Required] string planId,
        [FromQuery(Name = "accepts_incomplete")] bool acceptsIncomplete = false)
    {
        var context = Context(instanceId, bindingId);
        return Do(acceptsIncomplete,
            blocking: async x =>
            {
                await x.UnbindAsync(context, serviceId, planId);
                return Ok();
            },
            deferred: async x =>
            {
                var result = await x.UnbindAsync(context, serviceId, planId);
                return result.Completed
                    ? Ok()
                    : AsyncResult(context, result);
            });
    }

    /// <summary>
    /// Gets the state of the last requested deferred (asynchronous) operation for a Service Binding.
    /// </summary>
    /// <param name="instanceId">The id of instance to find last operation applied to it</param>
    /// <param name="bindingId">The binding id of Service Binding to find last operation applied to it</param>
    /// <param name="serviceId">The id of the service associated with the binding.</param>
    /// <param name="planId">The id of the plan associated with the binding.</param>
    /// <param name="operation">The value provided in <see cref="AsyncOperation.Operation"/>.</param>
    /// <response code="200"/>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="410">The binding requested to be deleted does not exist (anymore).</response>
    [HttpGet("last_operation")]
    [ProducesResponseType(typeof(LastOperationResource), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 410)]
    public Task<IActionResult> GetLastOperation(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromRoute(Name = "binding_id"), Required] string bindingId,
        [FromQuery(Name = "service_id")] string? serviceId = null,
        [FromQuery(Name = "plan_id")] string? planId = null,
        [FromQuery(Name = "operation")] string? operation = null)
    {
        var context = Context(instanceId, bindingId);
        return Do(acceptsIncomplete: true,
            blocking: _ => throw new NotSupportedException("This server does not support asynchronous operations."),
            deferred: async x => Ok(await x.GetLastOperationAsync(context, serviceId, planId, operation)));
    }

    private ServiceBindingContext Context(string instanceId, string bindingId)
        => new(instanceId, bindingId, OriginatingIdentity);

    private IActionResult SyncResult(ServiceBindingContext context, IUnchangedFlag result)
    {
        if (result.Unchanged)
            return Ok(result);
        else
        {
            return CreatedAtAction(
                actionName: nameof(Fetch),
                routeValues: new
                {
                    instance_id = context.InstanceId,
                    binding_id = context.BindingId
                },
                result);
        }
    }

    private IActionResult AsyncResult(ServiceBindingContext context, AsyncOperation result, IServicePlanReference? request = null)
        => AcceptedAtAction(
            actionName: nameof(GetLastOperation),
            routeValues: new
            {
                instance_id = context.InstanceId,
                binding_id = context.BindingId,
                service_id = request?.ServiceId,
                plan_id = request?.PlanId,
                operation = result.Operation
            },
            result);
}
