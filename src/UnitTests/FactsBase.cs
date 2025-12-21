using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenServiceBroker;

public abstract class FactsBase<TMock> : IDisposable
    where TMock : class
{
    protected readonly Mock<TMock> Mock = new();
    private readonly TestFactory _factory;
    protected readonly OpenServiceBrokerClient Client;

    protected FactsBase()
    {
        _factory = new(Mock);
        Client = new(_factory.CreateClient(), new Uri("http://localhost"));
    }

    public virtual void Dispose()
    {
        try
        {
            Mock.VerifyAll();
        }
        finally
        {
            _factory.Dispose();
        }
    }

    private sealed class Stub;

    private sealed class TestFactory(Mock<TMock> mock) : WebApplicationFactory<Stub>
    {
        protected override IHostBuilder CreateHostBuilder()
            => Host.CreateDefaultBuilder()
                   .ConfigureWebHostDefaults(webBuilder
                        => webBuilder.UseTestServer()
                                     .ConfigureServices(services
                                          => services.AddScoped(_ => mock.Object)
                                                     .AddControllers()
                                                     .AddOpenServiceBroker())
                                     .Configure(app
                                          => app.UseRouting()
                                                .UseEndpoints(endpoints => endpoints.MapControllers())));
    }
}
