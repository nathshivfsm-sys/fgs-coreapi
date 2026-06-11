using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementVisitConfiguration : IEntityTypeConfiguration<FgsServiceAgreementVisit>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreementVisit> entity)
    {
        entity.ToTable(
            "FgsServiceAgreementVisit",
            t =>
            {
                t.HasComment(
                    "Stores contractually required maintenance visits generated from a service agreement. Visits may later generate work orders to perform the required maintenance service.");
                t.HasCheckConstraint("CK_FgsServiceAgreementVisit_Status", "\"ServiceAgreementVisitStatusId\" IN (1, 2, 3, 4, 5)");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ServiceAgreementId).HasComment("Parent service agreement identifier.");
        entity.Property(e => e.VisitNumber).HasComment("Sequential visit number within the service agreement.");
        entity.Property(e => e.JobTypeId).HasComment("Job type used when generating a work order from the service agreement visit.");
        entity.Property(e => e.ExpectedServiceDate).HasComment("Expected date the maintenance service should be performed according to the agreement.");
        entity.Property(e => e.ServiceAgreementVisitStatusId)
            .HasComment("Visit lifecycle status. Values: 1=Pending, 2=WorkOrderCreated, 3=Completed, 4=Skipped, 5=Cancelled.");
        entity.Property(e => e.WorkOrderId).HasComment("Generated work order identifier associated with the visit.");
        entity.Property(e => e.ExternalWorkOrderNumber).HasMaxLength(100)
            .HasComment("Work order number from a legacy or external system used during data migration.");
        entity.Property(e => e.CompletedOn).HasColumnType("timestamptz").HasComment("Date and time the maintenance visit was completed.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasOne(e => e.ServiceAgreement)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementId)
            .HasConstraintName("FK_FgsServiceAgreementVisit_ServiceAgreement")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId, e.VisitNumber })
            .IsUnique()
            .HasDatabaseName("UQ_FgsServiceAgreementVisit_Agreement_VisitNumber");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreementVisit_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsServiceAgreementVisit_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeId }).HasDatabaseName("IX_FgsServiceAgreementVisit_JobTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementVisitStatusId })
            .HasDatabaseName("IX_FgsServiceAgreementVisit_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ExpectedServiceDate })
            .HasDatabaseName("IX_FgsServiceAgreementVisit_ExpectedServiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId }).HasDatabaseName("IX_FgsServiceAgreementVisit_WorkOrderId");
    }
}
