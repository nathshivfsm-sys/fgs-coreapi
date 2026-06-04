using Fgs.Billing.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Billing.Infrastructure.Database;

public sealed class FgsBillingDbContext(DbContextOptions<FgsBillingDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Billing);
    }
}
