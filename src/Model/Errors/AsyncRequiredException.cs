namespace OpenServiceBroker.Errors;

/// <summary>
/// This request requires client support for deferred (asynchronous) service operations.
/// </summary>
public class AsyncRequiredException : BrokerException
{
    public new const string ErrorCode = "AsyncRequired";

    public AsyncRequiredException(string message = "This request requires client support for asynchronous service operations.")
        : base(message, ErrorCode)
    {}
}
