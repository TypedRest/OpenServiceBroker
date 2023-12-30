using System.Net;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The API version request by the client is not supported by server.
/// </summary>
public class ApiVersionNotSupportedException(string message)
    : BrokerException(message, ErrorCode, HttpStatusCode.PreconditionFailed)
{
    public new const string ErrorCode = "ApiVersionNotSupported";
}
