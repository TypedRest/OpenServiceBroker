using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using OpenServiceBroker.Errors;
using TypedRest.Errors;
using TypedRest.Serializers;

namespace OpenServiceBroker
{
    /// <summary>
    /// Handles Open Service Broker error responses.
    /// </summary>
    public class OpenServiceBrokerErrorHandler : IErrorHandler
    {
        private static readonly MediaTypeFormatter Serializer = new DefaultJsonSerializer();

        public async Task HandleAsync(HttpResponseMessage response)
        {
            if (response.Content != null)
            {
                Error error = null;
                try
                {
                    error = await response.Content.ReadAsAsync<Error>(new[] {Serializer});
                }
                catch
                {
                    // Error responses without a response in standard format will be handled below
                }
                if (error != null) throw BrokerException.FromResponse(error, response.StatusCode);
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
