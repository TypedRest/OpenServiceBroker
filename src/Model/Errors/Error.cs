using Newtonsoft.Json;

namespace OpenServiceBroker.Errors
{
    public class Error
    {
        [JsonProperty("error")]
        public string ErrorCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
