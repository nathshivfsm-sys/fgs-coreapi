using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloPermissionConfiguration : IEntityTypeConfiguration<GloPermission>
{
    public void Configure(EntityTypeBuilder<GloPermission> entity)
    {
        entity.ToTable("GloPermission", t =>
        {
            t.HasComment(
                "Global catalog of permissions supported by the FGS platform. Permissions define the actions that can be assigned to security roles.");
            t.HasCheckConstraint(
                "CK_GloPermission_Name_NotEmpty",
                "length(trim(\"Name\")) > 0");
            t.HasCheckConstraint(
                "CK_GloPermission_PermissionCode_NotEmpty",
                "length(trim(\"PermissionCode\")) > 0");
        });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the global permission.");

        entity.HasAlternateKey(e => e.PermissionCode).HasName("UX_GloPermission_PermissionCode");

        entity.Property(e => e.PermissionCode)
            .HasMaxLength(100)
            .HasComment(
                "Unique system identifier for the permission. Used internally by the application and should not be changed after creation.");
        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .HasComment("Display name of the permission shown to administrators.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining what the permission allows the user to do.");
        entity.Property(e => e.DisplayOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)0)
            .HasComment("Controls the display order of permissions within the permission management interface.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the permission is currently available for assignment to roles.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())")
            .HasComment("UTC timestamp when the global permission was created.");

        entity.HasIndex(e => e.Name).HasDatabaseName("IX_GloPermission_Name");
    }
}
