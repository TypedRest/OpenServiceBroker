using System.Threading.Tasks;
using OpenServiceBroker.Catalogs;

namespace MyServiceBroker
{
    public class CatalogService : ICatalogService
    {
        private readonly Catalog _catalog;

        public CatalogService(Catalog catalog)
        {
            _catalog = catalog;
        }

        public Task<Catalog> GetCatalogAsync()
        {
            return Task.FromResult(_catalog);
        }
    }
}
