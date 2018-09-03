using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace OpenServiceBroker.Catalogs
{
    public class Service : IEquatable<Service>
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; } = new List<string>();

        [JsonProperty("requires", ItemConverterType = typeof(StringEnumConverter))]
        public List<Features> Requires { get; } = new List<Features>();

        [JsonProperty("bindable", Required = Required.Always)]
        public bool Bindable { get; set; }

        [JsonProperty("metadata")]
        public JObject Metadata { get; set; }

        [JsonProperty("dashboard_client")]
        public DashboardClient DashboardClient { get; set; }

        [JsonProperty("plan_updateable")]
        public bool? PlanUpdateable { get; set; }

        [JsonProperty("plans", Required = Required.Always)]
        public IList<Plan> Plans { get; } = new List<Plan>();

        public bool Equals(Service other)
        {
            if (other == null) return false;
            return string.Equals(Id, other.Id) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && Bindable == other.Bindable && Equals(DashboardClient, other.DashboardClient) && PlanUpdateable == other.PlanUpdateable;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == GetType() && Equals((Service)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Bindable.GetHashCode();
                hashCode = (hashCode * 397) ^ (DashboardClient != null ? DashboardClient.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ PlanUpdateable.GetHashCode();
                return hashCode;
            }
        }
    }
}
