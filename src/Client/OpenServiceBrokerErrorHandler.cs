using System.Net.Http.Formatting;
using OpenServiceBroker.Errors;
using TypedRest.Errors;
using TypedRest.Serializers;

namespace OpenServiceBroker;

/// <summary>
/// Handles Open Service Broker error responses.
/// </summary>
public class OpenServiceBrokerErrorHandler : IErrorHandler
{
    private static readonly MediaTypeFormatter Serializer = new NewtonsoftJsonSerializer();

    public async Task HandleAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        var error = await GetErrorAsync(response);
        if (error != null) throw BrokerException.FromResponse(error, response.StatusCode);

        response.EnsureSuccessStatusCode();
    }

    private static async Task<Error?> GetErrorAsync(HttpResponseMessage response)
    {
#if NETSTANDARD2_0
        if (response.Content == null) return null;
#endif

        try
        {
            return await response.Content.ReadAsAsync<Error>([Serializer]);
        }
        catch
        {
            return null;
        }
    }
}
