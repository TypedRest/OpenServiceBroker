using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances
{
    public interface IServiceInstanceEndpoint : IServiceInstanceEndpointBase<IServiceBindingEndpoint>
    {
        /// <summary>
        /// provision a service instance
        /// </summary>
        /// <param name="request">parameters for the requested service instance provision</param>
        /// <exception cref="ConflictException">instance with the same id already exists but with different attributes</exception>
        /// <returns>the provisioned instance</returns>
        Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceProvisionRequest request);

        /// <summary>
        /// update a service instance
        /// </summary>
        /// <param name="request">parameters for the requested service instance update</param>
        /// <exception cref="BrokerException">requested change is not supported</exception>
        Task UpdateAsync(ServiceInstanceUpdateRequest request);

        /// <summary>
        /// deprovision a service instance
        /// </summary>
        /// <param name="serviceId">id of the service associated with the instance being deleted</param>
        /// <param name="planId">id of the plan associated with the instance being deleted</param>
        /// <exception cref="GoneException">instance does not exist</exception>
        Task DeprovisionAsync(string serviceId, string planId);
    }
}
