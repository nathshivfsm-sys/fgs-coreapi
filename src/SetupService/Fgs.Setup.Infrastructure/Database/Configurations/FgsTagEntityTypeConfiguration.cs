using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsTagEntityTypeConfiguration : IEntityTypeConfiguration<FgsTagEntityType>
{
    public void Configure(EntityTypeBuilder<FgsTagEntityType> entity)
    {
        entity.ToTable("FgsTagEntityType");
        entity.HasKey(e => new { e.TenantId, e.CompanyId, e.TagId, e.MasterEntityTypeId });
        entity.Property(e => e.IsDefault).HasDefaultValue(false);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TagId })
            .HasDatabaseName("IX_FgsTagEntityType_TagId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId })
            .HasDatabaseName("IX_FgsTagEntityType_MasterEntityTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId, e.IsDefault })
            .HasDatabaseName("IX_FgsTagEntityType_IsDefault"); entity.HasOne<FgsTag>()
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName("FK_FgsTagEntityType_FgsTag_TagId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
