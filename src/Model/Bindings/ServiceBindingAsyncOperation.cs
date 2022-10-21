using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings;

public class ServiceBindingAsyncOperation : AsyncOperation, ICompletableWithResult<ServiceBinding>
{
    [JsonIgnore]
    public ServiceBinding Result { get; set; }
}
