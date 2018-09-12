using Newtonsoft.Json;

namespace OpenServiceBroker.Errors
{
    /// <summary>
    /// An error response object.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// A single word in camel case that uniquely identifies the error condition. If present, MUST be a non-empty string.
        /// </summary>
        [JsonProperty("error")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// A user-facing error message explaining why the request failed. If present, MUST be a non-empty string.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
