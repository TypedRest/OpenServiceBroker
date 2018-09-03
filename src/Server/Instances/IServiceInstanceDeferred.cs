using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// manages service instances with potentially deferred (asynchronous) operations
    /// </summary>
    public interface IServiceInstanceDeferred : IServiceInstanceBase
    {
        /// <summary>
        /// provision a service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to provision</param>
        /// <param name="request">parameters for the requested service instance provision</param>
        /// <returns>the provisioned instance or an async operation (<see cref="GetLastOperationAsync"/>)</returns>
        /// <exception cref="ConflictException">instance with the same id already exists but with different attributes</exception>
        Task<ServiceInstanceAsyncOperation> ProvisionAsync(string instanceId, ServiceInstanceProvisionRequest request);

        /// <summary>
        /// update a service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to update</param>
        /// <param name="request">parameters for the requested service instance update</param>
        /// <returns>completion indicator or an async operation (<see cref="GetLastOperationAsync"/>)</returns>
        /// <exception cref="BrokerException">requested change is not supported</exception>
        Task<ServiceInstanceAsyncOperation> UpdateAsync(string instanceId, ServiceInstanceUpdateRequest request);

        /// <summary>
        /// deprovision a service instance
        /// </summary>
        /// <param name="instanceId">id of instance being deleted</param>
        /// <param name="serviceId">id of the service associated with the instance being deleted</param>
        /// <param name="planId">id of the plan associated with the instance being deleted</param>
        /// <returns>completion indicator or an async operation (<see cref="GetLastOperationAsync"/>)</returns>
        /// <exception cref="GoneException">instance does not exist</exception>
        Task<AsyncOperation> DeprovisionAsync(string instanceId, string serviceId = null, string planId = null);

        /// <summary>
        /// get last requested operation state for service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to find last operation applied to it</param>
        /// <param name="serviceId">id of the service associated with the instance</param>
        /// <param name="planId">id of the plan associated with the instance</param>
        /// <param name="operation">a provided identifier for the operation</param>
        /// <exception cref="GoneException">result of asynchronous delete operation: instance does not exist</exception>
        Task<LastOperationResource> GetLastOperationAsync(string instanceId, string serviceId = null, string planId = null, string operation = null);
    }
}
