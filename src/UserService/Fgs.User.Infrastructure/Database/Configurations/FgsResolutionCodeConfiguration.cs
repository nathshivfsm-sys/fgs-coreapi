using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsResolutionCodeConfiguration : IEntityTypeConfiguration<FgsResolutionCode>
{
    public void Configure(EntityTypeBuilder<FgsResolutionCode> entity)
    {
        entity.ToTable("FgsResolutionCode");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName("FK_FgsResolutionCode_Company")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.ResolutionType)
            .WithMany()
            .HasForeignKey(e => e.GloResolutionTypeId)
            .HasConstraintName("FK_FgsResolutionCode_GloResType")
            .OnDelete(DeleteBehavior.Restrict);
        entity.Property(e => e.ResolutionCode).HasMaxLength(50);
        entity.Property(e => e.ResolutionName).HasMaxLength(200);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.ResolutionCode })
            .HasName("UQ_FgsResolutionCode_Code");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.GloResolutionTypeId })
            .HasDatabaseName("IX_FgsResolutionCode_GloResType");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_FgsResolutionCode_Code_Upper",
            "\"ResolutionCode\" = UPPER(\"ResolutionCode\")"));
    }
}
