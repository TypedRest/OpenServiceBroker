using Newtonsoft.Json;

namespace OpenServiceBroker.Errors
{
    public abstract class StatusBase
    {
        /// <summary>
        /// A user-facing error message explaining why the request failed. If present, MUST be a non-empty string.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// If an update or deprovisioning operation failed, this flag indicates whether or not the Service Instance is still usable. If true, the Service Instance can still be used, false otherwise. This field MUST NOT be present for errors of other operations. Defaults to true.
        /// </summary>
        [JsonProperty("instance_usable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? InstanceUsable { get; set; }

        /// <summary>
        /// If an update operation failed, this flag indicates whether this update can be repeated or not. If true, the same update operation MAY be repeated and MAY succeed; if false, repeating the same update operation will fail again. This field MUST NOT be present for errors of other operations. Defaults to true.
        /// </summary>
        [JsonProperty("update_repeatable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? UpdateRepeatable { get; set; }

        protected bool Equals(StatusBase other)
            => other != null
            && Description == other.Description
            && InstanceUsable == other.InstanceUsable
            && UpdateRepeatable == other.UpdateRepeatable;

        public override bool Equals(object obj) => obj is StatusBase other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Description != null ? Description.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ InstanceUsable.GetHashCode();
                hashCode = (hashCode * 397) ^ UpdateRepeatable.GetHashCode();
                return hashCode;
            }
        }
    }
}
