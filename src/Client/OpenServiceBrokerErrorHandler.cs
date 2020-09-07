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
            if (response.IsSuccessStatusCode) return;

            var error = await GetErrorAsync(response);
            if (error != null) throw BrokerException.FromResponse(error, response.StatusCode);

            response.EnsureSuccessStatusCode();
        }

        private static async Task<Error?> GetErrorAsync(HttpResponseMessage response)
        {
            if (response.Content == null) return null;

            try
            {
                return await response.Content.ReadAsAsync<Error>(new[] {Serializer});
            }
            catch
            {
                return null;
            }
        }
    }
}
