using System.Threading.Tasks;
using FluentAssertions;
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

            SetupMock(x => x.FetchAsync("123", "456"), response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].FetchAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task BindDeferred()
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

            SetupMock(x => x.BindAsync("123", "456", request), response);
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
            var response = new ServiceBindingAsyncOperation
            {
                Result = new ServiceBinding()
            };

            SetupMock(x => x.BindAsync("123", "456", request), response);
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
            var response = new ServiceBindingAsyncOperation
            {
                Result = new ServiceBinding
                {
                    Unchanged = true
                }
            };

            SetupMock(x => x.BindAsync("123", "456", request), response);
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

            SetupMock(x => x.BindAsync("123", "456", request), new ConflictException());
            Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].Awaiting(x => x.BindAsync(request)).Should().Throw<ConflictException>();
        }

        [Fact]
        public async Task UnbindDeferred()
        {
            var response = new AsyncOperation
            {
                Operation = "my operation"
            };

            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task UnbindCompleted()
        {
            var response = new AsyncOperation();

            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void UnbindGone()
        {
            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), new GoneException());
            Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].Awaiting(x => x.UnbindAsync("abc", "xyz")).Should().Throw<GoneException>();
        }

        [Fact]
        public async Task GetLastOperation()
        {
            var response = new LastOperationResource
            {
                State = LastOperationResourceState.InProgress
            };

            SetupMock(x => x.GetLastOperationAsync("123", "456", "abc", "xyz", "my operation"), response);
            var result = await Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].LastOperation("abc", "xyz", "my operation").ReadAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetLastOperationGone()
        {
            SetupMock(x => x.GetLastOperationAsync("123", "456", "abc", "xyz", "my operation"), new GoneException("custom message"));
            Client.ServiceInstancesDeferred["123"].ServiceBindings["456"].LastOperation("abc", "xyz", "my operation").Awaiting(x => x.ReadAsync()).Should().Throw<GoneException>().WithMessage("custom message");
        }
    }
}
