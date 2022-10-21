using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings;

public class ServiceBinding : ServiceBindingBase, IUnchangedFlag, IEquatable<ServiceBinding>
{
    [JsonIgnore]
    public bool Unchanged { get; set; }

    public bool Equals(ServiceBinding other)
        => other != null
        && base.Equals(other)
        && Unchanged == other.Unchanged;

    public override bool Equals(object obj) => obj is ServiceBinding other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ Unchanged.GetHashCode();
        }
    }
}
