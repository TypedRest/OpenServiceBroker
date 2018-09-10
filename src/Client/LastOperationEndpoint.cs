using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest;
using TypedRest.UriTemplates;

namespace OpenServiceBroker
{
    public class LastOperationEndpoint : PollingEndpoint<LastOperationResource>
    {
        private static readonly UriTemplate UriTemplate = new UriTemplate("./last_operation{?service_id,plan_id,operation}");

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
        {}

        protected override async Task HandleErrorsAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Gone)
            {
                var error = await response.Content.ReadAsAsync<Error>(new[] {Serializer});
                if (error != null) throw BrokerException.FromDto(error, response.StatusCode);
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
