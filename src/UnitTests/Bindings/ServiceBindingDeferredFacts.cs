using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OpenServiceBroker.Errors;
using Xunit;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingDeferredFacts : FactsBase<IServiceBindingDeferred>
    {
        [Fact]
        public async Task Fetch()
        {
            var response = new ServiceBindingResource();

            Mock.Setup(x => x.FetchAsync("123", "456"))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].FetchAsync();
            result.Should().BeEquivalentTo(response);
        }

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

            Mock.Setup(x => x.BindAsync(new("123", "456"), request))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].BindAsync(request);
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task BindCompleted()
        {
            var request = new ServiceBindingRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var response = new ServiceBindingAsyncOperation().Complete(new ServiceBinding());

            Mock.Setup(x => x.BindAsync(new("123", "456"), request))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].BindAsync(request);
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
            var response = new ServiceBindingAsyncOperation().Complete(new ServiceBinding
            {
                Unchanged = true
            });

            Mock.Setup(x => x.BindAsync(new("123", "456"), request))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].BindAsync(request);
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void BindConflict()
        {
            var request = new ServiceBindingRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };

            Mock.Setup(x => x.BindAsync(new("123", "456"), request))
                .Throws<ConflictException>();
            Client.ServiceInstancesDeferred["123"].ServiceBindings["456"]
                  .Awaiting(x => x.BindAsync(request))
                  .Should().Throw<ConflictException>();
        }

        [Fact]
        public async Task UnbindDeferred()
        {
            var response = new AsyncOperation
            {
                Operation = "my operation"
            };

            Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task UnbindCompleted()
        {
            var response = new AsyncOperation();

            Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void UnbindGone()
        {
            Mock.Setup(x => x.UnbindAsync(new("123", "456"), "abc", "xyz"))
                .Throws<GoneException>();
            Client.ServiceInstancesDeferred["123"].ServiceBindings["456"]
                  .Awaiting(x => x.UnbindAsync("abc", "xyz"))
                  .Should().Throw<GoneException>();
        }

        [Fact]
        public async Task GetLastOperation()
        {
            var response = new LastOperationResource
            {
                State = LastOperationResourceState.InProgress
            };

            Mock.Setup(x => x.GetLastOperationAsync(new("123", "456"), "abc", "xyz", "my operation"))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].LastOperation("abc", "xyz", "my operation").ReadAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetLastOperationGone()
        {
            Mock.Setup(x => x.GetLastOperationAsync(new("123", "456"), "abc", "xyz", "my operation"))
                .Throws(new GoneException("custom message"));
            Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].LastOperation("abc", "xyz", "my operation")
                  .Awaiting(x => x.ReadAsync())
                  .Should().Throw<GoneException>().WithMessage("custom message");
        }
    }
}
