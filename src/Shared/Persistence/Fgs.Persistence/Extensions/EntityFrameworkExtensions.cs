using Fgs.Kernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Fgs.Persistence.Extensions;

public static class EntityFrameworkExtensions
{
    public static DbContextOptionsBuilder UseFgsNpgsql(
        this DbContextOptionsBuilder options,
        string connectionString,
        string migrationsHistoryTable,
        string? migrationsHistorySchema = null)
    {
        options.UseNpgsql(connectionString, npgsql =>
        {
            if (migrationsHistorySchema is not null)
            {
                npgsql.MigrationsHistoryTable(migrationsHistoryTable, migrationsHistorySchema);
            }
            else
            {
                npgsql.MigrationsHistoryTable(migrationsHistoryTable);
            }

            npgsql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);
        });

        return options;
    }

    public static DbContextOptionsBuilder UseFgsNpgsql(
        this DbContextOptionsBuilder options,
        IConfiguration configuration,
        string connectionStringName,
        string migrationsHistoryTable,
        string? migrationsHistorySchema = null)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException($"ConnectionStrings:{connectionStringName} is required.");

        return options.UseFgsNpgsql(connectionString, migrationsHistoryTable, migrationsHistorySchema);
    }

    public static void ConfigureGloEntityAuditColumns<T>(this EntityTypeBuilder<T> entity)
        where T : GloEntityBase
    {
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);
    }
}
