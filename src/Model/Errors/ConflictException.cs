using System.Net;

namespace OpenServiceBroker.Errors;

/// <summary>
/// An instance with the same id already exists but with different attributes.
/// </summary>
public class ConflictException(string message)
    : BrokerException(message, ErrorCode, HttpStatusCode.Conflict)
{
    public new const string ErrorCode = "Conflict";

    public ConflictException()
        : this("An instance with the same id already exists but with different attributes.")
    {}
}
