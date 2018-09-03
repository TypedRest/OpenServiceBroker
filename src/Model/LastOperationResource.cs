using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenServiceBroker
{
    public class LastOperationResource : IEquatable<LastOperationResource>
    {
        [JsonProperty("state", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public LastOperationResourceState State { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public bool Equals(LastOperationResource other)
        {
            if (other == null) return false;
            return State == other.State && string.Equals(Description, other.Description);
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
