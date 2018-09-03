using System.Threading.Tasks;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// provides access to the catalog of services that the service broker offers
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// get the catalog of services that the service broker offers
        /// </summary>
        Task<Catalog> GetCatalogAsync();
    }
}
