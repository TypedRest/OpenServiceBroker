using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// common base for <see cref="IServiceInstanceBlocking"/> and <see cref="IServiceInstanceDeferred"/>; do not implement directly!
    /// </summary>
    public interface IServiceInstanceBase
    {
        /// <summary>
        /// fetches a service instance
        /// </summary>
        /// <param name="instanceId">id of instance to fetch</param>
        /// <exception cref="NotFoundException">instance does not exist or a provisioning operation is still in progress</exception>
        /// <exception cref="ConcurrencyException">instance is being updated and therefore cannot be fetched at this time</exception>
        Task<ServiceInstanceResource> FetchAsync(string instanceId);
    }
}
