using System.Net;

namespace OpenServiceBroker.Errors;

/// <summary>
/// The specified resource does not exist (anymore).
/// </summary>
public class GoneException(string message)
    : BrokerException(message, ErrorCode, HttpStatusCode.Gone)
{
    public new const string ErrorCode = "Gone";

    public GoneException()
        : this("The specified resource does not exist (anymore).")
    {}
}
