using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

/// <summary>
/// Exposes Service Instances.
/// </summary>
[Route("v2/service_instances/{instance_id}")]
public class ServiceInstancesController : BrokerControllerBase<IServiceInstanceBlocking, IServiceInstanceDeferred>
{
    public ServiceInstancesController(IServiceProvider provider)
        : base(provider)
    {}

    /// <summary>
    /// Fetches a Service Instance.
    /// </summary>
    /// <param name="instanceId">The id of instance to fetch.</param>
    /// <response code="200"/>
    /// <response code="404">The instance does not exist or a provisioning operation is still in progress.</response>
    /// <response code="422">The instance is being updated and therefore cannot be fetched at this time.</response>
    [HttpGet("")]
    [ProducesResponseType(typeof(ServiceInstanceResource), 200)]
    [ProducesResponseType(typeof(Error), 404)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Fetch(
        [FromRoute(Name = "instance_id"), Required] string instanceId)
    {
        return Do(acceptsIncomplete: true,
            blocking: async x => Ok(await x.FetchAsync(instanceId)),
            deferred: async x => Ok(await x.FetchAsync(instanceId)));
    }

    /// <summary>
    /// Provisions a Service Instance.
    /// </summary>
    /// <param name="instanceId">The id of instance to provision.</param>
    /// <param name="request">Parameters for the requested Service Instance provision</param>
    /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
    /// <response code="200">The instance already exists, is fully provisioned, and the requested parameters are identical to the existing instance.</response>
    /// <response code="201">The instance was provisioned as a result of this request.</response>
    /// <response code="202">The instance provisioning is in progress. See <see cref="GetLastOperation"/>.</response>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="409">The instance with the same id already exists but with different attributes.</response>
    /// <response code="422">The broker only supports asynchronous processing for the requested operation and the request did not include <paramref name="acceptsIncomplete"/>=true.</response>
    [HttpPut("")]
    [ProducesResponseType(typeof(ServiceInstanceProvision), 200)]
    [ProducesResponseType(typeof(ServiceInstanceProvision), 201)]
    [ProducesResponseType(typeof(ServiceInstanceAsyncOperation), 202)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 409)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Provision(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromBody, Required] ServiceInstanceProvisionRequest request,
        [FromQuery(Name = "accepts_incomplete")] bool acceptsIncomplete = false)
    {
        var context = Context(instanceId);
        return Do(acceptsIncomplete,
            blocking: async x => SyncResult(context, await x.ProvisionAsync(context, request)),
            deferred: async x =>
            {
                var result = await x.ProvisionAsync(context, request);
                return result.Completed
                    ? SyncResult(context, result.Result)
                    : AsyncResult(context, result, request);
            });
    }

    /// <summary>
    /// Updates a Service Instance.
    /// </summary>
    /// <param name="instanceId">The instance id of instance to update</param>
    /// <param name="request">Parameters for the requested Service Instance update</param>
    /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
    /// <response code="200">The request's changes have been applied or have had no effect.</response>
    /// <response code="202">The instance update is in progress. See <see cref="GetLastOperation"/>.</response>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="422">The requested change is not supported or the broker only supports asynchronous processing for the requested operation and the request did not include <paramref name="acceptsIncomplete"/>=true.</response>
    [HttpPatch("")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ServiceInstanceAsyncOperation), 202)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Update(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromBody, Required] ServiceInstanceUpdateRequest request,
        [FromQuery(Name = "accepts_incomplete")] bool acceptsIncomplete = false)
    {
        var context = Context(instanceId);
        return Do(acceptsIncomplete,
            blocking: async x =>
            {
                await x.UpdateAsync(context, request);
                return Ok();
            },
            deferred: async x =>
            {
                var result = await x.UpdateAsync(context, request);
                return result.Completed
                    ? Ok()
                    : AsyncResult(context, result, request);
            });
    }

    /// <summary>
    /// Deprovisions/deletes a Service Instance.
    /// </summary>
    /// <param name="instanceId">The id of instance being deleted.</param>
    /// <param name="serviceId">The id of the service associated with the instance being deleted.</param>
    /// <param name="planId">The id of the plan associated with the instance being deleted.</param>
    /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
    /// <response code="200">The instance was deleted as a result of this request.</response>
    /// <response code="202">The instance deletion is in progress. See <see cref="GetLastOperation"/>.</response>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="410">The instance does not exist.</response>
    /// <response code="422">The broker only supports asynchronous processing for the requested operation and the request did not include <paramref name="acceptsIncomplete"/>=true.</response>
    [HttpDelete("")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ServiceInstanceAsyncOperation), 202)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 410)]
    [ProducesResponseType(typeof(Error), 422)]
    public Task<IActionResult> Deprovision(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromQuery(Name = "service_id"), Required] string serviceId,
        [FromQuery(Name = "plan_id"), Required] string planId,
        [FromQuery(Name = "accepts_incomplete")] bool acceptsIncomplete = false)
    {
        var context = Context(instanceId);
        return Do(acceptsIncomplete,
            blocking: async x =>
            {
                await x.DeprovisionAsync(context, serviceId, planId);
                return Ok();
            },
            deferred: async x =>
            {
                var result = await x.DeprovisionAsync(context, serviceId, planId);
                return result.Completed
                    ? Ok()
                    : AsyncResult(context, result);
            });
    }

    /// <summary>
    /// Gets the state of the last requested deferred (asynchronous) operation for a Service Instance.
    /// </summary>
    /// <param name="instanceId">The instance id of instance to find last operation applied to it</param>
    /// <param name="serviceId">The id of the service associated with the instance.</param>
    /// <param name="planId">The id of the plan associated with the instance.</param>
    /// <param name="operation">The value provided in <see cref="AsyncOperation.Operation"/>.</param>
    /// <response code="200"/>
    /// <response code="400">The request is malformed or missing mandatory data.</response>
    /// <response code="410">The instance requested to be deleted does not exist (anymore).</response>
    [HttpGet("last_operation")]
    [ProducesResponseType(typeof(LastOperationResource), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 410)]
    public Task<IActionResult> GetLastOperation(
        [FromRoute(Name = "instance_id"), Required] string instanceId,
        [FromQuery(Name = "service_id")] string? serviceId = null,
        [FromQuery(Name = "plan_id")] string? planId = null,
        [FromQuery(Name = "operation")] string? operation = null)
    {
        var context = Context(instanceId);
        return Do(acceptsIncomplete: true,
            blocking: _ => throw new NotSupportedException("This server does not support asynchronous operations."),
            deferred: async x => Ok(await x.GetLastOperationAsync(context, serviceId, planId, operation)));
    }

    private ServiceInstanceContext Context(string? instanceId)
        => new(instanceId, OriginatingIdentity);

    private IActionResult SyncResult(ServiceInstanceContext context, IUnchangedFlag result)
    {
        if (result.Unchanged)
            return Ok(result);
        else
        {
            return CreatedAtAction(
                actionName: nameof(Fetch),
                routeValues: new
                {
                    instance_id = context.InstanceId
                },
                result);
        }
    }

    private IActionResult AsyncResult(ServiceInstanceContext context, AsyncOperation result, IServicePlanReference? request = null)
        => AcceptedAtAction(
            actionName: nameof(GetLastOperation),
            routeValues: new
            {
                instance_id = context.InstanceId,
                service_id = request?.ServiceId,
                plan_id = request?.PlanId,
                operation = result.Operation
            },
            result);
}
