using System.Net;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The API version request by the client is not supported by server.
/// </summary>
public class ApiVersionNotSupportedException : BrokerException
{
    public new const string ErrorCode = "ApiVersionNotSupported";

    public ApiVersionNotSupportedException(string message)
        : base(message, ErrorCode, HttpStatusCode.PreconditionFailed)
    {}
}
