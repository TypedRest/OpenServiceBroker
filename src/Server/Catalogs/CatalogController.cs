using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenServiceBroker.Catalogs;

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
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public async Task<IActionResult> Get()
    {
        var requestHeaders = Request.GetTypedHeaders();
        var responseHeaders = Response.GetTypedHeaders();

        string? eTag = (_catalogService as IETagProvider)?.ETag;
        if (!string.IsNullOrEmpty(eTag))
        {
            if (requestHeaders.IfNoneMatch?.Any(x => x.Tag.Value == eTag) ?? false)
                return StatusCode((int) HttpStatusCode.NotModified);
            responseHeaders.ETag = new(eTag);
        }

        var lastModified = (_catalogService as ILastModifiedProvider)?.LastModified;
        if (lastModified.HasValue)
        {
            if (requestHeaders.IfModifiedSince.HasValue && requestHeaders.IfModifiedSince <= lastModified.Value)
                return StatusCode((int) HttpStatusCode.NotModified);
            responseHeaders.LastModified = lastModified;
        }

        return Ok(await _catalogService.GetCatalogAsync());
    }
}
