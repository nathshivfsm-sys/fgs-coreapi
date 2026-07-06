using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventoryItemDependencyConfiguration : IEntityTypeConfiguration<FgsInventoryItemDependency>
{
    public void Configure(EntityTypeBuilder<FgsInventoryItemDependency> entity)
    {
        entity.ToTable("FgsInventoryItemDependency");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryItemId, e.DependentInventoryItemId })
            .HasName("UQ_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId_DependentInventoryItemId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId");

        entity.Property(e => e.Quantity).HasColumnType("numeric(18,4)").HasDefaultValue(1m);
        entity.Property(e => e.IsRequired).HasDefaultValue(true);
        entity.Property(e => e.Notes).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsInventoryItemDependency_FgsInventoryItem_InventoryItemId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.DependentInventoryItemId)
            .HasConstraintName("FK_FgsInventoryItemDependency_FgsInventoryItem_DependentInventoryItemId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
