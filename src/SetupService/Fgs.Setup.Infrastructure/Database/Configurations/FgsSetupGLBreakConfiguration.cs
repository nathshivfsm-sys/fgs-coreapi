using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupGLBreakConfiguration : IEntityTypeConfiguration<FgsSetupGLBreak>
{
    public void Configure(EntityTypeBuilder<FgsSetupGLBreak> entity)
    {
        entity.ToTable("FgsSetupGLBreak", t =>
        {
            t.HasComment(
                "Stores GL break configuration for financial reporting segmentation by trade, division, branch, or organizational unit.");
            t.HasCheckConstraint("CK_FgsSetupGLBreak_BreakLevel", "\"BreakLevel\" IN (1, 2)");
        });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Surrogate primary key.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code, e.BreakLevel })
            .HasName("UQ_FgsSetupGLBreak");

        entity.Property(e => e.Code)
            .HasColumnType("text")
            .HasComment("Unique GL break code within tenant, company, and break level scope.");

        entity.Property(e => e.Name)
            .HasColumnType("text")
            .HasComment("Display name of the GL break.");

        entity.Property(e => e.BreakLabel)
            .HasColumnType("text")
            .HasComment("Optional label displayed in UI and financial documents.");

        entity.Property(e => e.BreakLevel)
            .HasColumnType("smallint")
            .HasComment("Break hierarchy level. Allowed values: 1 or 2.");

        entity.Property(e => e.LogoFileId)
            .HasComment("Optional reference to uploaded logo file in FgsFile.");

        entity.Property(e => e.AddressId)
            .HasComment("Optional reference to branch or break address in FgsLocation.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasComment("UTC timestamp when the record was created.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("UTC timestamp when the record was last updated.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User or process that created the record.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User or process that last updated the record.");

        entity.Property(e => e.IsActive)
            .HasComment("Indicates whether the GL break is active.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BreakLevel })
            .HasDatabaseName("IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel");

        entity.HasIndex(e => e.AddressId)
            .HasDatabaseName("IX_FgsSetupGLBreak_AddressId");

        entity.HasIndex(e => e.LogoFileId)
            .HasDatabaseName("IX_FgsSetupGLBreak_LogoFileId");    }
}
