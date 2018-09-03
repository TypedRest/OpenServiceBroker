using System.Net;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// instance with the same id already exists but with different attributes
    /// </summary>
    public class ConflictException : BrokerException
    {
        public new const string ErrorCode = "Conflict";

        public ConflictException()
            : this("An instance with the same id already exists but with different attributes.")
        {}

        public ConflictException(string message)
            : base(message, ErrorCode, HttpStatusCode.Conflict)
        {}
    }
}
