using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker;

/// <summary>
/// Represents the state of the last requested deferred operation.
/// </summary>
public class LastOperationResource : StatusBase, IEquatable<LastOperationResource>
{
    /// <summary>
    /// The current state.
    /// </summary>
    [JsonProperty("state", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter))]
    public LastOperationResourceState State { get; set; }

    public bool Equals(LastOperationResource other)
        => other != null
        && base.Equals(other)
        && State == other.State;

    public override bool Equals(object obj) => obj is LastOperationResource other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return base.GetHashCode() ^ ((int)State * 397);
        }
    }
}
