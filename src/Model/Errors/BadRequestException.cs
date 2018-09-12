using System.Net;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// request was rejected by server due to semantic errors
    /// </summary>
    public class BadRequestException : BrokerException
    {
        public new const string ErrorCode = "BadRequest";

        public BadRequestException(string message)
            : base(message, ErrorCode, HttpStatusCode.BadRequest)
        {}
    }
}
