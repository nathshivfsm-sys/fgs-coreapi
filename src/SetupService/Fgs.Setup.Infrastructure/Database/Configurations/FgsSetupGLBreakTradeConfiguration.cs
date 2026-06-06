using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupGLBreakTradeConfiguration : IEntityTypeConfiguration<FgsSetupGLBreakTrade>
{
    public void Configure(EntityTypeBuilder<FgsSetupGLBreakTrade> entity)
    {
        entity.ToTable("FgsSetupGLBreakTrade", t =>
            t.HasComment("Stores trade-to-GL-break mappings used for financial segmentation and reporting."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Surrogate primary key.");

        entity.Property(e => e.TenantId)
            .HasComment("Owning tenant identifier.");

        entity.Property(e => e.CompanyId)
            .HasComment("Tenant-scoped company number.");

        entity.Property(e => e.GLBreakId)
            .HasComment("Reference to the associated GL break configuration.");

        entity.Property(e => e.TradeCode)
            .HasMaxLength(50)
            .HasComment(
                "Technician or operational trade code associated with the GL break such as HVAC, Plumbing, Electrical, or Drain.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("UTC timestamp when the record was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User or process that created the record.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.GLBreakId, e.TradeCode })
            .HasName("UQ_FgsSetupGLBreakTrade");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSetupGLBreakTrade_TenantId_CompanyId");

        entity.HasIndex(e => e.GLBreakId)
            .HasDatabaseName("IX_FgsSetupGLBreakTrade_GLBreakId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TradeCode })
            .HasDatabaseName("IX_FgsSetupGLBreakTrade_TradeCode");

        entity.HasOne(e => e.GLBreak)
            .WithMany(b => b.Trades)
            .HasForeignKey(e => e.GLBreakId)
            .HasConstraintName("FK_FgsSetupGLBreakTrade_FgsSetupGLBreak_GLBreakId")
            .OnDelete(DeleteBehavior.Cascade);    }
}
