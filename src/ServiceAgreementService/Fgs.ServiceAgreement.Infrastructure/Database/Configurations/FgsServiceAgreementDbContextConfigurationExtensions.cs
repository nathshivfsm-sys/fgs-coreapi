using Fgs.Kernel.Entities;
using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal static class FgsServiceAgreementDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped
    {
        entity.Property(nameof(ITenantCompanyScoped.TenantId)).HasColumnOrder(1);
        entity.Property(nameof(ITenantCompanyScoped.CompanyId)).HasColumnOrder(2);
    }

    internal static void ConfigureAuditColumns(this EntityTypeBuilder entity)
    {
        entity.Property("CreatedOn").HasColumnType("timestamptz");
        entity.Property("UpdatedOn").HasColumnType("timestamptz");
    }

    internal static void ConfigureTenantCompanyCacheFkNonGeneric(
        this EntityTypeBuilder entity,
        string constraintName)
    {
        entity.HasOne(typeof(FgsTenantCompanyCache))
            .WithMany()
            .HasForeignKey(nameof(ITenantCompanyScoped.TenantId), nameof(ITenantCompanyScoped.CompanyId))
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder)
    {
        var excludedTypes = new HashSet<Type> { typeof(FgsTenantCompanyCache) };

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null || excludedTypes.Contains(clrType))
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
                .ConfigureTenantCompanyCacheFkNonGeneric($"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId");
        }
    }
}
