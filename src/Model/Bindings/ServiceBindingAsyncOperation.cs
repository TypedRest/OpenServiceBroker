using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingAsyncOperation : AsyncOperation, ICompletedResult<ServiceBinding>
    {
        [JsonIgnore]
        public ServiceBinding Result { get; set; }
    }
}
