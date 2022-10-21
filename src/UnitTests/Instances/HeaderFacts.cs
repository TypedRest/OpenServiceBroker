using Newtonsoft.Json.Linq;
using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

public class HeaderFacts : FactsBase<IServiceInstanceBlocking>
{
    [Fact]
    public async Task WrongMajorVersion()
    {
        Client.SetApiVersion(new(1, 0));
        await Client.ServiceInstancesBlocking["123"]
                    .Awaiting(x => x.FetchAsync())
                    .Should().ThrowAsync<ApiVersionNotSupportedException>();
    }

    [Fact]
    public async Task TooNewMinorVersion()
    {
        Client.SetApiVersion(new(ServiceInstancesController.SupportedApiVersion.Major, ServiceInstancesController.SupportedApiVersion.Minor + 1));
        await Client.ServiceInstancesBlocking["123"]
                    .Awaiting(x => x.FetchAsync())
                    .Should().ThrowAsync<ApiVersionNotSupportedException>();
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

        ServiceInstanceProvision result = new();
        Mock.Setup(x => x.ProvisionAsync(new("123", identity), request))
            .ReturnsAsync(result);
        Client.SetOriginatingIdentity(identity);
        await Client.ServiceInstancesBlocking["123"].ProvisionAsync(request);
    }
}
