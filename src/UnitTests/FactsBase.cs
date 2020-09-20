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

        protected readonly Mock<TMock> Mock = new Mock<TMock>();

        protected readonly OpenServiceBrokerClient Client;

        protected FactsBase()
        {
            _server = new TestServer(
                new WebHostBuilder()
#if NETCOREAPP2_1
                   .ConfigureServices(x
                        => x.AddTransient(_ => Mock.Object)
                            .AddMvc()
                            .AddOpenServiceBroker())
                   .Configure(x => x.UseMvc()));
#elif NETCOREAPP3_1
                   .ConfigureServices(x
                        => x.AddScoped(_ => Mock.Object)
                            .AddControllers()
                            .AddOpenServiceBroker())
                   .Configure(x => x.UseRouting()
                                    .UseEndpoints(endpoints => endpoints.MapControllers())));
#endif
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
