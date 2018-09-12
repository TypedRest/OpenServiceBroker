using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker
{
    /// <summary>
    /// Common base class for Open Service Broker API controllers.
    /// </summary>
    /// <typeparam name="TBlocking">The service type to request from dependency injection for blocking operations.</typeparam>
    /// <typeparam name="TDeferred">The service type to request from dependency injection for deferred (asynchronous) operations.</typeparam>
    public abstract class BrokerControllerBase<TBlocking, TDeferred> : Controller
    {
        private readonly IServiceProvider _provider;

        protected BrokerControllerBase(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Performs either a blocking or a deferred operation, handling aspects such as API versioning and error serialization.
        /// </summary>
        /// <param name="acceptsIncomplete">A value of true indicates that the Platform and its clients support deferred (asynchronous) Service Broker operations. If this parameter is false, and the Service Broker can only handle a request deferred (asynchronously) <see cref="Errors.AsyncRequiredException"/> is thrown.</param>
        /// <param name="blocking">A callback to invoke for blocking operations.</param>
        /// <param name="deferred">A callback to invoke for deferred (asynchronous) operations.</param>
        /// <returns></returns>
        protected async Task<IActionResult> Do(
            bool acceptsIncomplete,
            Func<TBlocking, Task<IActionResult>> blocking,
            Func<TDeferred, Task<IActionResult>> deferred)
        {
            try
            {
                CheckApiVersion();
                ValidateModel();

                if (acceptsIncomplete)
                {
                    if (TryGetService<TDeferred>(out var asyncService))
                        return await deferred(asyncService);
                    else if (TryGetService<TBlocking>(out var syncService))
                        return await blocking(syncService);
                }
                else
                {
                    if (TryGetService<TBlocking>(out var syncService))
                        return await blocking(syncService);
                    else
                        throw new AsyncRequiredException();
                }
            }
            catch (BrokerException ex)
            {
                return StatusCode((int)ex.HttpCode, ex.ToResponse());
            }

            throw new InvalidOperationException($"Neither {typeof(TBlocking).Name} nor {typeof(TDeferred).Name} implementation was found.");
        }

        private void CheckApiVersion()
        {
            string headerValue = Request.Headers[ApiVersion.HttpHeaderName].FirstOrDefault();
            if (!string.IsNullOrEmpty(headerValue))
            {
                var clientVersion = ApiVersion.Parse(headerValue);
                if (clientVersion.Major != ApiVersion.Current.Major || clientVersion.Minor > ApiVersion.Current.Minor)
                    throw new ApiVersionNotSupportedException($"Client requested API version {clientVersion} but server only supports versions between {ApiVersion.Current.Major}.0 and {ApiVersion.Current}.");
            }
        }

        private void ValidateModel()
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(x => x.Errors).FirstOrDefault();
                throw new BadRequestException((error == null)
                    ? "Request was rejected by server due to semantic errors."
                    : (string.IsNullOrEmpty(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage));
            }
        }

        private bool TryGetService<T>(out T service)
        {
            service = _provider.GetService<T>();
            return (service != null);
        }

        /// <summary>
        /// Describes the identity of the user that initiated a request from the Platform.
        /// </summary>
        protected OriginatingIdentity OriginatingIdentity
        {
            get
            {
                string headerValue = Request.Headers[OriginatingIdentity.HttpHeaderName].FirstOrDefault();
                return string.IsNullOrEmpty(headerValue) ? null : OriginatingIdentity.Parse(headerValue);
            }
        }
    }
}
