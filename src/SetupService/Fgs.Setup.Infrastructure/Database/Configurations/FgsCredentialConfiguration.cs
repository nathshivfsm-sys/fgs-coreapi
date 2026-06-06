using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsCredentialConfiguration : IEntityTypeConfiguration<FgsCredential>
{
    public void Configure(EntityTypeBuilder<FgsCredential> entity)
    {
        entity.ToTable(
            "FgsCredential",
            t => t.HasComment(
                "Stores tenant-owned credentials encrypted using AWS KMS envelope encryption."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.TenantId)
            .HasComment("Tenant that owns the credential.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company that owns the credential.");
        entity.Property(e => e.CredentialProviderTypeId)
            .HasComment("Credential provider associated with this credential.");
        entity.Property(e => e.CredentialName)
            .HasMaxLength(200)
            .HasComment("User friendly name displayed in tenant administration screens.");
        entity.Property(e => e.Description)
            .HasMaxLength(500)
            .HasComment("Optional description of the credential usage.");
        entity.Property(e => e.CredentialData)
            .HasColumnType("bytea")
            .HasComment("Provider credential JSON encrypted using a Data Encryption Key (DEK).");
        entity.Property(e => e.EncryptedDataKey)
            .HasColumnType("bytea")
            .HasComment("Data Encryption Key encrypted using AWS KMS.");
        entity.Property(e => e.KeyIdentifier)
            .HasMaxLength(200)
            .HasComment("AWS KMS key ARN or alias used to encrypt the Data Encryption Key.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the credential is active and available for use.");
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne(e => e.ProviderType)
            .WithMany()
            .HasForeignKey(e => e.CredentialProviderTypeId)
            .HasPrincipalKey(p => p.ProviderTypeId)
            .HasConstraintName("FK_FgsCredential_GloCredentialProviderTypeCache_ProviderTypeId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCredential_Tenant_Company");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderTypeId })
            .HasDatabaseName("IX_FgsCredential_Tenant_Company_ProviderType");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderTypeId })
            .IsUnique()
            .HasDatabaseName("UQ_FgsCredential_Tenant_Company_ProviderType");
    }
}
