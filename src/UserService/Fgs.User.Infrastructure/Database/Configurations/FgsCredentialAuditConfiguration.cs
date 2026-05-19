using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsCredentialAuditConfiguration : IEntityTypeConfiguration<FgsCredentialAudit>
{
    public void Configure(EntityTypeBuilder<FgsCredentialAudit> entity)
    {
        entity.ToTable("FgsCredentialAudit");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.ActionType).HasMaxLength(100);
        entity.Property(e => e.Remarks).HasMaxLength(1000);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName("FK_FgsCredentialAudit_Company")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<FgsCredentialSecret>()
            .WithMany()
            .HasForeignKey(e => e.CredentialSecretId)
            .HasConstraintName("FK_FgsCredentialAudit_CredentialSecret")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new
            {
                e.TenantId,
                e.CompanyId,
                e.CredentialSecretId,
                e.ActionType,
                e.NewVersionNo
            })
            .IsUnique()
            .HasDatabaseName("UQ_FgsCredentialAudit");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCredentialAudit_Tenant_Company");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialSecretId })
            .HasDatabaseName("IX_FgsCredentialAudit_Tenant_Company_Cred");
    }
}
