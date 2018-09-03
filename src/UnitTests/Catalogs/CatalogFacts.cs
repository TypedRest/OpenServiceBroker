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
            var response = new Catalog()
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
            AssertionExtensions.Should((object)result).BeEquivalentTo(response);
        }
    }
}
