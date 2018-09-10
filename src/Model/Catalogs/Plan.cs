using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Catalogs
{
    public class Plan : IEquatable<Plan>
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty("metadata")]
        public JObject Metadata { get; set; }

        [JsonProperty("free")]
        [DefaultValue(true)]
        public bool Free { get; set; } = true;

        [JsonProperty("bindable")]
        public bool Bindable { get; set; }

        [JsonProperty("schemas")]
        public Schemas Schemas { get; set; }

        public bool Equals(Plan other)
        {
            if (other == null) return false;
            return Id == other.Id && Name == other.Name && Description == other.Description && Free == other.Free && Bindable == other.Bindable;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((Plan)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id != null ? Id.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Free.GetHashCode();
                hashCode = (hashCode * 397) ^ Bindable.GetHashCode();
                return hashCode;
            }
        }
    }
}
