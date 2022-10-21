using OpenServiceBroker.Errors;
using TypedRest;

namespace OpenServiceBroker.Bindings;

public class ServiceBindingBlockingFacts : FactsBase<IServiceBindingBlocking>
{
    [Fact]
    public async Task Fetch()
    {
        var response = new ServiceBindingResource();

        Mock.Setup(x => x.FetchAsync("123", "456"))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesBlocking["123"].ServiceBindings["456"].FetchAsync();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Bind()
    {
        var request = new ServiceBindingRequest
        {
            ServiceId = "abc",
            PlanId = "xyz",
            BindResource = new()
            {
                AppGuid = "123-456"
            }
        };
        var response = new ServiceBinding();

        Mock.Setup(x => x.BindAsync(new("123", "456"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesBlocking["123"].ServiceBindings["456"].BindAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task BindUnchanged()
    {
        var request = new ServiceBindingRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };
        var response = new ServiceBinding();

        Mock.Setup(x => x.BindAsync(new("123", "456"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesBlocking["123"].ServiceBindings["456"].BindAsync(request);
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task BindConflict()
    {
        var request = new ServiceBindingRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };

        Mock.Setup(x => x.BindAsync(new("123", "456"), request))
            .Throws<ConflictException>();
        await Client.ServiceInstancesBlocking["123"].ServiceBindings["456"]
                    .Awaiting(x => x.BindAsync(request))
                    .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Unbind()
    {
        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .Returns(Task.CompletedTask);
        await Client.ServiceInstancesBlocking["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
    }

    [Fact]
    public async Task UnbindBody()
    {
        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .Returns(Task.CompletedTask);

        var result = await Client.HttpClient.DeleteAsync(Client.ServiceInstancesBlocking["123"].ServiceBindings["456"].Uri.Join("?service_id=abc&plan_id=xyz"));
        string resultString = await result.Content.ReadAsStringAsync();
        resultString.Should().Be("{}");
    }

    [Fact]
    public async Task UnbindGone()
    {
        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .Throws<GoneException>();
        await Client.ServiceInstancesBlocking["123"].ServiceBindings["456"]
                    .Awaiting(x => x.UnbindAsync("abc", "xyz"))
                    .Should().ThrowAsync<GoneException>();
    }
}
