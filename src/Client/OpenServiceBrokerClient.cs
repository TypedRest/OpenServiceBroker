using System;
using System.Net;
using System.Net.Http;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;
using TypedRest;

namespace OpenServiceBroker
{
    /// <summary>
    /// Provides a type-safe client for the Open Service Broker API.
    /// </summary>
    public class OpenServiceBrokerClient : EntryEndpoint, IOpenServiceBrokerClient
    {
        /// <summary>
        /// Creates a new Open Service Broker Client using a custom <see cref="HttpClient"/>. This is usually used for custom authentication schemes, e.g. client certificates.
        /// </summary>
        /// <param name="uri">The base URI of the Open Service Broker API instance (without the version number).</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use for communication with My Service.</param>
        public OpenServiceBrokerClient(Uri uri, HttpClient httpClient)
            : base(uri, httpClient)
        {
            SetApiVersion();
        }

        /// <summary>
        /// Creates a new Open Service Broker Client.
        /// </summary>
        /// <param name="uri">The base URI of the Open Service Broker API instance (without the version number).</param>
        /// <param name="credentials">Optional HTTP Basic Auth credentials used to authenticate against the REST interface.</param>
        public OpenServiceBrokerClient(Uri uri, ICredentials credentials = null)
            : base(uri, credentials)
        {
            SetApiVersion();
        }

        /// <summary>
        /// Creates a new Open Service Broker Client.
        /// </summary>
        /// <param name="uri">The base URI of the Open Service Broker API instance (without the version number).</param>
        /// <param name="token">The OAuth token to present as a "Bearer" to the REST interface.</param>
        public OpenServiceBrokerClient(Uri uri, string token)
            : base(uri, token)
        {
            SetApiVersion();
        }

        /// <summary>
        /// Sets the Open Service Broker API version to the default value of <see cref="ApiVersion.Current"/>.
        /// </summary>
        public void SetApiVersion()
            => SetApiVersion(ApiVersion.Current);

        /// <summary>
        /// Sets the Open Service Broker API version to a custom value. Only use this if you know what you are doing!
        /// </summary>
        public void SetApiVersion(ApiVersion version)
            => SetCustomHeader(ApiVersion.HttpHeaderName, version.ToString());

        /// <summary>
        /// Sets the identity of the user that initiated the request from the Platform.
        /// </summary>
        public void SetOriginatingIdentity(OriginatingIdentity identity)
            => SetCustomHeader(OriginatingIdentity.HttpHeaderName, identity.ToString());

        /// <summary>
        /// exposes the catalog of services that the service broker offers
        /// </summary>
        public IElementEndpoint<Catalog> Catalog
            => new ElementEndpoint<Catalog>(this, relativeUri: "./v2/catalog");

        /// <summary>
        /// exposes service instances with blocking operations
        /// </summary>
        public IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesBlocking
            => new IndexerEndpoint<ServiceInstanceBlockingEndpoint>(this, relativeUri: "./v2/service_instances");

        /// <summary>
        /// exposes service instances with potentially deferred (asynchronous) operations
        /// </summary>
        public IIndexerEndpoint<IServiceInstanceDeferredEndpoint> ServiceInstancesDeferred
            => new IndexerEndpoint<ServiceInstanceDeferredEndpoint>(this, relativeUri: "./v2/service_instances");

        /// <summary>
        /// exposes service instances with polling operations
        /// </summary>
        public IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesPolling
            => new IndexerEndpoint<ServiceInstancePollingEndpoint>(this, relativeUri: "./v2/service_instances");
    }
}
