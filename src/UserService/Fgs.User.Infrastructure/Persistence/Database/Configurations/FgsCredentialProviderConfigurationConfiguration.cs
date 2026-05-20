using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsCredentialProviderConfigurationConfiguration
    : IEntityTypeConfiguration<FgsCredentialProviderConfiguration>
{
    public void Configure(EntityTypeBuilder<FgsCredentialProviderConfiguration> entity)
    {
        entity.ToTable("FgsCredentialProviderConfiguration");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.ConfigurationKey).HasMaxLength(200);
        entity.Property(e => e.Environment).HasMaxLength(50);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName("FK_FgsCredProvCfg_Company")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<FgsCredentialProvider>()
            .WithMany()
            .HasForeignKey(e => e.CredentialProviderId)
            .HasConstraintName("FK_FgsCredProvCfg_Provider")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new
            {
                e.TenantId,
                e.CompanyId,
                e.CredentialProviderId,
                e.ConfigurationKey,
                e.Environment
            })
            .IsUnique()
            .HasDatabaseName("UQ_FgsCredentialProviderConfiguration");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCredProvCfg_Tenant_Company");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId })
            .HasDatabaseName("IX_FgsCredProvCfg_Tenant_Company_Prov");
    }
}
