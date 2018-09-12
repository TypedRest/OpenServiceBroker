using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using OpenServiceBroker.Bindings;
using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Instances
{
    /// <summary>
    /// Represents a specific Service Instance. Uses potentially deferred (asynchronous) operations and automatically handles polling to make them appear blocking.
    /// </summary>
    public class ServiceInstancePollingEndpoint : ServiceInstanceEndpointBase<IServiceBindingEndpoint, ServiceBindingPollingEndpoint>, IServiceInstanceEndpoint
    {
        private readonly IServiceInstanceDeferredEndpoint _inner;

        /// <summary>
        /// Creates a new polling Service Instance endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        public ServiceInstancePollingEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri)
        {
            _inner = new ServiceInstanceDeferredEndpoint(referrer, relativeUri);
        }

        public async Task<ServiceInstanceProvision> ProvisionAsync(ServiceInstanceProvisionRequest request)
        {
            var response = await _inner.ProvisionAsync(request);

            if (string.IsNullOrEmpty(response.Operation))
                return response.Result;

            await LastOperationWaitAsync(request.ServiceId, request.PlanId, response.Operation);
            return new ServiceInstanceProvision {DashboardUrl = response.DashboardUrl};
        }

        public async Task UpdateAsync(ServiceInstanceUpdateRequest request)
        {
            var response = await _inner.UpdateAsync(request);

            if (string.IsNullOrEmpty(response.Operation))
                return;

            await LastOperationWaitAsync(request.ServiceId, request.PlanId, response.Operation);
        }

        public async Task DeprovisionAsync(string serviceId, string planId)
        {
            var response = await _inner.DeprovisionAsync(serviceId, planId);

            if (string.IsNullOrEmpty(response.Operation))
                return;

            await LastOperationWaitAsync(serviceId, planId, response.Operation);
        }

        private async Task LastOperationWaitAsync(string serviceId, string planId, string operation)
        {
            var result = await _inner.LastOperation(serviceId, planId, operation).GetStream().LastAsync();
            if (result?.State != LastOperationResourceState.Succeeded)
                throw new BrokerException(result?.Description ?? "Asynchronous operation failed.", "AsyncFailed");
        }
    }
}
