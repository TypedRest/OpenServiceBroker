using System;
using Newtonsoft.Json;

namespace OpenServiceBroker
{
    public class AsyncOperation : ICompletable, IEquatable<AsyncOperation>
    {
        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonIgnore]
        public bool Completed { get; set; }

        public bool Equals(AsyncOperation other)
        {
            if (other == null) return false;
            return Operation == other.Operation && Completed == other.Completed;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((AsyncOperation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Operation != null ? Operation.GetHashCode() : 0) * 397) ^ Completed.GetHashCode();
            }
        }
    }
}
