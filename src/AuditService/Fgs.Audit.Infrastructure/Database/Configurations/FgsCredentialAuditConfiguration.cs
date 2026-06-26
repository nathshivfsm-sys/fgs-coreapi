using Fgs.Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Audit.Infrastructure.Database.Configurations;

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

        entity.HasIndex(e => e.CredentialId);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCredentialAudit_Tenant_Company");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialId })
            .HasDatabaseName("IX_FgsCredentialAudit_Tenant_Company_Cred");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialId, e.ActionType, e.NewVersionNo })
            .IsUnique()
            .HasDatabaseName("UQ_FgsCredentialAudit");
    }
}
