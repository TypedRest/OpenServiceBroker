using Microsoft.Extensions.DependencyInjection;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;

namespace OpenServiceBroker
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Registers API Controllers implementing the Open Service Broker API.
        /// </summary>
        /// <remarks>
        /// Make sure to also register your implementations of:
        /// <see cref="ICatalogService"/>,
        /// <see cref="IServiceInstanceBlocking"/> or <see cref="IServiceInstanceDeferred"/>,
        /// optionally <see cref="IServiceBindingBlocking"/> or <see cref="IServiceBindingDeferred"/>.
        /// </remarks>
        public static IMvcBuilder AddOpenServiceBroker(this IMvcBuilder builder)
        {
            builder.AddApplicationPart(typeof(CatalogController).Assembly);
            builder.AddNewtonsoftJson();

            return builder;
        }
    }
}
