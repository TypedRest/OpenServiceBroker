using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
                PlanId = "xyz",
                OrganizationGuid = "org",
                SpaceGuid = "space"
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

            Mock.Setup(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request))
                .ReturnsAsync(response);
            Mock.Setup(x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation"))
                .ReturnsAsync(operation);
            var result = await Client.ServiceInstancesPolling["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(syntheticResponse);
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
            var syntheticResponse = new ServiceInstanceProvision
            {
                DashboardUrl = new Uri("http://example.com")
            };
            var response = new ServiceInstanceAsyncOperation().Complete(syntheticResponse);

            Mock.Setup(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request))
                .ReturnsAsync(response);
            var result = await Client.ServiceInstancesPolling["123"].ProvisionAsync(request);
            result.Should().BeEquivalentTo(syntheticResponse);
        }

        [Fact]
        public void ProvisionConflict()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz",
                OrganizationGuid = "org",
                SpaceGuid = "space"
            };

            Mock.Setup((x => x.ProvisionAsync(new ServiceInstanceContext("123"), request)))
                .Throws(new ConflictException("custom message"));
            Client.ServiceInstancesPolling["123"]
                  .Awaiting(x => x.ProvisionAsync(request))
                  .Should().Throw<ConflictException>().WithMessage("custom message");
        }

        [Fact]
        public void ProvisionAsyncError()
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
                Operation = "my operation"
            };
            var operation = new LastOperationResource
            {
                State = LastOperationResourceState.Failed,
                Description = "custom message"
            };

            Mock.Setup(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request))
                .ReturnsAsync(response);
            Mock.Setup(x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation"))
                .ReturnsAsync(operation);
            Client.ServiceInstancesPolling["123"]
                  .Awaiting(x => x.ProvisionAsync(request))
                  .Should().Throw<BrokerException>().WithMessage("custom message");
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

            Mock.Setup(x => x.UpdateAsync(new ServiceInstanceContext("123"), request))
                .ReturnsAsync(response);
            Mock.Setup(x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation"))
                .ReturnsAsync(operation);
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

            Mock.Setup(x => x.UpdateAsync(new ServiceInstanceContext("123"), request))
                .ReturnsAsync(response);
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

            Mock.Setup(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"))
                .ReturnsAsync(response);
            Mock.Setup(x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation"))
                .ReturnsAsync(operation);
            await Client.ServiceInstancesPolling["123"].DeprovisionAsync("abc", "xyz");
        }

        [Fact]
        public async Task DeprovisionCompleted()
        {
            var response = new AsyncOperation();

            Mock.Setup(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"))
                .ReturnsAsync(response);
            await Client.ServiceInstancesPolling["123"].DeprovisionAsync("abc", "xyz");
        }

        [Fact]
        public void DeprovisionGone()
        {
            Mock.Setup((x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz")))
                .Throws<GoneException>();
            Client.ServiceInstancesPolling["123"]
                  .Awaiting(x => x.DeprovisionAsync("abc", "xyz"))
                  .Should().Throw<GoneException>();
        }

        [Fact]
        public void DeprovisionAsyncGone()
        {
            var response = new ServiceInstanceAsyncOperation
            {
                Operation = "my operation"
            };

            Mock.Setup(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"))
                .ReturnsAsync(response);
            Mock.Setup((x => x.GetLastOperationAsync(new ServiceInstanceContext("123"), "abc", "xyz", "my operation")))
                .Throws<GoneException>();
            Client.ServiceInstancesPolling["123"]
                  .Awaiting(x => x.DeprovisionAsync("abc", "xyz"))
                  .Should().Throw<GoneException>();
        }
    }
}
