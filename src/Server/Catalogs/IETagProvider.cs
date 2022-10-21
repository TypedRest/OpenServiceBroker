namespace OpenServiceBroker.Catalogs;

/// <summary>
/// Provides an ETag.
/// </summary>
/// <remarks>Implement this in addition to <see cref="ICatalogService"/> to enable ETag-based caching.</remarks>
public interface IETagProvider
{
    /// <summary>
    /// The current ETag of the entity this service provides. Must be enclosed in quotes (").
    /// </summary>
    string ETag { get; }
}
