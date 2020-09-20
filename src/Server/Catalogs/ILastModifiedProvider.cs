using System;

namespace OpenServiceBroker.Catalogs
{
    /// <summary>
    /// Provides a last modified timestamp.
    /// </summary>
    /// <remarks>Implement this in addition to <see cref="ICatalogService"/> to enable timestamp-based caching.</remarks>
    public interface ILastModifiedProvider
    {
        /// <summary>
        /// The last time the entity this service provides was modified.
        /// </summary>
        DateTimeOffset LastModified { get; }
    }
}
