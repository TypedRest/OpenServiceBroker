using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// exposes bindings for service instances
    /// </summary>
    [Route("v2/service_instances/{instance_id}/service_bindings/{binding_id}")]
    public class ServiceBindingsController : BrokerControllerBase<IServiceBindingBlocking, IServiceBindingDeferred>
    {
        public ServiceBindingsController(IServiceProvider provider)
            : base(provider)
        {}

        /// <summary>
        /// fetches a service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance associated with the binding</param>
        /// <param name="bindingId">binding id of binding to fetch</param>
        /// <response code="200">binding is in response body</response>
        /// <response code="404">binding does not exist or a binding operation is still in progress</response>
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(ServiceBindingResource), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(Error), 422)]
        public Task<IActionResult> Fetch(
            [FromRoute(Name = "instance_id"), Required] string instanceId,
            [FromRoute(Name = "binding_id"), Required] string bindingId)
        {
            return Do(allowDeferred: true,
                blocking: async x => Ok(await x.FetchAsync(instanceId, bindingId)),
                deferred: async x => Ok(await x.FetchAsync(instanceId, bindingId)));
        }

        /// <summary>
        /// generates a service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance to create a binding on</param>
        /// <param name="bindingId">binding id of binding to create</param>
        /// <param name="request">parameters for the requested service binding</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        /// <response code="200">binding already exists and the requested parameters are identical to the existing binding</response>
        /// <response code="201">binding was created as a result of this request</response>
        /// <response code="202">binding is in progress</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="409">binding with the same id already exists but with different attributes</response>
        /// <response code="422">broker only supports asynchronous processing for the requested operation and the request did not include ?accepts_incomplete=true</response>
        [HttpPut, Route("")]
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
                    return string.IsNullOrEmpty(result.Operation)
                        ? SyncResult(context, result.Result)
                        : AsyncResult(context, result, request);
                });
        }

        /// <summary>
        /// deletes a service binding
        /// </summary>
        /// <param name="instanceId">id of the instance associated with the binding being deleted</param>
        /// <param name="bindingId">id of the binding being deleted</param>
        /// <param name="serviceId">id of the service associated with the binding being deleted</param>
        /// <param name="planId">id of the plan associated with the binding being deleted</param>
        /// <param name="acceptsIncomplete">deferred (asynchronous) operations supported</param>
        /// <response code="200">binding was deleted as a result of this request</response>
        /// <response code="202">binding deletion is in progress</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="410">binding does not exist</response>
        /// <response code="422">broker only supports asynchronous processing for the requested operation and the request did not include ?accepts_incomplete=true</response>
        [HttpDelete, Route("")]
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
                    return string.IsNullOrEmpty(result.Operation)
                        ? Ok()
                        : AsyncResult(context, result);
                });
        }

        /// <summary>
        /// get last requested operation state for service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance to find last operation applied to it</param>
        /// <param name="bindingId">binding id of service binding to find last operation applied to it</param>
        /// <param name="serviceId">id of the service associated with the binding</param>
        /// <param name="planId">id of the plan associated with the binding</param>
        /// <param name="operation">a provided identifier for the operation</param>
        /// <response code="200">status is in response body</response>
        /// <response code="400">request is malformed or missing mandatory data</response>
        /// <response code="410">result of asynchronous delete operation: binding does not exist</response>
        [HttpGet, Route("last_operation")]
        [ProducesResponseType(typeof(LastOperationResource), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 410)]
        public Task<IActionResult> GetLastOperation(
            [FromRoute(Name = "instance_id"), Required] string instanceId,
            [FromRoute(Name = "binding_id"), Required] string bindingId,
            [FromQuery(Name = "service_id")] string serviceId = null,
            [FromQuery(Name = "plan_id")] string planId = null,
            [FromQuery(Name = "operation")] string operation = null)
        {
            var context = Context(instanceId, bindingId);
            return Do(allowDeferred: true,
                blocking: _ => throw new NotSupportedException("This server does not support asynchronous operations."),
                deferred: async x => Ok(await x.GetLastOperationAsync(context, serviceId, planId, operation)));
        }

        private ServiceBindingContext Context(string instanceId, string bindingId)
            => new ServiceBindingContext(instanceId, bindingId, OriginatingIdentity);

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
                        instanceId = context.InstanceId,
                        bindingId = context.BindingId
                    },
                    result);
            }
        }

        private IActionResult AsyncResult(ServiceBindingContext context, AsyncOperation result, IServicePlanReference request = null)
            => AcceptedAtAction(
                actionName: nameof(GetLastOperation),
                routeValues: new
                {
                    instanceId = context.InstanceId,
                    bindingId = context.BindingId,
                    serviceId = request?.ServiceId,
                    planId = request?.PlanId,
                    operation = result.Operation
                },
                result);
    }
}
