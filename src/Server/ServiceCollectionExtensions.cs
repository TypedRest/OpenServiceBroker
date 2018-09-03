using Microsoft.Extensions.DependencyInjection;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;

namespace OpenServiceBroker
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers ASP.NET Core MVC Controllers implementing the Open Service Broker API.
        /// </summary>
        /// <remarks>
        /// Make sure to also register your implementations of:
        /// <see cref="ICatalogService"/>,
        /// <see cref="IServiceInstanceBlocking"/> or <see cref="IServiceInstanceDeferred"/>,
        /// optionally <see cref="IServiceBindingBlocking"/> or <see cref="IServiceBindingDeferred"/>.
        /// </remarks>
        public static IServiceCollection AddOpenServiceBroker(this IServiceCollection services)
            => services.AddTransient<CatalogController>()
                       .AddTransient<ServiceInstancesController>()
                       .AddTransient<ServiceBindingsController>();
    }
}
