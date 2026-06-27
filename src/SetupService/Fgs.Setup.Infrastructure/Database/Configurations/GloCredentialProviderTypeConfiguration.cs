using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloCredentialProviderTypeConfiguration : IEntityTypeConfiguration<GloCredentialProviderType>
{
    public void Configure(EntityTypeBuilder<GloCredentialProviderType> entity)
    {
        entity.ToTable(
            "GloCredentialProviderType",
            t => t.HasComment(
                "Master list of supported credential providers and integrations available within the FSM platform."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.ProviderCode)
            .HasMaxLength(50)
            .HasComment("System unique code used by application logic and integration services.");
        entity.Property(e => e.ProviderName)
            .HasMaxLength(200)
            .HasComment("User friendly provider name displayed in setup screens.");
        entity.Property(e => e.ConfigurationSchema)
            .HasColumnType("jsonb")
            .HasComment(
                "JSON schema used by the UI to dynamically render provider configuration fields and perform validation.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the provider can be selected for new credential configurations.");
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => e.ProviderCode)
            .IsUnique()
            .HasDatabaseName("UQ_GloCredentialProviderType_ProviderCode");
    }
}
