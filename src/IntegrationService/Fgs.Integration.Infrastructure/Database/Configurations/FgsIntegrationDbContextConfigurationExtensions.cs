using Fgs.Integration.Domain.Entities;
using Fgs.Kernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Integration.Infrastructure.Database.Configurations;

internal static class FgsIntegrationDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped
    {
        entity.Property(nameof(ITenantCompanyScoped.TenantId)).HasColumnOrder(1);
        entity.Property(nameof(ITenantCompanyScoped.CompanyId)).HasColumnOrder(2);
    }

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder)
    {
        var excludedTypes = new HashSet<Type>
        {
            typeof(FgsTenantCompanyCache),
            typeof(FgsPaymentTransactionPayload)
        };

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

            var constraintName = tableName switch
            {
                "FgsPaymentTransactionPayload" => "FK_FgsPaymentTransactionPayload_TenantCompany",
                _ => $"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId"
            };

            ((EntityTypeBuilder)modelBuilder.Entity(clrType))
                .HasOne(typeof(FgsTenantCompanyCache))
                .WithMany()
                .HasForeignKey(nameof(ITenantCompanyScoped.TenantId), nameof(ITenantCompanyScoped.CompanyId))
                .HasConstraintName(constraintName)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
