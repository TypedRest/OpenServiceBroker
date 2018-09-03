using Newtonsoft.Json;

namespace OpenServiceBroker
{
    public interface ICompletedResult<T>
    {
        /// <summary>
        /// TODO
        /// </summary>
        [JsonIgnore]
        T Result { get; set; }
    }
}
