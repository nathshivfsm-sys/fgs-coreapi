using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsTenantServiceSetupConfiguration : IEntityTypeConfiguration<FgsTenantServiceSetup>
{
    public void Configure(EntityTypeBuilder<FgsTenantServiceSetup> entity)
    {
        entity.ToTable("FgsTenantServiceSetup");
        entity.HasKey(e => new { e.TenantId, e.CompanyId });
        entity.Property(e => e.TenantId).HasColumnOrder(0);
        entity.Property(e => e.CompanyId).HasColumnOrder(1);
        entity.Property(e => e.BillHoursFromDispatchOrArrive).HasMaxLength(20);
        entity.Property(e => e.InvoiceNumberPrefix).HasMaxLength(20);
        entity.Property(e => e.QuoteNumberPrefix).HasMaxLength(20);
        entity.Property(e => e.PONumberPrefix).HasMaxLength(20);
        entity.Property(e => e.WorkOrderNumberPrefix).HasMaxLength(20);
        entity.Property(e => e.InvoiceBatchNumberFormat).HasMaxLength(200);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName("FK_FgsTenantServiceSetup_Company")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<GloTimeCardOption>()
            .WithMany()
            .HasForeignKey(e => e.GloTimeCardOptionId)
            .HasConstraintName("FK_FgsTenantServiceSetup_TimeCardOption")
            .OnDelete(DeleteBehavior.Restrict);
        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_FgsTenantServiceSetup_WorkLocationRadius",
                "\"WorkLocationRadiusForAutoArrive\" IS NULL OR \"WorkLocationRadiusForAutoArrive\" >= 0");
            t.HasCheckConstraint(
                "CK_FgsTenantServiceSetup_OTRange",
                "\"OTStartTime\" IS NULL OR \"OTEndTime\" IS NULL OR \"OTEndTime\" > \"OTStartTime\"");
            t.HasCheckConstraint(
                "CK_FgsTenantServiceSetup_DTRange",
                "\"DTStartTime\" IS NULL OR \"DTEndTime\" IS NULL OR \"DTEndTime\" > \"DTStartTime\"");
        });
    }
}
