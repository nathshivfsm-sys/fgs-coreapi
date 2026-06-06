using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsInvitationConfiguration : IEntityTypeConfiguration<FgsInvitation>
{
    public void Configure(EntityTypeBuilder<FgsInvitation> entity)
    {
        entity.ToTable("FgsInvitation");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.TokenHash);
        entity.HasIndex(e => new { e.TenantId, e.Email, e.Status });
        entity.Property(e => e.Email).HasMaxLength(300);
        entity.Property(e => e.TokenHash).HasMaxLength(128);
        entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.ExpiresAtUtc).HasColumnType("timestamptz");
        entity.Property(e => e.AcceptedAtUtc).HasColumnType("timestamptz");
        entity.HasOne(e => e.User)
            .WithMany(u => u.Invitations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
