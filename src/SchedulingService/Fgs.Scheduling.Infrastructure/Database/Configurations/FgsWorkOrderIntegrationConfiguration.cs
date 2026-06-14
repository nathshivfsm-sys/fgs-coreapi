using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsWorkOrderIntegrationConfiguration : IEntityTypeConfiguration<FgsWorkOrderIntegration>
{
    public void Configure(EntityTypeBuilder<FgsWorkOrderIntegration> entity)
    {
        entity.ToTable(
            "FgsWorkOrderIntegration",
            t => t.HasComment(
                "Stores externally received work orders and their raw payloads before they are reviewed and booked into dispatch."));

        entity.HasKey(e => e.Id).HasName("PK_FgsWorkOrderIntegration");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.WorkOrderId).HasComment("Dispatch work order created from this integration record.");
        entity.Property(e => e.IntegrationName).HasMaxLength(100).IsRequired()
            .HasComment("Integration source such as Corrigo, ServiceChannel, Verizon, AHS, etc.");
        entity.Property(e => e.ExternalId).HasMaxLength(100).IsRequired()
            .HasComment("Primary identifier from the external system.");
        entity.Property(e => e.ExternalWorkOrderNumber).HasMaxLength(100)
            .HasComment("External work order number visible to users in the external system.");
        entity.Property(e => e.ReceivedOn).IsRequired().HasColumnType("timestamptz")
            .HasComment("Date and time the payload was received from the external system.");
        entity.Property(e => e.Status).HasMaxLength(50).IsRequired().HasDefaultValue("Received")
            .HasComment("Current processing status of the imported work order.");
        entity.Property(e => e.Payload).HasColumnType("jsonb").IsRequired()
            .HasComment("Raw JSON payload received from the external system.");
        entity.Property(e => e.ProcessedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was processed or booked into dispatch.");
        entity.Property(e => e.ProcessedBy).HasMaxLength(100).HasComment("User that processed or booked the work order.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100).HasComment("User who last updated the record.");

        entity.HasOne<FgsWorkOrder>()
            .WithMany()
            .HasForeignKey(e => e.WorkOrderId)
            .HasConstraintName("FK_FgsWorkOrderIntegration_WorkOrder")
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsWorkOrderIntegration_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId }).HasDatabaseName("IX_FgsWorkOrderIntegration_WorkOrderId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Status }).HasDatabaseName("IX_FgsWorkOrderIntegration_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ReceivedOn }).HasDatabaseName("IX_FgsWorkOrderIntegration_ReceivedOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IntegrationName, e.ExternalId })
            .IsUnique()
            .HasDatabaseName("UQ_FgsWorkOrderIntegration_External");
    }
}
