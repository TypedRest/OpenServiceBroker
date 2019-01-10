using System;
using System.Net;
using System.Net.Http;
using System.Text;
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

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), response);
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

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), response);
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

            SetupMock(x => x.ProvisionAsync(new ServiceInstanceContext("123"), request), new ConflictException("custom message"));
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

            SetupMock(x => x.UpdateAsync(new ServiceInstanceContext("123"), request));
            await Client.ServiceInstancesBlocking["123"].UpdateAsync(request);
        }

        [Fact]
        public async Task Deprovision()
        {
            SetupMock(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"));
            await Client.ServiceInstancesBlocking["123"].DeprovisionAsync("abc", "xyz");
        }

        [Fact]
        public async Task DeprovisionRequest()
        {
            var result = await Client.HttpClient.DeleteAsync(Client.ServiceInstancesBlocking["123"].Uri);
            result.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            var resultString = await result.Content.ReadAsStringAsync();
            resultString.Should().Be("{}");
        }

        [Fact]
        public void DeprovisionGone()
        {
            SetupMock(x => x.DeprovisionAsync(new ServiceInstanceContext("123"), "abc", "xyz"), new GoneException());
            Client.ServiceInstancesBlocking["123"].Awaiting(x => x.DeprovisionAsync("abc", "xyz")).Should().Throw<GoneException>();
        }

        [Fact]
        public async Task BadRequest()
        {
            var result = await Client.HttpClient.PutAsync(Client.ServiceInstancesBlocking["123"].Uri, new StringContent("{}", Encoding.UTF8, "application/json"));
            var error = await result.Content.ReadAsAsync<Error>(new[] {Client.Serializer});
            error.ErrorCode.Should().Be(BadRequestException.ErrorCode);
        }
    }
}
