using System;
using System.Threading.Tasks;
using FluentAssertions;
using OpenServiceBroker.Errors;
using OpenServiceBroker.Instances;
using Xunit;

namespace OpenServiceBroker.Bindings
{
    public class ServiceBindingPollingFacts : FactsBase<IServiceBindingDeferred>
    {
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

            SetupMock(x => x.BindAsync("123", "456", request), response);
            SetupMock(x => x.GetLastOperationAsync("123", "456", "abc", "xyz", "my operation"), operation);
            SetupMock(x => x.FetchAsync("123", "456"), resource);
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
            var response = new ServiceBindingAsyncOperation
            {
                Result = syntheticResponse
            };

            SetupMock(x => x.BindAsync("123", "456", request), response);
            var result = await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].BindAsync(request);
            result.Should().BeEquivalentTo(syntheticResponse);
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
            Client.ServiceInstancesPolling["123"].ServiceBindings["456"].Awaiting(x => x.BindAsync(request)).Should().Throw<ConflictException>();
        }

        [Fact]
        public void BindAsyncError()
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

            SetupMock(x => x.BindAsync("123", "456", request), response);
            SetupMock(x => x.GetLastOperationAsync("123", "456", "abc", "xyz", "my operation"), operation);
            Client.ServiceInstancesPolling["123"].ServiceBindings["456"].Awaiting(x => x.BindAsync(request)).Should().Throw<BrokerException>().WithMessage("custom message");
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

            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), response);
            SetupMock(x => x.GetLastOperationAsync("123", "456", "abc", "xyz", "my operation"), operation);
            await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
        }

        [Fact]
        public async Task UnbindCompleted()
        {
            var response = new AsyncOperation();

            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), response);
            await Client.ServiceInstancesPolling["123"].ServiceBindings["456"].UnbindAsync("abc", "xyz");
        }

        [Fact]
        public void UnbindGone()
        {
            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), new GoneException());
            Client.ServiceInstancesPolling["123"].ServiceBindings["456"].Awaiting(x => x.UnbindAsync("abc", "xyz")).Should().Throw<GoneException>();
        }

        [Fact]
        public void UnbindAsyncGone()
        {
            var response = new ServiceInstanceAsyncOperation
            {
                Operation = "my operation"
            };

            SetupMock(x => x.UnbindAsync("123", "456", "abc", "xyz"), response);
            SetupMock(x => x.GetLastOperationAsync("123", "456", "abc", "xyz", "my operation"), new GoneException());
            Client.ServiceInstancesPolling["123"].ServiceBindings["456"].Awaiting(x => x.UnbindAsync("abc", "xyz")).Should().Throw<GoneException>();
        }
    }
}
