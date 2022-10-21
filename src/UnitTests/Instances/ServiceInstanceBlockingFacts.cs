using System.Net;
using OpenServiceBroker.Errors;
using TypedRest;
using TypedRest.Http;

namespace OpenServiceBroker.Instances;

public class ServiceInstanceBlockingFacts : FactsBase<IServiceInstanceBlocking>
{
    [Fact]
    public async Task Fetch()
    {
        var response = new ServiceInstanceResource
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };

        Mock.Setup(x => x.FetchAsync("123"))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesBlocking["123"].FetchAsync();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Provision()
    {
        var request = new ServiceInstanceProvisionRequest
        {
            ServiceId = "abc",
            PlanId = "xyz",
            OrganizationGuid = "org",
            SpaceGuid = "space"
        };
        var response = new ServiceInstanceProvision
        {
            DashboardUrl = new Uri("http://example.com")
        };

        Mock.Setup(x => x.ProvisionAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesBlocking["123"].ProvisionAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ProvisionUnchanged()
    {
        var request = new ServiceInstanceProvisionRequest
        {
            ServiceId = "abc",
            PlanId = "xyz",
            OrganizationGuid = "org",
            SpaceGuid = "space"
        };
        var response = new ServiceInstanceProvision
        {
            DashboardUrl = new Uri("http://example.com"),
            Unchanged = true
        };

        Mock.Setup(x => x.ProvisionAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesBlocking["123"].ProvisionAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ProvisionConflict()
    {
        var request = new ServiceInstanceProvisionRequest
        {
            ServiceId = "abc",
            PlanId = "xyz",
            OrganizationGuid = "org",
            SpaceGuid = "space"
        };

        Mock.Setup((x => x.ProvisionAsync(new("123"), request)))
            .Throws(new ConflictException("custom message"));
        await Client.ServiceInstancesBlocking["123"]
                    .Awaiting(x => x.ProvisionAsync(request))
                    .Should().ThrowAsync<ConflictException>().WithMessage("custom message");
    }

    [Fact]
    public async Task Update()
    {
        var request = new ServiceInstanceUpdateRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };

        Mock.Setup(x => x.UpdateAsync(new("123"), request))
            .Returns(Task.CompletedTask);
        await Client.ServiceInstancesBlocking["123"].UpdateAsync(request);
    }

    [Fact]
    public async Task UpdateBody()
    {
        var request = new ServiceInstanceUpdateRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };

        Mock.Setup(x => x.UpdateAsync(new("123"), request))
            .Returns(Task.CompletedTask);

        var result = await Client.HttpClient.PatchAsync(Client.ServiceInstancesBlocking["123"].Uri, request, Client.Serializer);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        string resultString = await result.Content.ReadAsStringAsync();
        resultString.Should().Be("{}");
    }

    [Fact]
    public async Task Deprovision()
    {
        Mock.Setup(x => x.DeprovisionAsync(new("123"), "abc", "xyz"))
            .Returns(Task.CompletedTask);
        await Client.ServiceInstancesBlocking["123"].DeprovisionAsync("abc", "xyz");
    }

    [Fact]
    public async Task DeprovisionBody()
    {
        Mock.Setup(x => x.DeprovisionAsync(new("123"), "abc", "xyz"))
            .Returns(Task.CompletedTask);

        var result = await Client.HttpClient.DeleteAsync(Client.ServiceInstancesBlocking["123"].Uri.Join("?service_id=abc&plan_id=xyz"));
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        string resultString = await result.Content.ReadAsStringAsync();
        resultString.Should().Be("{}");
    }

    [Fact]
    public async Task DeprovisionGone()
    {
        Mock.Setup((x => x.DeprovisionAsync(new("123"), "abc", "xyz")))
            .Throws<GoneException>();
        await Client.ServiceInstancesBlocking["123"]
                    .Awaiting(x => x.DeprovisionAsync("abc", "xyz"))
                    .Should().ThrowAsync<GoneException>();
    }
}
