using System.Net;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The The request is malformed or missing mandatory data..
/// </summary>
public class BadRequestException : BrokerException
{
    public new const string ErrorCode = "BadRequest";

    public BadRequestException(string message)
        : base(message, ErrorCode, HttpStatusCode.BadRequest)
    {}
}
