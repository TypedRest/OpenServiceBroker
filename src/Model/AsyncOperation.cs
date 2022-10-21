using Newtonsoft.Json;

namespace OpenServiceBroker;

public class AsyncOperation : ICompletable, IEquatable<AsyncOperation>
{
    [JsonProperty("operation")]
    public string Operation { get; set; }

    [JsonIgnore]
    public bool Completed { get; set; }

    public bool Equals(AsyncOperation other)
        => other != null
        && Operation == other.Operation
        && Completed == other.Completed;

    public override bool Equals(object obj) => obj is AsyncOperation other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Operation?.GetHashCode() ?? 0) * 397) ^ Completed.GetHashCode();
        }
    }
}
