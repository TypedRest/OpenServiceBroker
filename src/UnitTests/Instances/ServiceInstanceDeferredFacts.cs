using OpenServiceBroker.Errors;

namespace OpenServiceBroker.Instances;

public class ServiceInstanceDeferredFacts : FactsBase<IServiceInstanceDeferred>
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
        var result = await Client.ServiceInstancesDeferred["123"].FetchAsync();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ProvisionDeferred()
    {
        var request = new ServiceInstanceProvisionRequest
        {
            ServiceId = "abc",
            PlanId = "xyz",
            OrganizationGuid = "org",
            SpaceGuid = "space"
        };
        var response = new ServiceInstanceAsyncOperation
        {
            Operation = "my operation",
            DashboardUrl = new Uri("http://example.com")
        };

        Mock.Setup(x => x.ProvisionAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].ProvisionAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ProvisionCompleted()
    {
        var request = new ServiceInstanceProvisionRequest
        {
            ServiceId = "abc",
            PlanId = "xyz",
            OrganizationGuid = "org",
            SpaceGuid = "space"
        };
        var response = new ServiceInstanceAsyncOperation().Complete(new ServiceInstanceProvision
        {
            DashboardUrl = new Uri("http://example.com")
        });

        Mock.Setup(x => x.ProvisionAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].ProvisionAsync(request);
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
        var response = new ServiceInstanceAsyncOperation().Complete(new ServiceInstanceProvision
        {
            DashboardUrl = new Uri("http://example.com"),
            Unchanged = true
        });

        Mock.Setup(x => x.ProvisionAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].ProvisionAsync(request);
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
        await Client.ServiceInstancesDeferred["123"]
                    .Awaiting(x => x.ProvisionAsync(request))
                    .Should().ThrowAsync<ConflictException>().WithMessage("custom message");
    }

    [Fact]
    public async Task UpdateDeferred()
    {
        var request = new ServiceInstanceUpdateRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };
        var response = new ServiceInstanceAsyncOperation
        {
            Operation = "my operation",
            DashboardUrl = new Uri("http://example.com")
        };

        Mock.Setup(x => x.UpdateAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].UpdateAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task UpdateCompleted()
    {
        var request = new ServiceInstanceUpdateRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };
        var response = new ServiceInstanceAsyncOperation();

        Mock.Setup(x => x.UpdateAsync(new("123"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].UpdateAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task DeprovisionDeferred()
    {
        var response = new AsyncOperation
        {
            Operation = "my operation"
        };

        Mock.Setup(x => x.DeprovisionAsync(new("123"), "abc", "xyz"))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].DeprovisionAsync("abc", "xyz");
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task DeprovisionCompleted()
    {
        var response = new AsyncOperation();

        Mock.Setup(x => x.DeprovisionAsync(new("123"), "abc", "xyz"))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].DeprovisionAsync("abc", "xyz");
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task DeprovisionGone()
    {
        Mock.Setup((x => x.DeprovisionAsync(new("123"), "abc", "xyz")))
            .Throws<GoneException>();
        await Client.ServiceInstancesDeferred["123"]
                    .Awaiting(x => x.DeprovisionAsync("abc", "xyz"))
                    .Should().ThrowAsync<GoneException>();
    }

    [Fact]
    public async Task GetLastOperation()
    {
        var response = new LastOperationResource
        {
            State = LastOperationResourceState.InProgress
        };

        Mock.Setup(x => x.GetLastOperationAsync(new("123"), "abc", "xyz", "my operation"))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesDeferred["123"].LastOperation("abc", "xyz", "my operation").ReadAsync();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetLastOperationGone()
    {
        Mock.Setup((x => x.GetLastOperationAsync(new("123"), "abc", "xyz", "my operation")))
            .Throws(new GoneException("custom message"));
        await Client.ServiceInstancesDeferred["123"].LastOperation("abc", "xyz", "my operation")
                    .Awaiting(x => x.ReadAsync())
                    .Should().ThrowAsync<GoneException>().WithMessage("custom message");
    }
}
