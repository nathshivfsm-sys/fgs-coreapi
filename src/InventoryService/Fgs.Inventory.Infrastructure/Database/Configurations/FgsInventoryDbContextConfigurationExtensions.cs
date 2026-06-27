using Fgs.Inventory.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal static class FgsInventoryDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped =>
        EntityFrameworkExtensions.ConfigureTenantCompanyColumns(entity);

    internal static void ConfigureCatalogColumns<T>(this EntityTypeBuilder<T> entity)
        where T : FgsTenantCompanySetupEntityBase<long>
    {
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.ConfigureTenantCompanyColumns();
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);
    }

    internal static void ConfigureAuditColumns(this EntityTypeBuilder entity) =>
        entity.ConfigureTimestamptzAuditColumns();

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type>
            {
                typeof(FgsTenantCompanyCache),
                typeof(InventoryOutboxMessage)
            });
}
