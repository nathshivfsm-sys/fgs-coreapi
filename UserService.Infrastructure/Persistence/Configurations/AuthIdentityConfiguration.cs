using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class AuthIdentityConfiguration : IEntityTypeConfiguration<AuthIdentity>
{
    public void Configure(EntityTypeBuilder<AuthIdentity> builder)
    {
        builder.ToTable("auth_identity");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Issuer).IsRequired();
        builder.Property(e => e.ObjectId).IsRequired();
        builder.Property(e => e.Subject);
        builder.Property(e => e.EmailSnapshot).HasColumnType("citext");
        builder.Property(e => e.LinkedAt).IsRequired();

        builder.HasIndex(e => new { e.Issuer, e.ObjectId }).IsUnique();
        builder.HasIndex(e => e.UserId).HasDatabaseName("ix_auth_identity_user");
    }
}
