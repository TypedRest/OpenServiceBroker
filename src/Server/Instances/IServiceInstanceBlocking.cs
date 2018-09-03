using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// manages service instances with blocking operations
    /// </summary>
    public interface IServiceInstanceBlocking : IServiceInstanceBase
    {
        /// <summary>
        /// provision a service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to provision</param>
        /// <param name="request">parameters for the requested service instance provision</param>
        /// <exception cref="ConflictException">instance with the same id already exists but with different attributes</exception>
        /// <returns>the provisioned instance</returns>
        Task<ServiceInstanceProvision> ProvisionAsync(string instanceId, ServiceInstanceProvisionRequest request);

        /// <summary>
        /// update a service instance
        /// </summary>
        /// <param name="instanceId">instance id of instance to update</param>
        /// <param name="request">parameters for the requested service instance update</param>
        /// <exception cref="BrokerException">requested change is not supported</exception>
        Task UpdateAsync(string instanceId, ServiceInstanceUpdateRequest request);

        /// <summary>
        /// deprovision a service instance
        /// </summary>
        /// <param name="instanceId">id of instance being deleted</param>
        /// <param name="serviceId">id of the service associated with the instance being deleted</param>
        /// <param name="planId">id of the plan associated with the instance being deleted</param>
        /// <exception cref="GoneException">instance does not exist</exception>
        Task DeprovisionAsync(string instanceId, string serviceId, string planId);
    }
}
