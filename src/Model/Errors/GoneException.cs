using System.Net;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// specified resource no longer exists
    /// </summary>
    public class GoneException : BrokerException
    {
        public new const string ErrorCode = "Gone";

        public GoneException()
            : this("The specified resource no longer exists.")
        {}

        public GoneException(string message)
            : base(message, ErrorCode, HttpStatusCode.Gone)
        {}
    }
}
