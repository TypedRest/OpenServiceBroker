using Newtonsoft.Json;

namespace OpenServiceBroker
{
    public interface IUnchangedFlag
    {
        /// <summary>
        /// TODO
        /// </summary>
        [JsonIgnore]
        bool Unchanged { get; set; }
    }
}
