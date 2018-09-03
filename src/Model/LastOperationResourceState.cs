using System.Runtime.Serialization;

namespace OpenServiceBroker
{
    public enum LastOperationResourceState
    {
        [EnumMember(Value = "in progress")]
        InProgress,

        [EnumMember(Value = "succeeded")]
        Succeeded,

        [EnumMember(Value = "failed")]
        Failed
    }
}
