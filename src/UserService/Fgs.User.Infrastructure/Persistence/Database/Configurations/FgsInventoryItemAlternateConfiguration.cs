using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsInventoryItemAlternateConfiguration : IEntityTypeConfiguration<FgsInventoryItemAlternate>
{
    public void Configure(EntityTypeBuilder<FgsInventoryItemAlternate> entity)
    {
        entity.ToTable("FgsInventoryItemAlternate");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryItemId, e.AlternateInventoryItemId })
            .HasName("UQ_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId_AlternateInventoryItemId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryItemId })
            .HasDatabaseName("IX_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId");

        entity.Property(e => e.AlternateType).HasMaxLength(50).IsRequired();
        entity.Property(e => e.PriorityOrder).HasDefaultValue((short)1);
        entity.Property(e => e.Notes).HasColumnType("text");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName("FK_FgsInventoryItemAlternate_FgsTenantCompany_TenantId_CompanyId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.InventoryItemId)
            .HasConstraintName("FK_FgsInventoryItemAlternate_FgsInventoryItem_InventoryItemId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInventoryItem>()
            .WithMany()
            .HasForeignKey(e => e.AlternateInventoryItemId)
            .HasConstraintName("FK_FgsInventoryItemAlternate_FgsInventoryItem_AlternateInventoryItemId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
