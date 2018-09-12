using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenServiceBroker
{
    /// <summary>
    /// Represents the state of the last requested deferred operation.
    /// </summary>
    public class LastOperationResource : IEquatable<LastOperationResource>
    {
        /// <summary>
        /// The current state.
        /// </summary>
        [JsonProperty("state", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public LastOperationResourceState State { get; set; }

        /// <summary>
        /// A user-facing message that can be used to tell the user details about the status of the operation. If present, MUST be a non-empty string.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        public bool Equals(LastOperationResource other)
        {
            if (other == null) return false;
            return State == other.State && Description == other.Description;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((LastOperationResource)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)State * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }
    }
}
