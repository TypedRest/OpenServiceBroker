using Newtonsoft.Json;

namespace OpenServiceBroker
{
    /// <summary>
    /// An async/deferred operation that can also be completed and provide a result synchronously.
    /// </summary>
    public interface ICompletableWithResult<T> : ICompletable
        where T : class
    {
        /// <summary>
        /// If <see cref="ICompletable.Completed"/> is <c>true</c> this holds the result of the operation.
        /// </summary>
        /// <remarks>This is not part of the Open Service Broker JSON representation. Instead it is communicated out of band via HTTP status codes.</remarks>
        [JsonIgnore]
        T Result { get; set; }
    }

    public static class CompletableWithResult
    {
        /// <summary>
        /// Marks an operation as already completed synchronously.
        /// </summary>
        /// <param name="completable">The operation to mark.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns><paramref name="completable"/> for fluent-style use.</returns>
        public static TCompletable Complete<TCompletable, TResult>(this TCompletable completable, TResult result)
            where TCompletable : ICompletableWithResult<TResult>
            where TResult : class
        {
            completable.Result = result;
            return completable.Complete();
        }
    }
}
