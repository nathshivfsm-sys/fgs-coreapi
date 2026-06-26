using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloCredentialConfiguration : IEntityTypeConfiguration<GloCredential>
{
    public void Configure(EntityTypeBuilder<GloCredential> entity)
    {
        entity.ToTable("GloCredential");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.CredentialName).HasMaxLength(200);
        entity.Property(e => e.Description).HasMaxLength(500);
        entity.Property(e => e.CredentialData).HasColumnType("bytea");
        entity.Property(e => e.EncryptedDataKey).HasColumnType("bytea");
        entity.Property(e => e.KeyIdentifier).HasMaxLength(200);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasOne(e => e.ProviderType)
            .WithMany(p => p.Credentials)
            .HasForeignKey(e => e.CredentialProviderTypeId)
            .HasConstraintName("FK_GloCredential_ProviderType")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
