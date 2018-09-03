using System;
using Newtonsoft.Json;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBinding : ServiceBindingBase, IUnchangedFlag, IEquatable<ServiceBinding>
    {
        [JsonIgnore]
        public bool Unchanged { get; set; }

        public bool Equals(ServiceBinding other)
        {
            if (other == null) return false;
            return base.Equals(other) && Unchanged == other.Unchanged;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((ServiceBinding)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Unchanged.GetHashCode();
            }
        }
    }
}
