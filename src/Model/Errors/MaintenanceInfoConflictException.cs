namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// The <see cref="MaintenanceInfo.Version"/> field provided in the request does not match the<see cref="MaintenanceInfo.Version"/> field provided in the Service Broker's Catalog.
    /// </summary>
    public class MaintenanceInfoConflictException : BrokerException
    {
        public new const string ErrorCode = "MaintenanceInfoConflict";

        public MaintenanceInfoConflictException()
            : this("The maintenance_info.version field provided in the request does not match the maintenance_info.version field provided in the Service Broker's Catalog.")
        {}

        public MaintenanceInfoConflictException(string message)
            : base(message, ErrorCode)
        {}
    }
}
