using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsCredentialSecretConfiguration : IEntityTypeConfiguration<FgsCredentialSecret>
{
    public void Configure(EntityTypeBuilder<FgsCredentialSecret> entity)
    {
        entity.ToTable("FgsCredentialSecret");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.SecretName).HasMaxLength(200);
        entity.Property(e => e.EncryptionKeyId).HasMaxLength(500);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.LastRotatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.ExpiresOn).HasColumnType("timestamptz");
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName("FK_FgsCredentialSecret_Company")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<FgsCredentialProvider>()
            .WithMany()
            .HasForeignKey(e => e.CredentialProviderId)
            .HasConstraintName("FK_FgsCredentialSecret_Provider")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new
            {
                e.TenantId,
                e.CompanyId,
                e.CredentialProviderId,
                e.SecretName,
                e.VersionNo
            })
            .IsUnique()
            .HasDatabaseName("UQ_FgsCredentialSecret");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCredentialSecret_Tenant_Company");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId })
            .HasDatabaseName("IX_FgsCredentialSecret_Tenant_Company_Prov");
        entity.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_FgsCredentialSecret_IsActive");
    }
}
