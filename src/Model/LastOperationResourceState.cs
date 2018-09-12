using System.Runtime.Serialization;

namespace OpenServiceBroker
{
    public enum LastOperationResourceState
    {
        /// <summary>
        /// The operation is still in progress. The Platform SHOULD continue polling.
        /// </summary>
        [EnumMember(Value = "in progress")]
        InProgress,

        /// <summary>
        /// The operation succeeded. The Platform MUST cease polling.
        /// </summary>
        [EnumMember(Value = "succeeded")]
        Succeeded,

        /// <summary>
        /// The operation failed. The Platform MUST cease polling.
        /// </summary>
        [EnumMember(Value = "failed")]
        Failed
    }
}
