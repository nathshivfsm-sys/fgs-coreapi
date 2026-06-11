using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloLeadDisqualificationReasonConfiguration : IEntityTypeConfiguration<GloLeadDisqualificationReason>
{
    public void Configure(EntityTypeBuilder<GloLeadDisqualificationReason> entity)
    {
        entity.ToTable(
            "GloLeadDisqualificationReason",
            t => t.HasComment(
                "Master list of lead disqualification reasons used to seed tenant-specific records into setup.FgsLeadDisqualificationReason."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.ReasonCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique business code for the disqualification reason.");
        entity.Property(e => e.ReasonName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the reason.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which reasons are displayed.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the reason is available for seeding and use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => e.ReasonCode)
            .IsUnique()
            .HasDatabaseName("UX_GloLeadDisqualificationReason_ReasonCode");

        entity.HasIndex(e => e.ReasonName)
            .IsUnique()
            .HasDatabaseName("UX_GloLeadDisqualificationReason_ReasonName");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloLeadDisqualificationReason_DisplayOrder");
    }
}
