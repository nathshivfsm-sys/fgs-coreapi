using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsApiSecretConfiguration : IEntityTypeConfiguration<FgsApiSecret>
{
    public void Configure(EntityTypeBuilder<FgsApiSecret> entity)
    {
        entity.ToTable(
            "FgsApiSecret",
            t => t.HasComment(
                "Stores hashed API secrets associated with API clients. Supports secret rotation, expiration, revocation and auditing."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("User-friendly name used to identify the secret, such as Production, Sandbox or July 2026 Rotation.");
        entity.Property(e => e.SecretHash)
            .HasMaxLength(500)
            .HasComment("Cryptographic hash of the API secret. The original secret is never stored and cannot be recovered.");
        entity.Property(e => e.ExpiresOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the secret expires. NULL indicates the secret does not expire.");
        entity.Property(e => e.LastUsedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the secret was most recently used for successful authentication.");
        entity.Property(e => e.RevokedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the secret was revoked.");
        entity.Property(e => e.RevokedBy)
            .HasMaxLength(100)
            .HasComment("User or system that revoked the secret.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the secret is currently valid for API authentication.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the secret was created.");
        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("User or system that created the secret.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsApiClientId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsApiSecret_TenantId_CompanyId_Client_Name");
        entity.HasIndex(e => e.FgsApiClientId)
            .HasDatabaseName("IX_FgsApiSecret_FgsApiClientId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsApiSecret_TenantId_CompanyId");

        entity.HasOne(e => e.FgsApiClient)
            .WithMany(c => c.Secrets)
            .HasForeignKey(e => e.FgsApiClientId)
            .HasConstraintName("FK_FgsApiSecret_FgsApiClient")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
