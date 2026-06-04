using Fgs.Reporting.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Reporting.Infrastructure.Database;

public sealed class FgsReportingDbContext(DbContextOptions<FgsReportingDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Reporting);
    }
}
