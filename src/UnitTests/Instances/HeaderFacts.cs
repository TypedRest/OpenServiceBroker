using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using OpenServiceBroker.Errors;
using Xunit;

namespace OpenServiceBroker.Instances
{
    public class HeaderFacts : FactsBase<IServiceInstanceBlocking>
    {
        [Fact]
        public void WrongMajorVersion()
        {
            Client.SetApiVersion(new ApiVersion(1, 0));
            Client.ServiceInstancesBlocking["123"]
                  .Awaiting(x => x.FetchAsync())
                  .Should().Throw<ApiVersionNotSupportedException>();
        }

        [Fact]
        public void TooNewMinorVersion()
        {
            Client.SetApiVersion(new ApiVersion(ServiceInstancesController.SupportedApiVersion.Major, ServiceInstancesController.SupportedApiVersion.Minor + 1));
            Client.ServiceInstancesBlocking["123"]
                  .Awaiting(x => x.FetchAsync())
                  .Should().Throw<ApiVersionNotSupportedException>();
        }

        [Fact]
        public async Task OriginatingIdentity()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz",
                OrganizationGuid = "org",
                SpaceGuid = "space"
            };
            var identity = new OriginatingIdentity("myplatform", new JObject {{"id", "test"}});

            ServiceInstanceProvision result = new ServiceInstanceProvision();
            Mock.Setup(x => x.ProvisionAsync(new ServiceInstanceContext("123", identity), request))
                .ReturnsAsync(result);
            Client.SetOriginatingIdentity(identity);
            await Client.ServiceInstancesBlocking["123"].ProvisionAsync(request);
        }
    }
}
