using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Get()
        {
            var catalog = await _catalogService.GetCatalogAsync();
            string eTag = GenerateETag(catalog);

            if (Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag.Value == eTag) ?? false)
                return StatusCode((int) HttpStatusCode.NotModified);

            Response.GetTypedHeaders().ETag = new EntityTagHeaderValue(eTag);
            return Ok(catalog);
        }

        private static string GenerateETag(Catalog catalog)
        {
            string serializedCatalog = JsonConvert.SerializeObject(catalog);
            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(serializedCatalog));
            return '"' + BitConverter.ToString(hash) + '"';
        }
    }
}
