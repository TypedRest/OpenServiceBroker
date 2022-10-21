namespace OpenServiceBroker.Catalogs;

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
        Catalog result = new()
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
        Mock.As<IETagProvider>().SetupGet(x => x.ETag)
            .Returns("\"abc\"");
        Mock.As<ILastModifiedProvider>().SetupGet(x => x.LastModified)
            .Returns(new DateTimeOffset(new DateTime(2000, 1, 1)));
        Mock.Setup(x => x.GetCatalogAsync())
            .ReturnsAsync(result);

        var catalogEndpoint = Client.Catalog;
        var result1 = await catalogEndpoint.ReadAsync();
        var result2 = await catalogEndpoint.ReadAsync();
        result1.Should().BeEquivalentTo(result2);

        Mock.Verify(x => x.GetCatalogAsync(), Times.Once());
    }
}
