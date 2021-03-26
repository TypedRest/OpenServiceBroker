using System;
using System.Net;
using System.Net.Http;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;
using TypedRest.Endpoints;
using TypedRest.Endpoints.Generic;
using TypedRest.Http;

namespace OpenServiceBroker
{
    /// <summary>
    /// A type-safe client for the Open Service Broker API.
    /// </summary>
    public class OpenServiceBrokerClient : EntryEndpoint, IOpenServiceBrokerClient
    {
        /// <summary>
        /// Creates a new Open Service Broker Client using a custom <see cref="HttpClient"/>. This is usually used for custom authentication schemes, e.g. client certificates.
        /// </summary>
        /// <param name="uri">The base URI of the Open Service Broker API instance (without the version number).</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use for communication with My Service.</param>
        public OpenServiceBrokerClient(HttpClient httpClient, Uri uri)
            : base(httpClient, uri, errorHandler: new OpenServiceBrokerErrorHandler())
        {
            SetApiVersion(DefaultApiVersion);
        }

        /// <summary>
        /// Creates a new Open Service Broker Client.
        /// </summary>
        /// <param name="uri">The base URI of the Open Service Broker API instance (without the version number).</param>
        /// <param name="credentials">Optional HTTP Basic Auth credentials used to authenticate against the REST interface.</param>
        public OpenServiceBrokerClient(Uri uri, NetworkCredential? credentials = null)
            : base(uri, errorHandler: new OpenServiceBrokerErrorHandler())
        {
            if (credentials != null) HttpClient.AddBasicAuth(credentials);
            SetApiVersion(DefaultApiVersion);
        }

        /// <summary>
        /// Creates a new Open Service Broker Client.
        /// </summary>
        /// <param name="uri">The base URI of the Open Service Broker API instance (without the version number).</param>
        /// <param name="token">The OAuth token to present as a "Bearer" to the REST interface.</param>
        public OpenServiceBrokerClient(Uri uri, string token)
            : base(uri, errorHandler: new OpenServiceBrokerErrorHandler())
        {
            SetApiVersion(DefaultApiVersion);
            HttpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }

        /// <summary>
        /// The default Open Service Broker API version to set for requests.
        /// </summary>
        public static ApiVersion DefaultApiVersion => new(2, 13);

        /// <summary>
        /// Sets the Open Service Broker API version to a custom value. Only use this if you know what you are doing!
        /// </summary>
        public void SetApiVersion(ApiVersion version)
        {
            HttpClient.DefaultRequestHeaders.Remove(ApiVersion.HttpHeaderName);
            HttpClient.DefaultRequestHeaders.Add(ApiVersion.HttpHeaderName, version.ToString());
        }

        /// <summary>
        /// Sets the identity of the user that initiated the request from the Platform.
        /// </summary>
        public void SetOriginatingIdentity(OriginatingIdentity identity)
        {
            HttpClient.DefaultRequestHeaders.Remove(OriginatingIdentity.HttpHeaderName);
            HttpClient.DefaultRequestHeaders.Add(OriginatingIdentity.HttpHeaderName, identity.ToString());
        }

        /// <summary>
        /// Exposes a list of all services available on the Service Broker.
        /// </summary>
        public IElementEndpoint<Catalog> Catalog
            => new ElementEndpoint<Catalog>(this, relativeUri: "./v2/catalog");

        /// <summary>
        /// Exposes Service Instances with blocking operations. If the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.
        /// </summary>
        public IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesBlocking
            => new IndexerEndpoint<ServiceInstanceBlockingEndpoint>(this, relativeUri: "./v2/service_instances");

        /// <summary>
        /// Exposes Service Instances with potentially deferred (asynchronous) operations.
        /// </summary>
        public IIndexerEndpoint<IServiceInstanceDeferredEndpoint> ServiceInstancesDeferred
            => new IndexerEndpoint<ServiceInstanceDeferredEndpoint>(this, relativeUri: "./v2/service_instances");

        /// <summary>
        /// Exposes Service Instances. Uses potentially deferred (asynchronous) operations and automatically handles polling to make them appear blocking.
        /// </summary>
        public IIndexerEndpoint<IServiceInstanceEndpoint> ServiceInstancesPolling
            => new IndexerEndpoint<ServiceInstancePollingEndpoint>(this, relativeUri: "./v2/service_instances");
    }
}
