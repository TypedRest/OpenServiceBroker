using System.Runtime.Serialization;

namespace OpenServiceBroker.Catalogs
{
    public enum Features
    {
        [EnumMember(Value = "syslog_drain")]
        SyslogDrain,

        [EnumMember(Value = "route_forwarding")]
        RouteForwarding,

        [EnumMember(Value = "volume_mount")]
        VolumeMount
    }
}
