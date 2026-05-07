using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Infrastructure.Persistence.Converters;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class InviteConfiguration : IEntityTypeConfiguration<Invite>
{
    public void Configure(EntityTypeBuilder<Invite> builder)
    {
        builder.ToTable("invite");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.InvitedEmail)
            .HasColumnType("citext")
            .IsRequired();

        builder.Property(e => e.TokenHash)
            .HasColumnType("bytea")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion(v => PgEnumMapper.InviteStatusToPg(v), v => PgEnumMapper.InviteStatusFromPg(v))
            .IsRequired();

        builder.Property(e => e.CompanyId).IsRequired();
        builder.Property(e => e.ExpiresAt).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasIndex(e => e.TokenHash).HasDatabaseName("ix_invite_token_hash");
        builder.HasIndex(e => e.UserId).HasDatabaseName("ix_invite_user");

        builder.HasIndex(e => e.TenantId)
            .HasDatabaseName("ix_invite_pending")
            .HasFilter("status = 'pending'");
    }
}
