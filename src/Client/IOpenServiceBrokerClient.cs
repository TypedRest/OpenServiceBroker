using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;
using TypedRest;

namespace OpenServiceBroker
{
    public interface IOpenServiceBrokerClient
    {
        /// <summary>
        /// exposes the catalog of services that the service broker offers
        /// </summary>
        IElementEndpoint<Catalog> Catalog { get; }

        /// <summary>
        /// exposes service instances with blocking operations
        /// </summary>
        IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesBlocking { get; }

        /// <summary>
        /// exposes service instances with potentially deferred (asynchronous) operations
        /// </summary>
        IIndexerEndpoint<IServiceInstanceDeferredEndpoint> ServiceInstancesDeferred { get; }

        /// <summary>
        /// exposes service instances with polling operations
        /// </summary>
        IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesPolling { get; }
    }
}
