namespace OpenServiceBroker.Errors;

/// <summary>
/// The Service Broker does not support concurrent requests that mutate the same resource.
/// </summary>
public class ConcurrencyException(string message = "The Service Broker does not support concurrent requests that mutate the same resource.")
    : BrokerException(message, ErrorCode)
{
    public new const string ErrorCode = "ConcurrencyError";
}
