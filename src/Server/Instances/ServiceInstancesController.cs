using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// exposes service instances
    /// </summary>
    [Route("v2/service_instances/{instance_id}")]
    public class ServiceInstancesController : BrokerControllerBase<IServiceInstanceBlocking, IServiceInstanceDeferred>
    {
        public ServiceInstancesController(IServiceProvider provider)
            : base(provider)
        {}

        /// <summary>
        /// fetches a service instance
        /// </summary>
        /// <param name="instanceId">id of instance to fetch</param>
        /// <response code="200">instance is in response body</response>
        /// <response code="404">instance does not exist or a provisioning operation is still in progress</response>
        /// <response code="422">instance is being updated and therefore cannot be fetched at this time</response>
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(ServiceInstanceResource), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(Error), 422)]
        public Task<IActionResult> Fetch(
            [FromRoute(Name = "instance_id"), Required] string instanceId)
        {
            return Do(allowDeferred: true,
                blocking: async x => Ok(await x.FetchAsync(instanceId)),
                deferred: async x => Ok(await x.FetchAsync(instanceId)));
        }

        /// <summary>
        /// provision a service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to provision</param>
        /// <param name="request">parameters for the requested service instance provision</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        /// <response code="200">instance already exists, is fully provisioned, and the requested parameters are identical to the existing instance</response>
        /// <response code="201">instance was provisioned as a result of this request</response>
        /// <response code="202">instance provisioning is in progress</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="409">instance with the same id already exists but with different attributes</response>
        /// <response code="422">broker only supports asynchronous processing for the requested operation and the request did not include ?accepts_incomplete=true</response>
        [HttpPut, Route("")]
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
                    return string.IsNullOrEmpty(result.Operation)
                        ? SyncResult(context, result.Result)
                        : AsyncResult(context, result, request);
                });
        }

        /// <summary>
        /// update a service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to update</param>
        /// <param name="request">parameters for the requested service instance update</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        /// <response code="200">request's changes have been applied or have had no effect</response>
        /// <response code="202">instance update is in progress</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="422">requested change is not supported or broker only supports asynchronous processing for the requested operation and the request did not include ?accepts_incomplete=true</response>
        [HttpPatch, Route("")]
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
                    return string.IsNullOrEmpty(result.Operation)
                        ? Ok()
                        : AsyncResult(context, result, request);
                });
        }

        /// <summary>
        /// deprovision a service instance
        /// </summary>
        /// <param name="instanceId">id of instance being deleted</param>
        /// <param name="serviceId">id of the service associated with the instance being deleted</param>
        /// <param name="planId">id of the plan associated with the instance being deleted</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        /// <response code="200">instance was deleted as a result of this request</response>
        /// <response code="202">instance deletion is in progress</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="410">instance does not exist</response>
        /// <response code="422">broker only supports asynchronous processing for the requested operation and the request did not include ?accepts_incomplete=true</response>
        [HttpDelete, Route("")]
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
                    return string.IsNullOrEmpty(result.Operation)
                        ? Ok()
                        : AsyncResult(context, result);
                });
        }

        /// <summary>
        /// get last requested operation state for service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to find last operation applied to it</param>
        /// <param name="serviceId">id of the service associated with the instance</param>
        /// <param name="planId">id of the plan associated with the instance</param>
        /// <param name="operation">a provided identifier for the operation</param>
        /// <response code="200">status is in response body</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="410">result of asynchronous delete operation: instance does not exist</response>
        [HttpGet, Route("last_operation")]
        [ProducesResponseType(typeof(LastOperationResource), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 410)]
        public Task<IActionResult> GetLastOperation(
            [FromRoute(Name = "instance_id"), Required] string instanceId,
            [FromQuery(Name = "service_id")] string serviceId = null,
            [FromQuery(Name = "plan_id")] string planId = null,
            [FromQuery(Name = "operation")] string operation = null)
        {
            var context = Context(instanceId);
            return Do(allowDeferred: true,
                blocking: _ => throw new NotSupportedException("This server does not support asynchronous operations."),
                deferred: async x => Ok(await x.GetLastOperationAsync(context, serviceId, planId, operation)));
        }

        private ServiceInstanceContext Context(string instanceId)
            => new ServiceInstanceContext(instanceId, OriginatingIdentity);

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
                        instanceId = context.InstanceId
                    },
                    result);
            }
        }

        private IActionResult AsyncResult(ServiceInstanceContext context, AsyncOperation result, IServicePlanReference request = null)
            => AcceptedAtAction(
                actionName: nameof(GetLastOperation),
                routeValues: new
                {
                    instanceId = context.InstanceId,
                    serviceId = request?.ServiceId,
                    planId = request?.PlanId,
                    operation = result.Operation
                },
                result);
    }
}
