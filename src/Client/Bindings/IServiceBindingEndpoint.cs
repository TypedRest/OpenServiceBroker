using System.Threading.Tasks;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings
{
    /// <summary>
    /// Represents a Service Binding for a specific Service Instance with blocking operations.
    /// </summary>
    /// <remarks>What a Service Instance represents can vary by service. Examples include a single database on a multi-tenant server, a dedicated cluster, or an account on a web application.</remarks>
    public interface IServiceBindingEndpoint : IServiceBindingEndpointBase
    {
        /// <summary>
        /// Generates the Service Binding.
        /// </summary>
        /// <param name="request">Parameters for the requested Service Binding.</param>
        /// <exception cref="ConflictException">A binding with the same id already exists but with different attributes.</exception>
        /// <returns>The binding.</returns>
        Task<ServiceBinding> BindAsync(ServiceBindingRequest request);

        /// <summary>
        /// Unbinds/deletes the Service Binding.
        /// </summary>
        /// <param name="serviceId">The id of the service associated with the binding being deleted.</param>
        /// <param name="planId">The id of the plan associated with the binding being deleted.</param>
        /// <exception cref="GoneException">The binding does not exist (anymore).</exception>
        Task UnbindAsync(string serviceId, string planId);
    }
}
