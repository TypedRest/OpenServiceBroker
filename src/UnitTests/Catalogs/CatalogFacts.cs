using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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

            Mock.Setup(x => x.GetCatalogAsync())
                .ReturnsAsync(response);
            var result = await Client.Catalog.ReadAsync();
            result.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task ReadCached()
        {
            Catalog result = new Catalog
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
            Mock.Setup(x => x.GetCatalogAsync())
                .ReturnsAsync(result);

            var catalogEndpoint = Client.Catalog;
            var result1 = await catalogEndpoint.ReadAsync();
            var result2 = await catalogEndpoint.ReadAsync();
            result1.Should().BeEquivalentTo(result2);
        }
    }
}
