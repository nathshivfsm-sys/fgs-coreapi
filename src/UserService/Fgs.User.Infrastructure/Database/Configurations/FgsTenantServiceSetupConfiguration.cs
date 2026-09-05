using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsTenantServiceSetupConfiguration : IEntityTypeConfiguration<FgsTenantServiceSetup>
{
    public void Configure(EntityTypeBuilder<FgsTenantServiceSetup> entity)
    {
        entity.ToTable(
            "FgsTenantServiceSetup",
            t =>
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
                t.HasCheckConstraint(
                    "CK_FgsTenantServiceSetup_TimeCardOptionId",
                    "\"TimeCardOptionId\" IN (1, 2, 3, 4)");
                t.HasCheckConstraint(
                    "CK_FgsTenantServiceSetup_EstimateRevisionCreationMode",
                    "\"EstimateRevisionCreationMode\" IN ('OnDemand', 'OnPostSignatureChange')");
            });
        entity.HasKey(e => new { e.TenantId, e.CompanyId });
        entity.Property(e => e.TenantId).HasColumnOrder(0);
        entity.Property(e => e.CompanyId).HasColumnOrder(1);
        entity.Property(e => e.TimeCardOptionId)
            .HasConversion<short>()
            .IsRequired()
            .HasComment(
                "Determines the technician time tracking workflow. Valid values: 1 = No formal technician time tracking workflow, 2 = Technician manually checks in and checks out, 3 = Tracks dispatch, arrival, and completion timestamps, 4 = Tracks dispatch, arrival, completion, and documentation time timestamps.");
        entity.Property(e => e.BillHoursFromDispatchOrArrive).HasMaxLength(20);
        entity.Property(e => e.InvoiceNumberPrefix).HasMaxLength(20);
        entity.Property(e => e.QuoteNumberPrefix).HasMaxLength(20);
        entity.Property(e => e.PONumberPrefix).HasMaxLength(20);
        entity.Property(e => e.WorkOrderNumberPrefix).HasMaxLength(20);
        entity.Property(e => e.InvoiceBatchNumberFormat).HasMaxLength(200);
        entity.Property(e => e.EstimateRevisionCreationMode)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue(EstimateRevisionCreationModes.OnDemand)
            .HasComment(
                "Controls when estimate revisions are created. Valid values: OnDemand = user manually creates a revision; OnPostSignatureChange = automatically creates a revision when a signed estimate is changed.");
        entity.Property(e => e.AutoBookMaintenanceScheduleCalls)
            .HasDefaultValue(false)
            .HasComment(
                "Controls whether maintenance schedule calls are automatically booked. TRUE = maintenance schedule calls are automatically booked; FALSE = maintenance schedule calls must be booked manually.");
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
