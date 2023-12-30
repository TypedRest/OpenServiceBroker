using System.Net;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The The request is malformed or missing mandatory data..
/// </summary>
public class BadRequestException(string message)
    : BrokerException(message, ErrorCode, HttpStatusCode.BadRequest)
{
    public new const string ErrorCode = "BadRequest";
}
