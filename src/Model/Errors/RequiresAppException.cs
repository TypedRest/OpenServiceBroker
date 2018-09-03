namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// request body is missing the app_guid field
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
}
