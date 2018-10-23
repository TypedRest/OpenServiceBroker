using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;
using TypedRest.UriTemplates;

namespace OpenServiceBroker
{
    /// <summary>
    /// An endpoint to obtain the state of a deferred (asynchronous) operation.
    /// </summary>
    public class LastOperationEndpoint : PollingEndpoint<LastOperationResource>
    {
        private static readonly UriTemplate UriTemplate = new UriTemplate("./last_operation{?service_id,plan_id,operation}");

        /// <summary>
        /// Creates a new last operation endpoint.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="serviceId">If present, it MUST be the ID of the service being used.</param>
        /// <param name="planId">If present, it MUST be the ID of the plan for the Service Instance. If this endpoint is being polled as a result of changing the plan through a Service Instance Update, the ID of the plan prior to the update MUST be used.</param>
        /// <param name="operation">A Service Broker-provided identifier for the operation. When a value for operation is included with asynchronous responses for Provision, Update, and Deprovision requests, the Platform MUST provide the same value using this query parameter. If present, MUST be a non-empty string.</param>
        public LastOperationEndpoint(IEndpoint referrer, string serviceId = null, string planId = null, string operation = null)
            : base(
                referrer,
                UriTemplate.Resolve(new
                {
                    service_id = serviceId,
                    plan_id = planId,
                    operation
                }),
                endCondition: obj => obj?.State != LastOperationResourceState.InProgress)
        {
            PollingInterval = TimeSpan.FromSeconds(10);
        }
    }
}
