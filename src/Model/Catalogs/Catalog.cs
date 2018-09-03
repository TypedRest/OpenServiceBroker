using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenServiceBroker.Catalogs
{
    public class Catalog
    {
        [JsonProperty("services", Required = Required.Always)]
        public List<Service> Services { get; } = new List<Service>();
    }
}
