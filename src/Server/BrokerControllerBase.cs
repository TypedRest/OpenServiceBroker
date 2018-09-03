using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker
{
    public abstract class BrokerControllerBase<TBlocking, TDeferred> : Controller
    {
        private readonly IServiceScope _scope;

        protected BrokerControllerBase(IServiceScopeFactory factory)
        {
            _scope = factory.CreateScope();
        }

        protected async Task<IActionResult> Do(
            bool allowDeferred,
            Func<TBlocking, Task<IActionResult>> blocking,
            Func<TDeferred, Task<IActionResult>> deferred)
        {
            try
            {
                if (allowDeferred)
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
                return StatusCode((int)ex.HttpCode, ex.ToDto());
            }

            throw new InvalidOperationException($"Neither {typeof(TBlocking).Name} nor {typeof(TDeferred).Name} implementation was found.");
        }

        private bool TryGetService<T>(out T service)
        {
            service = _scope.ServiceProvider.GetService<T>();
            return (service != null);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing) _scope.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
