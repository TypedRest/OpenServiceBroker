using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Bindings;

/// <summary>
/// Manages bindings for Service Instances with blocking operations.
/// </summary>
public interface IServiceBindingBlocking : IServiceBindingBase
{
    /// <summary>
    /// generates a Service Binding
    /// </summary>
    /// <param name="context">The id of binding to create.</param>
    /// <param name="request">Parameters for the requested Service Binding.</param>
    /// <exception cref="ConflictException">An instance with the same id already exists but with different attributes.</exception>
    Task<ServiceBinding> BindAsync(ServiceBindingContext context, ServiceBindingRequest request);

    /// <summary>
    /// deletes a Service Binding
    /// </summary>
    /// <param name="context">The id of the binding being deleted.</param>
    /// <param name="serviceId">The id of the service associated with the binding being deleted.</param>
    /// <param name="planId">The id of the plan associated with the binding being deleted.</param>
    /// <exception cref="GoneException">The binding does not exist (anymore).</exception>
    Task UnbindAsync(ServiceBindingContext context, string serviceId, string planId);
}
