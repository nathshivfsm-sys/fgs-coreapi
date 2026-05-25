using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsSetupGLBreakConfiguration : IEntityTypeConfiguration<FgsSetupGLBreak>
{
    public void Configure(EntityTypeBuilder<FgsSetupGLBreak> entity)
    {
        entity.ToTable("FgsSetupGLBreak");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsSetupGLBreak_FgsTenantCompany_TenantId_CompanyId");
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code, e.BreakLevel })
            .HasName("UQ_FgsSetupGLBreak");
        entity.Property(e => e.Code).HasColumnType("text");
        entity.Property(e => e.Name).HasColumnType("text");
        entity.Property(e => e.BreakLabel).HasColumnType("text");
        entity.Property(e => e.BreakLevel).HasColumnType("smallint");
        entity.Property(e => e.Trades).HasColumnType("text[]");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.ToTable(t => t.HasCheckConstraint("CK_FgsSetupGLBreak_BreakLevel", "\"BreakLevel\" IN (1, 2)"));
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BreakLevel })
            .HasDatabaseName("IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel");
        entity.HasOne<FgsFile>()
            .WithMany()
            .HasForeignKey(e => e.LogoFileId)
            .HasConstraintName("FK_FgsSetupGLBreak_FgsFile_LogoFileId")
            .OnDelete(DeleteBehavior.SetNull);
        entity.HasOne<FgsLocation>()
            .WithMany()
            .HasForeignKey(e => e.AddressId)
            .HasConstraintName("FK_FgsSetupGLBreak_FgsLocation_AddressId")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
