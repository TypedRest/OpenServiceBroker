using FluentAssertions;
using OpenServiceBroker.Errors;
using Xunit;

namespace OpenServiceBroker.Instances
{
    public class HeaderFacts : FactsBase<IServiceInstanceBlocking>
    {
        [Fact]
        public void WrongMajorVersion()
        {
            Client.SetApiVersion(new ApiVersion(1, 0));
            Client.ServiceInstancesBlocking["123"].Awaiting(x => x.FetchAsync()).Should().Throw<ApiVersionNotSupportedException>();
        }

        [Fact]
        public void TooNewMinorVersion()
        {
            Client.SetApiVersion(new ApiVersion(ApiVersion.Current.Major, ApiVersion.Current.Minor + 1));
            Client.ServiceInstancesBlocking["123"].Awaiting(x => x.FetchAsync()).Should().Throw<ApiVersionNotSupportedException>();
        }
    }
}
