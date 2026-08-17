using Fgs.Kernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Fgs.Persistence.Extensions;

public static class EntityFrameworkExtensions
{
    /// <summary>
    /// Registers a DbContext with <see cref="ServiceLifetime.Scoped"/> options so connection
    /// strings from credential snapshots are re-resolved on each scope (request) after hot-reload.
    /// </summary>
    public static IServiceCollection AddFgsDbContext<TContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where TContext : DbContext =>
        services.AddDbContext<TContext>(
            optionsAction,
            contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Scoped);

    public static DbContextOptionsBuilder UseFgsNpgsql(
        this DbContextOptionsBuilder options,
        string connectionString,
        string migrationsHistoryTable,
        string? migrationsHistorySchema = null,
        Action<NpgsqlDbContextOptionsBuilder>? configure = null)
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

            configure?.Invoke(npgsql);
        });

        // Health checks / credential reloads can rebuild options; avoid treating that as fatal.
        options.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.ManyServiceProvidersCreatedWarning));

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

    public static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped
    {
        entity.Property(nameof(ITenantCompanyScoped.TenantId)).HasColumnOrder(1);
        entity.Property(nameof(ITenantCompanyScoped.CompanyId)).HasColumnOrder(2);
    }

    public static void ConfigureTimestamptzAuditColumns(this EntityTypeBuilder entity)
    {
        entity.Property("CreatedOn").HasColumnType("timestamptz");
        entity.Property("UpdatedOn").HasColumnType("timestamptz");
    }

    public static void ConfigureTenantCompanyCacheFkNonGeneric(
        this EntityTypeBuilder entity,
        Type tenantCompanyCacheClrType,
        string constraintName)
    {
        entity.HasOne(tenantCompanyCacheClrType)
            .WithMany()
            .HasForeignKey(nameof(ITenantCompanyScoped.TenantId), nameof(ITenantCompanyScoped.CompanyId))
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public static void ConfigureTenantCompanyCacheFk<T>(
        this EntityTypeBuilder<T> entity,
        Type tenantCompanyCacheClrType,
        string constraintName)
        where T : class, ITenantCompanyScoped =>
        ((EntityTypeBuilder)entity).ConfigureTenantCompanyCacheFkNonGeneric(tenantCompanyCacheClrType, constraintName);

    public static void ApplyTenantCompanyCacheForeignKeys(
        this ModelBuilder modelBuilder,
        Type tenantCompanyCacheClrType,
        IReadOnlySet<Type>? excludedClrTypes = null,
        Func<string, string>? resolveConstraintName = null)
    {
        excludedClrTypes ??= new HashSet<Type>();
        resolveConstraintName ??= tableName => $"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId";

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null || excludedClrTypes.Contains(clrType))
            {
                continue;
            }

            if (!typeof(ITenantCompanyScoped).IsAssignableFrom(clrType))
            {
                continue;
            }

            var tableName = entityType.GetTableName();
            if (string.IsNullOrEmpty(tableName))
            {
                continue;
            }

            ((EntityTypeBuilder)modelBuilder.Entity(clrType))
                .ConfigureTenantCompanyCacheFkNonGeneric(
                    tenantCompanyCacheClrType,
                    resolveConstraintName(tableName));
        }
    }

    public static void ConfigureAuditActorColumns(this ModelBuilder modelBuilder, int maxLength = 100)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdBy = entityType.FindProperty("CreatedBy");
            if (createdBy?.ClrType == typeof(string))
            {
                createdBy.SetMaxLength(maxLength);
            }

            var updatedBy = entityType.FindProperty("UpdatedBy");
            if (updatedBy?.ClrType == typeof(string))
            {
                updatedBy.SetMaxLength(maxLength);
            }
        }
    }
}
