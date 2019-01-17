using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace OpenServiceBroker
{
    public abstract class FactsBase<TMock> : IDisposable
        where TMock : class
    {
        private readonly TestServer _server;

        private readonly Mock<TMock> _mock = new Mock<TMock>();

        protected readonly OpenServiceBrokerClient Client;

        protected FactsBase()
        {
            _server = new TestServer(new WebHostBuilder()
                                    .ConfigureServices(x => x.AddOpenServiceBroker()
                                                             .AddSingleton(_mock.Object)
                                                             .AddMvc()
                                                             .SetCompatibilityVersion(CompatibilityVersion.Version_2_2))
                                    .Configure(x => x.UseMvc()));
            Client = new OpenServiceBrokerClient(new Uri("http://localhost"), _server.CreateClient());
        }

        protected void SetupMock(Expression<Func<TMock, Task>> expression)
            => _mock.Setup(expression).Returns(Task.CompletedTask);

        protected void SetupMock<T>(Expression<Func<TMock, Task<T>>> expression, T result)
            => _mock.Setup(expression).ReturnsAsync(result);

        protected void SetupMock(Expression<Func<TMock, Task>> expression, Exception exception)
            => _mock.Setup(expression).Throws(exception);

        public virtual void Dispose()
        {
            try
            {
                _mock.VerifyAll();
            }
            finally
            {
                _server.Dispose();
            }
        }
    }
}
