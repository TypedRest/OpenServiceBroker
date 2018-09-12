using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// Exposes a list of all services available on the Service Broker.
    /// </summary>
    [Route("v2/catalog")]
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Gets the catalog of services that the service broker offers.
        /// </summary>
        [Route(""), HttpGet]
        public Task<Catalog> Get()
            => _catalogService.GetCatalogAsync();
    }
}
