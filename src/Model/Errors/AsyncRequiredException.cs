namespace OpenServiceBroker.Errors;

/// <summary>
/// This request requires client support for deferred (asynchronous) service operations.
/// </summary>
public class AsyncRequiredException(string message = "This request requires client support for asynchronous service operations.")
    : BrokerException(message, ErrorCode)
{
    public new const string ErrorCode = "AsyncRequired";
}
