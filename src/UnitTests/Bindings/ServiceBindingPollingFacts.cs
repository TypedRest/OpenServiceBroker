using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OpenServiceBroker.Errors;
using OpenServiceBroker.Instances;
using Xunit;

namespace OpenServiceBroker.Bindings;

public class ServiceBindingPollingFacts : FactsBase<IServiceBindingDeferred>
{
    [Fact]
    public async Task BindDeferred()
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
        var response = new ServiceBindingAsyncOperation
        {
            Operation = "my operation"
        };
        var operation = new LastOperationResource
        {
            State = LastOperationResourceState.Succeeded,
            Description = "done"
        };
        var resource = new ServiceBindingResource
        {
            RouteServiceUrl = new Uri("http://example.com")
        };
        var syntheticResponse = new ServiceBinding
        {
            RouteServiceUrl = new Uri("http://example.com")
        };

        Mock.Setup(x => x.BindAsync(new("123", "456"), request))
            .ReturnsAsync(response);
        Mock.Setup(x => x.GetLastOperationAsync(new("123", "456"), "abc", "xyz", "my operation"))
            .ReturnsAsync(operation);
        Mock.Setup(x => x.FetchAsync("123", "456"))
            .ReturnsAsync(resource);
        var result = await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].BindAsync(request);
        result.Should().BeEquivalentTo(syntheticResponse);
    }

    [Fact]
    public async Task BindCompleted()
    {
        var request = new ServiceBindingRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };
        var syntheticResponse = new ServiceBinding
        {
            RouteServiceUrl = new Uri("http://example.com")
        };
        var response = new ServiceBindingAsyncOperation().Complete(syntheticResponse);

        Mock.Setup(x => x.BindAsync(new("123", "456"), request))
            .ReturnsAsync(response);
        var result = await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].BindAsync(request);
        result.Should().BeEquivalentTo(syntheticResponse);
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
        await Client.ServiceInstancesPolling["123"].ServiceBindings["456"]
                    .Awaiting(x => x.BindAsync(request))
                    .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task BindAsyncError()
    {
        var request = new ServiceBindingRequest
        {
            ServiceId = "abc",
            PlanId = "xyz"
        };
        var response = new ServiceBindingAsyncOperation
        {
            Operation = "my operation"
        };
        var operation = new LastOperationResource
        {
            State = LastOperationResourceState.Failed,
            Description = "custom message"
        };

        Mock.Setup(x => x.BindAsync(new("123", "456"), request))
            .ReturnsAsync(response);
        Mock.Setup(x => x.GetLastOperationAsync(new("123", "456"), "abc", "xyz", "my operation"))
            .ReturnsAsync(operation);
        await Client.ServiceInstancesPolling["123"].ServiceBindings["456"]
                    .Awaiting(x => x.BindAsync(request))
                    .Should().ThrowAsync<BrokerException>().WithMessage("custom message");
    }

    [Fact]
    public async Task UnbindDeferred()
    {
        var response = new AsyncOperation
        {
            Operation = "my operation"
        };
        var operation = new LastOperationResource
        {
            State = LastOperationResourceState.Succeeded,
            Description = "done"
        };

        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .ReturnsAsync(response);
        Mock.Setup(x => x.GetLastOperationAsync(new("123", "456"), "abc", "xyz", "my operation"))
            .ReturnsAsync(operation);
        await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
    }

    [Fact]
    public async Task UnbindCompleted()
    {
        var response = new AsyncOperation();

        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .ReturnsAsync(response);
        await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
    }

    [Fact]
    public async Task UnbindGone()
    {
        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .Throws<GoneException>();
        await Client.ServiceInstancesPolling["123"].ServiceBindings["456"]
                    .Awaiting(x => x.UnbindAsync("abc", "xyz"))
                    .Should().ThrowAsync<GoneException>();
    }

    [Fact]
    public async Task UnbindAsyncGone()
    {
        var response = new ServiceInstanceAsyncOperation
        {
            Operation = "my operation"
        };

        Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
            .ReturnsAsync(response);
        Mock.Setup(x => x.GetLastOperationAsync(new("123", "456"), "abc", "xyz", "my operation"))
            .Throws<GoneException>();
        await Client.ServiceInstancesPolling["123"].ServiceBindings["456"]
                    .Awaiting(x => x.UnbindAsync("abc", "xyz"))
                    .Should().ThrowAsync<GoneException>();
    }
}
