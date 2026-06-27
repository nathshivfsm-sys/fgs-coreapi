using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal sealed class FgsWorkOrderConfiguration : IEntityTypeConfiguration<FgsWorkOrder>
{
    public void Configure(EntityTypeBuilder<FgsWorkOrder> entity)
    {
        entity.ToTable(
            "FgsWorkOrder",
            t => t.HasComment(
                "Master work order record representing a customer service request that can be scheduled through one or more appointments."));

        entity.HasKey(e => e.Id).HasName("PK_FgsWorkOrder");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Primary key.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.WorkOrderNumber).HasMaxLength(50).IsRequired()
            .HasComment("Unique work order number within tenant and company.");
        entity.Property(e => e.ProjectId).HasComment("Optional project identifier. References project service; no FK by design.");
        entity.Property(e => e.CustomerId).HasComment("Customer identifier. References CRM service; no FK by design.");
        entity.Property(e => e.LocationId).HasComment("Service location identifier. References CRM service; no FK by design.");
        entity.Property(e => e.ServiceAgreementId).HasComment("Service agreement identifier. References service agreement service; no FK by design.");
        entity.Property(e => e.ServiceAgreementVisitId).HasComment("Service agreement visit identifier. References service agreement service; no FK by design.");
        entity.Property(e => e.Break1Id).HasComment("Primary break classification identifier. References setup service; no FK by design.");
        entity.Property(e => e.Break2Id).HasComment("Secondary break classification identifier. References setup service; no FK by design.");
        entity.Property(e => e.JobTypeId).HasComment("Job type identifier. References setup.FgsJobType through application logic; no FK by design.");
        entity.Property(e => e.PriorityId).HasComment("Priority identifier. References setup service; no FK by design.");
        entity.Property(e => e.WorkOrderStatusId).HasComment("Work order status. New, Started, Completed, or Cancelled.");
        entity.Property(e => e.WorkOrderResolutionId).HasComment("Completion or cancellation reason identifier. References setup service; no FK by design.");
        entity.Property(e => e.TimeSlotId).HasComment("Promised time window. References setup.FgsSetupTimeSlot through application logic; no FK by design.");
        entity.Property(e => e.CustomerPO).HasMaxLength(100).HasComment("Customer purchase order reference.");
        entity.Property(e => e.PersonCalling).HasMaxLength(200).HasComment("Name of person who called to request service.");
        entity.Property(e => e.PersonCallingPhoneNumber).HasMaxLength(30).HasComment("Phone number of person who called.");
        entity.Property(e => e.ContactPerson).HasMaxLength(200).HasComment("Onsite contact person name.");
        entity.Property(e => e.ContactPersonPhoneNumber).HasMaxLength(30).HasComment("Onsite contact person phone number.");
        entity.Property(e => e.ProblemDescription).HasColumnType("text").HasComment("Customer problem description.");
        entity.Property(e => e.Note).HasColumnType("text").HasComment("Special instructions for technicians.");
        entity.Property(e => e.MaterialPricingMatrixId).HasComment("Material pricing matrix identifier. References setup service; no FK by design.");
        entity.Property(e => e.LaborPricingMatrixId).HasComment("Labor pricing matrix identifier. References setup service; no FK by design.");
        entity.Property(e => e.OtherPricingMatrixId).HasComment("Other pricing matrix identifier. References setup service; no FK by design.");
        entity.Property(e => e.PaymentMethodId).HasComment("Payment method identifier. References setup service; no FK by design.");
        entity.Property(e => e.EstimatedHours).HasColumnType("numeric(8,2)").HasComment("Estimated hours for the work order.");
        entity.Property(e => e.RequestedOn).IsRequired().HasColumnType("timestamptz").HasComment("Date and time the work order was requested.");
        entity.Property(e => e.StartDate).HasColumnType("timestamptz").HasComment("Work order start date and time.");
        entity.Property(e => e.EndDate).HasColumnType("timestamptz").HasComment("Work order end date and time.");
        entity.Property(e => e.Source).HasMaxLength(50).HasComment("Source of the work order such as Manual, Portal, API, Corrigo, ServiceChannel, Verizon, AHS, etc.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsWorkOrder_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId }).HasDatabaseName("IX_FgsWorkOrder_Customer");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LocationId }).HasDatabaseName("IX_FgsWorkOrder_Location");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ProjectId }).HasDatabaseName("IX_FgsWorkOrder_Project");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId }).HasDatabaseName("IX_FgsWorkOrder_ServiceAgreement");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderStatusId }).HasDatabaseName("IX_FgsWorkOrder_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeId }).HasDatabaseName("IX_FgsWorkOrder_JobType");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PriorityId }).HasDatabaseName("IX_FgsWorkOrder_Priority");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TimeSlotId }).HasDatabaseName("IX_FgsWorkOrder_TimeSlot");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RequestedOn }).HasDatabaseName("IX_FgsWorkOrder_RequestedOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Source }).HasDatabaseName("IX_FgsWorkOrder_Source");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderNumber })
            .IsUnique()
            .HasDatabaseName("UQ_FgsWorkOrder_WorkOrderNumber");
    }
}
