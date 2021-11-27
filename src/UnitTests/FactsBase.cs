using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace OpenServiceBroker
{
    public abstract class FactsBase<TMock> : IDisposable
        where TMock : class
    {
        private readonly TestServer _server;

        protected readonly Mock<TMock> Mock = new();

        protected readonly OpenServiceBrokerClient Client;

        protected FactsBase()
        {
            _server = new TestServer(
                new WebHostBuilder()
                   .ConfigureServices(x
                        => x.AddScoped(_ => Mock.Object)
                            .AddControllers()
                            .AddOpenServiceBroker())
                   .Configure(x => x.UseRouting()
                                    .UseEndpoints(endpoints => endpoints.MapControllers())));
            Client = new OpenServiceBrokerClient(_server.CreateClient(), new Uri("http://localhost"));
        }

        public virtual void Dispose()
        {
            try
            {
                Mock.VerifyAll();
            }
            finally
            {
                _server.Dispose();
            }
        }
    }
}
