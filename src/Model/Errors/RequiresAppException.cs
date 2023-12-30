using OpenServiceBroker.Bindings;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The request body is missing the <see cref="ServiceBindingResourceObject.AppGuid"/> field.
/// </summary>
public class RequiresAppException(string message)
    : BrokerException(message, ErrorCode)
{
    public new const string ErrorCode = "RequiresApp";

    public RequiresAppException()
        : this("The request body is missing the app_guid field.")
    {}
}
