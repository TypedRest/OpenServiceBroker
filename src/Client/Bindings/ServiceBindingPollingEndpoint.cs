using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingPollingEndpoint : ServiceBindingEndpointBase, IServiceBindingEndpoint
    {
        private readonly IServiceBindingDeferredEndpoint _inner;

        /// <summary>
        /// Creates a new service binding endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        public ServiceBindingPollingEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri)
        {
            _inner = new ServiceBindingDeferredEndpoint(referrer, relativeUri);
        }

        public async Task<ServiceBinding> BindAsync(ServiceBindingRequest request)
        {
            var response = await _inner.BindAsync(request);

            if (string.IsNullOrEmpty(response.Operation))
                return response.Result;

            await LastOperationWaitAsync(request.ServiceId, request.PlanId, response.Operation);

            var resource = await FetchAsync();
            var result = new ServiceBinding
            {
                Credentials = resource.Credentials,
                RouteServiceUrl = resource.RouteServiceUrl,
                SyslogDrainUrl = resource.SyslogDrainUrl
            };
            result.VolumeMounts.AddRange(resource.VolumeMounts);
            return result;
        }

        public async Task UnbindAsync(string serviceId, string planId)
        {
            var response = await _inner.UnbindAsync(serviceId, planId);

            if (string.IsNullOrEmpty(response.Operation))
                return;

            await LastOperationWaitAsync(serviceId, planId, response.Operation);
        }

        private async Task LastOperationWaitAsync(string serviceId, string planId, string operation)
        {
            var result = await _inner.LastOperation(serviceId, planId, operation).GetStream().LastAsync();
            if (result.State != LastOperationResourceState.Succeeded)
                throw new BrokerException(result.Description, "AsyncFailed");
        }
    }
}
