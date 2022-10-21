namespace OpenServiceBroker.Errors;

/// <summary>
/// The Service Broker does not support concurrent requests that mutate the same resource.
/// </summary>
public class ConcurrencyException : BrokerException
{
    public new const string ErrorCode = "ConcurrencyError";

    public ConcurrencyException(string message = "The Service Broker does not support concurrent requests that mutate the same resource.")
        : base(message, ErrorCode)
    {}
}
