using System.Threading.Tasks;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// Provides access to the catalog of services that the service broker offers.
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Gets the catalog of services that the service broker offers.
        /// </summary>
        Task<Catalog> GetCatalogAsync();
    }
}
