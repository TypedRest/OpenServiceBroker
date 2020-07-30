using Microsoft.EntityFrameworkCore;

namespace MyServiceBroker
{
    /// <summary>
    /// Describes the service's database model.
    /// Used as a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ServiceInstanceEntity> ServiceInstances { get; set; } = default!;

        public DbContext(DbContextOptions options)
            : base(options)
        {}
    }
}
