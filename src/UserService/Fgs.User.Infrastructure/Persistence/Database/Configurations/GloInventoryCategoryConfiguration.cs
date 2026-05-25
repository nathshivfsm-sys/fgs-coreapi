using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloInventoryCategoryConfiguration : IEntityTypeConfiguration<GloInventoryCategory>
{
    public void Configure(EntityTypeBuilder<GloInventoryCategory> entity)
    {
        entity.ToTable("GloInventoryCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasIndex(e => new { e.BusinessTypeId, e.CategoryCode })
            .IsUnique()
            .HasDatabaseName("UQ_GloInventoryCategory_BusinessTypeId_CategoryCode");
        entity.HasIndex(e => e.BusinessTypeId)
            .HasDatabaseName("IX_GloInventoryCategory_BusinessTypeId");
        entity.Property(e => e.CategoryCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloInventoryCategory_GloBusinessType")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
