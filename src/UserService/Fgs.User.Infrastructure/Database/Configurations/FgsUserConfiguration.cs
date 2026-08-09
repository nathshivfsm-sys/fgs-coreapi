using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsUserConfiguration : IEntityTypeConfiguration<FgsUser>
{
    public void Configure(EntityTypeBuilder<FgsUser> entity)
    {
        entity.ToTable("FgsUser", t => t.HasCheckConstraint(
            "CK_FgsUser_AuthenticationMethod",
            "\"AuthenticationMethod\" IN (1, 2, 3, 4, 5)"));
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Email })
            .IsUnique()
            .HasDatabaseName("IX_FgsUser_TenantId_CompanyId_Email")
            .HasFilter("\"IsDeleted\" = false");
        entity.Property(e => e.Email).HasMaxLength(300);
        entity.Property(e => e.DisplayName).HasMaxLength(200);
        entity.Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .HasComment(
                "Primary phone number used for SMS notifications and one-time password (OTP) verification when multi-factor authentication (MFA) using SMS is enabled.");
        entity.Property(e => e.EntraObjectId).HasMaxLength(100);
        entity.Property(e => e.AuthenticationMethod)
            .HasConversion<short>()
            .IsRequired()
            .HasDefaultValue(AuthenticationMethod.PasswordOrEmailOtp);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
