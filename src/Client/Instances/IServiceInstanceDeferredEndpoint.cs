using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Instances
{
    public interface IServiceInstanceDeferredEndpoint : IServiceInstanceEndpointBase<IServiceBindingDeferredEndpoint>
    {
        /// <summary>
        /// provision a service instance
        /// </summary>
        /// <param name="request">parameters for the requested service instance provision</param>
        /// <returns>the provisioned instance or an async operation (<see cref="LastOperation"/>)</returns>
        /// <exception cref="ConflictException">instance with the same id already exists but with different attributes</exception>
        Task<ServiceInstanceAsyncOperation> ProvisionAsync(ServiceInstanceProvisionRequest request);

        /// <summary>
        /// update a service instance
        /// </summary>
        /// <param name="request">parameters for the requested service instance update</param>
        /// <returns>completion indicator or an async operation (<see cref="LastOperation"/>)</returns>
        /// <exception cref="BrokerException">requested change is not supported</exception>
        Task<ServiceInstanceAsyncOperation> UpdateAsync(ServiceInstanceUpdateRequest request);

        /// <summary>
        /// deprovision a service instance
        /// </summary>
        /// <param name="serviceId">id of the service associated with the instance being deleted</param>
        /// <param name="planId">id of the plan associated with the instance being deleted</param>
        /// <returns>completion indicator or an async operation (<see cref="LastOperation"/>)</returns>
        /// <exception cref="GoneException">instance does not exist</exception>
        Task<AsyncOperation> DeprovisionAsync(string serviceId = null, string planId = null);

        /// <summary>
        /// get last requested operation state for service instance
        /// </summary>
        /// <param name="serviceId">id of the service associated with the instance</param>
        /// <param name="planId">id of the plan associated with the instance</param>
        /// <param name="operation">a provided identifier for the operation</param>
        IPollingEndpoint<LastOperationResource> LastOperation(string serviceId = null, string planId = null, string operation = null);
    }
}
