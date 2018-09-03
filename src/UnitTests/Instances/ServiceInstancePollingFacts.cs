using System;
using System.Threading.Tasks;
using FluentAssertions;
using OpenServiceBroker.Errors;
using Xunit;

namespace OpenServiceBroker.Instances
{
    public class ServiceInstancePollingFacts : FactsBase<IServiceInstanceDeferred>
    {
        [Fact]
        public async Task ProvisionDeferred()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var response = new ServiceInstanceAsyncOperation
            {
                Operation = "my operation",
                DashboardUrl = new Uri("http://example.com")
            };
            var operation = new LastOperationResource
            {
                State = LastOperationResourceState.Succeeded,
                Description = "done"
            };
            var syntheticResponse = new ServiceInstanceProvision
            {
                DashboardUrl = response.DashboardUrl
            };

            SetupMock(x => x.ProvisionAsync("123", request), response);
            SetupMock(x => x.GetLastOperationAsync("123", "abc", "xyz", "my operation"), operation);
            var result = await Client.ServiceInstancesPolling["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(syntheticResponse);
        }

        [Fact]
        public async Task ProvisionCompleted()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var syntheticResponse = new ServiceInstanceProvision
            {
                DashboardUrl = new Uri("http://example.com")
            };
            var response = new ServiceInstanceAsyncOperation
            {
                Result = syntheticResponse
            };

            SetupMock(x => x.ProvisionAsync("123", request), response);
            var result = await Client.ServiceInstancesPolling["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(syntheticResponse);
        }

        [Fact]
        public void ProvisionConflict()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };

            SetupMock(x => x.ProvisionAsync("123", request), new ConflictException("custom message"));
            Client.ServiceInstancesPolling["123"].Awaiting(x => x.ProvisionAsync(request)).Should().Throw<ConflictException>().WithMessage("custom message");
        }

        [Fact]
        public void ProvisionAsyncError()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var response = new ServiceInstanceAsyncOperation
            {
                Operation = "my operation"
            };
            var operation = new LastOperationResource
            {
                State = LastOperationResourceState.Failed,
                Description = "custom message"
            };

            SetupMock(x => x.ProvisionAsync("123", request), response);
            SetupMock(x => x.GetLastOperationAsync("123", "abc", "xyz", "my operation"), operation);
            Client.ServiceInstancesPolling["123"].Awaiting(x => x.ProvisionAsync(request)).Should().Throw<BrokerException>().WithMessage("custom message");
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
                Operation = "my operation"
            };
            var operation = new LastOperationResource
            {
                State = LastOperationResourceState.Succeeded,
                Description = "done"
            };

            SetupMock(x => x.UpdateAsync("123", request), response);
            SetupMock(x => x.GetLastOperationAsync("123", "abc", "xyz", "my operation"), operation);
            await Client.ServiceInstancesPolling["123"].UpdateAsync(request);
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

            SetupMock(x => x.UpdateAsync("123", request), response);
            await Client.ServiceInstancesPolling["123"].UpdateAsync(request);
        }

        [Fact]
        public async Task DeprovisionDeferred()
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

            SetupMock(x => x.DeprovisionAsync("123", "abc", "xyz"), response);
            SetupMock(x => x.GetLastOperationAsync("123", "abc", "xyz", "my operation"), operation);
            await Client.ServiceInstancesPolling["123"].DeprovisionAsync("abc", "xyz");
        }

        [Fact]
        public async Task DeprovisionCompleted()
        {
            var response = new AsyncOperation();

            SetupMock(x => x.DeprovisionAsync("123", "abc", "xyz"), response);
            await Client.ServiceInstancesPolling["123"].DeprovisionAsync("abc", "xyz");
        }

        [Fact]
        public void DeprovisionGone()
        {
            SetupMock(x => x.DeprovisionAsync("123", "abc", "xyz"), new GoneException());
            Client.ServiceInstancesPolling["123"].Awaiting(x => x.DeprovisionAsync("abc", "xyz")).Should().Throw<GoneException>();
        }

        [Fact]
        public void DeprovisionAsyncGone()
        {
            var response = new ServiceInstanceAsyncOperation
            {
                Operation = "my operation"
            };

            SetupMock(x => x.DeprovisionAsync("123", "abc", "xyz"), response);
            SetupMock(x => x.GetLastOperationAsync("123", "abc", "xyz", "my operation"), new GoneException());
            Client.ServiceInstancesPolling["123"].Awaiting(x => x.DeprovisionAsync("abc", "xyz")).Should().Throw<GoneException>();
        }
    }
}
