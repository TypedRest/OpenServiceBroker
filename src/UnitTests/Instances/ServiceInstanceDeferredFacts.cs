using System;
using System.Threading.Tasks;
using FluentAssertions;
using OpenServiceBroker.Errors;
using Xunit;

namespace OpenServiceBroker.Instances
{
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

            SetupMock(x => x.FetchAsync("123"), response);
            var result = await Client.ServiceInstancesDeferred["123"].FetchAsync();
            result.Should().BeEquivalentTo(response);
        }

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

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), response);
            var result = await Client.ServiceInstancesDeferred["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task ProvisionCompleted()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var response = new ServiceInstanceAsyncOperation().Complete(new ServiceInstanceProvision
            {
                DashboardUrl = new Uri("http://example.com")
            });

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), response);
            var result = await Client.ServiceInstancesDeferred["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task ProvisionUnchanged()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var response = new ServiceInstanceAsyncOperation().Complete(new ServiceInstanceProvision
            {
                DashboardUrl = new Uri("http://example.com"),
                Unchanged = true
            });

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), response);
            var result = await Client.ServiceInstancesDeferred["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void ProvisionConflict()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), new ConflictException("custom message"));
            Client.ServiceInstancesDeferred["123"].Awaiting(x => x.ProvisionAsync(request)).Should().Throw<ConflictException>().WithMessage("custom message");
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

            SetupMock(x => x.UpdateAsync(new ServiceInstanceContext("123"), request), response);
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

            SetupMock(x => x.UpdateAsync(new ServiceInstanceContext("123"), request), response);
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

            SetupMock(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"), response);
            var result = await Client.ServiceInstancesDeferred["123"].DeprovisionAsync("abc", "xyz");
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task DeprovisionCompleted()
        {
            var response = new AsyncOperation();

            SetupMock(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"), response);
            var result = await Client.ServiceInstancesDeferred["123"].DeprovisionAsync("abc", "xyz");
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void DeprovisionGone()
        {
            SetupMock(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"), new GoneException());
            Client.ServiceInstancesDeferred["123"].Awaiting(x => x.DeprovisionAsync("abc", "xyz")).Should().Throw<GoneException>();
        }

        [Fact]
        public async Task GetLastOperation()
        {
            var response = new LastOperationResource
            {
                State = LastOperationResourceState.InProgress
            };

            SetupMock(x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation"), response);
            var result = await Client.ServiceInstancesDeferred["123"].LastOperation("abc", "xyz", "my operation").ReadAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void GetLastOperationGone()
        {
            SetupMock(x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation"), new GoneException("custom message"));
            Client.ServiceInstancesDeferred["123"].LastOperation("abc", "xyz", "my operation").Awaiting(x => x.ReadAsync()).Should().Throw<GoneException>().WithMessage("custom message");
        }
    }
}
