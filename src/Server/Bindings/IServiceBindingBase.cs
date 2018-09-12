using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// Common base for <see cref="IServiceBindingBlocking"/> and <see cref="IServiceBindingDeferred"/>; do not use directly!
    /// </summary>
    public interface IServiceBindingBase
    {
        /// <summary>
        /// fetches a Service Binding
        /// </summary>
        /// <param name="instanceId">The id of instance associated with the binding.</param>
        /// <param name="bindingId">binding id of binding to fetch</param>
        /// <exception cref="NotFoundException">The binding does not exist or a binding operation is still in progress.</exception>
        Task<ServiceBindingResource> FetchAsync(string instanceId, string bindingId);
    }
}
