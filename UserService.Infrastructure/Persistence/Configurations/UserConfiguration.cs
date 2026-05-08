using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence.Converters;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .HasColumnType("citext")
            .IsRequired();

        builder.Property(e => e.DisplayName);
        builder.Property(e => e.Status)
            .HasConversion(v => PgEnumMapper.UserStatusToPg(v), v => PgEnumMapper.UserStatusFromPg(v))
            .IsRequired();

        builder.Property(e => e.Role)
            .HasConversion(v => PgEnumMapper.UserRoleToPg(v), v => PgEnumMapper.UserRoleFromPg(v))
            .IsRequired();

        builder.Property(e => e.CompanyId).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasIndex(e => e.TenantId).HasDatabaseName("IxUsersTenant");

        builder.HasIndex(e => new { e.TenantId, e.Email }).IsUnique().HasDatabaseName("IxUsersTenantEmail");

        builder.HasMany(e => e.Invites)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.AuthIdentities)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
