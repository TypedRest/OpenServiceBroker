using System;
using System.Threading.Tasks;
using FluentAssertions;
using OpenServiceBroker.Errors;
using Xunit;

namespace OpenServiceBroker.Instances
{
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

            SetupMock(x => x.FetchAsync("123"), response);
            var result = await Client.ServiceInstancesBlocking["123"].FetchAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task Provision()
        {
            var request = new ServiceInstanceProvisionRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };
            var response = new ServiceInstanceProvision
            {
                DashboardUrl = new Uri("http://example.com")
            };

            SetupMock(x => x.ProvisionAsync("123", request), response);
            var result = await Client.ServiceInstancesBlocking["123"].ProvisionAsync(request);
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
            var response = new ServiceInstanceProvision
            {
                DashboardUrl = new Uri("http://example.com"),
                Unchanged = true
            };

            SetupMock(x => x.ProvisionAsync("123", request), response);
            var result = await Client.ServiceInstancesBlocking["123"].ProvisionAsync(request);
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

            SetupMock(x => x.ProvisionAsync("123", request), new ConflictException("custom message"));
            Client.ServiceInstancesBlocking["123"].Awaiting(x => x.ProvisionAsync(request)).Should().Throw<ConflictException>().WithMessage("custom message");
        }

        [Fact]
        public async Task Update()
        {
            var request = new ServiceInstanceUpdateRequest
            {
                ServiceId = "abc",
                PlanId = "xyz"
            };

            SetupMock(x => x.UpdateAsync("123", request));
            await Client.ServiceInstancesBlocking["123"].UpdateAsync(request);
        }

        [Fact]
        public async Task Deprovision()
        {
            SetupMock(x => x.DeprovisionAsync("123", "abc", "xyz"));
            await Client.ServiceInstancesBlocking["123"].DeprovisionAsync("abc", "xyz");
        }

        [Fact]
        public void DeprovisionGone()
        {
            SetupMock(x => x.DeprovisionAsync("123", "abc", "xyz"), new GoneException());
            Client.ServiceInstancesBlocking["123"].Awaiting(x => x.DeprovisionAsync("abc", "xyz")).Should().Throw<GoneException>();
        }
    }
}
