using System;
using Newtonsoft.Json;

namespace OpenServiceBroker
{
    public class AsyncOperation : IEquatable<AsyncOperation>
    {
        [JsonProperty("operation", Required = Required.Always)]
        public string Operation { get; set; }

        public bool Equals(AsyncOperation other)
            => other != null && string.Equals(Operation, other.Operation);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((AsyncOperation)obj);
        }

        public override int GetHashCode() => (Operation != null ? Operation.GetHashCode() : 0);
    }
}
