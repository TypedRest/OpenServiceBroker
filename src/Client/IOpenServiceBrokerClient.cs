using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;
using TypedRest.Endpoints.Generic;

namespace OpenServiceBroker;

/// <summary>
/// A type-safe client for the Open Service Broker API.
/// </summary>
public interface IOpenServiceBrokerClient
{
    /// <summary>
    /// Exposes a list of all services available on the Service Broker.
    /// </summary>
    IElementEndpoint<Catalog> Catalog { get; }

    /// <summary>
    /// Exposes Service Instances with blocking operations. If the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.
    /// </summary>
    IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesBlocking { get; }

    /// <summary>
    /// Exposes Service Instances with potentially deferred (asynchronous) operations.
    /// </summary>
    IIndexerEndpoint<IServiceInstanceDeferredEndpoint> ServiceInstancesDeferred { get; }

    /// <summary>
    /// Exposes Service Instances. Uses potentially deferred (asynchronous) operations and automatically handles polling to make them appear blocking.
    /// </summary>
    IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesPolling { get; }
}
