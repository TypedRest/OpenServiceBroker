using System.Net;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// The resource does not exist or a provisioning operation is still in progress.
    /// </summary>
    public class NotFoundException : BrokerException
    {
        public new const string ErrorCode = "NotFound";

        public NotFoundException()
            : this("The resource does not exist or a provisioning operation is still in progress.")
        {}

        public NotFoundException(string message)
            : base(message, ErrorCode, HttpStatusCode.NotFound)
        {}
    }
}
