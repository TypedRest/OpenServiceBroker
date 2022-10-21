using OpenServiceBroker.Bindings;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The request body is missing the <see cref="ServiceBindingResourceObject.AppGuid"/> field.
/// </summary>
public class RequiresAppException : BrokerException
{
    public new const string ErrorCode = "RequiresApp";

    public RequiresAppException()
        : this("The request body is missing the app_guid field.")
    {}

    public RequiresAppException(string message)
        : base(message, ErrorCode)
    {}
}
