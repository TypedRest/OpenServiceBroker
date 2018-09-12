using Newtonsoft.Json;

namespace OpenServiceBroker
{
    /// <summary>
    /// An operation result that can indicate that nothing was changed.
    /// </summary>
    public interface IUnchangedFlag
    {
        /// <summary>
        /// Indicates whether the request operation resulted in no changes to the existing state.
        /// </summary>
        /// <remarks>This is not part of the Open Service Broker JSON representation. Instead it is communicated out of band via HTTP status codes.</remarks>
        [JsonIgnore]
        bool Unchanged { get; set; }
    }
}
