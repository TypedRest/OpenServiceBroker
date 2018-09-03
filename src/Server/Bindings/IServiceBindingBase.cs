using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// common base for <see cref="IServiceBindingBlocking"/> and <see cref="IServiceBindingDeferred"/>; do not implement directly!
    /// </summary>
    public interface IServiceBindingBase
    {
        /// <summary>
        /// fetches a service binding
        /// </summary>
        /// <param name="instanceId">instance id of instance associated with the binding</param>
        /// <param name="bindingId">binding id of binding to fetch</param>
        /// <exception cref="NotFoundException">binding does not exist or a binding operation is still in progress</exception>
        Task<ServiceBindingResource> FetchAsync(string instanceId, string bindingId);
    }
}
