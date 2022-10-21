using Newtonsoft.Json;

namespace OpenServiceBroker;

/// <summary>
/// An async/deferred operation that can also be completed synchronously.
/// </summary>
public interface ICompletable
{
    /// <summary>
    /// Indicates whether this operation has already been completed synchronously and therefore requires no polling.
    /// </summary>
    /// <remarks>This is not part of the Open Service Broker JSON representation. Instead it is communicated out of band via HTTP status codes.</remarks>
    [JsonIgnore]
    bool Completed { get; set; }
}

public static class Completable
{
    /// <summary>
    /// Marks an operation as already completed synchronously.
    /// </summary>
    /// <param name="completable">The operation to mark.</param>
    /// <returns><paramref name="completable"/> for fluent-style use.</returns>
    public static TCompletable Complete<TCompletable>(this TCompletable completable)
        where TCompletable : ICompletable
    {
        completable.Completed = true;
        return completable;
    }
}
