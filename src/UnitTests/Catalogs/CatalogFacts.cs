using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace OpenServiceBroker.Catalogs
{
    public class CatalogFacts : FactsBase<ICatalogService>
    {
        [Fact]
        public async Task Read()
        {
            var response = new Catalog
            {
                Services =
                {
                    new Service
                    {
                        Id = "123",
                        Name = "my_service",
                        Description = "my service"
                    }
                }
            };

            SetupMock(x => x.GetCatalogAsync(), response);
            var result = await Client.Catalog.ReadAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task ReadCached()
        {
            SetupMock(x => x.GetCatalogAsync(), new Catalog
            {
                Services =
                {
                    new Service
                    {
                        Id = "123",
                        Name = "my_service",
                        Description = "my service"
                    }
                }
            });

            var catalogEndpoint = Client.Catalog;
            var result1 = await catalogEndpoint.ReadAsync();
            var result2 = await catalogEndpoint.ReadAsync();
            result1.Should().BeEquivalentTo(result2);
        }
    }
}
