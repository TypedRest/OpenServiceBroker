using System.Net;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// The specified resource does not exist (anymore).
    /// </summary>
    public class GoneException : BrokerException
    {
        public new const string ErrorCode = "Gone";

        public GoneException()
            : this("The specified resource does not exist (anymore).")
        {}

        public GoneException(string message)
            : base(message, ErrorCode, HttpStatusCode.Gone)
        {}
    }
}
