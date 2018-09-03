using System.Runtime.Serialization;

namespace OpenServiceBroker.Bindings
{
    public enum ServiceBindingVolumeMountMode
    {
        [EnumMember(Value = "r")]
        Read,

        [EnumMember(Value = "rw")]
        ReadWrite
    }
}
