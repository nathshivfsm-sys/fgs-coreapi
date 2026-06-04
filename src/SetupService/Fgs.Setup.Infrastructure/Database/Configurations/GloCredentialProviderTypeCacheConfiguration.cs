using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class GloCredentialProviderTypeCacheConfiguration : IEntityTypeConfiguration<GloCredentialProviderTypeCache>
{
    public void Configure(EntityTypeBuilder<GloCredentialProviderTypeCache> entity)
    {
        entity.ToTable(
            "GloCredentialProviderTypeCache",
            t => t.HasComment(
                "Local cache of globally defined credential providers used to eliminate cross-schema dependencies."));

        entity.HasKey(e => e.ProviderTypeId)
            .HasName("PK_GloCredentialProviderTypeCache");

        entity.Property(e => e.ProviderTypeId)
            .ValueGeneratedNever()
            .HasComment("Identifier from glo.GloCredentialProviderType.Id.");
        entity.Property(e => e.ProviderCode).HasMaxLength(50)
            .HasComment("System unique provider code used by application logic and integration services.");
        entity.Property(e => e.ProviderName).HasMaxLength(200)
            .HasComment("User friendly provider name displayed in setup screens.");
        entity.Property(e => e.ConfigurationSchema).HasColumnType("jsonb")
            .HasComment("JSON schema used by the UI to dynamically render provider configuration fields and perform validation.");
        entity.Property(e => e.IsActive)
            .HasComment("Indicates whether the provider can be selected for new credential configurations.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Timestamp of the most recent synchronization from glo.GloCredentialProviderType.");

        entity.HasIndex(e => e.ProviderCode)
            .IsUnique()
            .HasDatabaseName("UQ_GloCredentialProviderTypeCache_ProviderCode");

        entity.HasIndex(e => e.ProviderName)
            .HasDatabaseName("IX_GloCredentialProviderTypeCache_ProviderName");
    }
}
