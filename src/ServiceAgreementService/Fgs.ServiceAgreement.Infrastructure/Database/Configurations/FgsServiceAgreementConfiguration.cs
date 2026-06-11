using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementConfiguration : IEntityTypeConfiguration<FgsServiceAgreement>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreement> entity)
    {
        entity.ToTable(
            "FgsServiceAgreement",
            t =>
            {
                t.HasComment(
                    "Stores recurring maintenance agreements, membership plans, preventive maintenance contracts, and service contracts. Visit schedules and billing schedules are generated into separate tables.");
                t.HasCheckConstraint("CK_FgsServiceAgreement_Status", "\"ServiceAgreementStatusId\" IN (1, 2, 3, 4)");
                t.HasCheckConstraint("CK_FgsServiceAgreement_EndDate", "\"EndDate\" >= \"StartDate\"");
                t.HasCheckConstraint(
                    "CK_FgsServiceAgreement_Discounts",
                    "\"LaborDiscountPercent\" BETWEEN 0 AND 100 AND \"MaterialDiscountPercent\" BETWEEN 0 AND 100");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.AgreementNumber).HasMaxLength(50).IsRequired()
            .HasComment("User visible service agreement number.");
        entity.Property(e => e.CustomerId).HasComment("Customer that owns the agreement.");
        entity.Property(e => e.CustomerLocationId).HasComment("Service location covered by the agreement.");
        entity.Property(e => e.EstimateId).HasComment("Estimate that was accepted and converted into this service agreement.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired().HasComment("Agreement name.");
        entity.Property(e => e.Description).HasColumnType("text").HasComment("Internal agreement description.");
        entity.Property(e => e.Break1Id).HasComment("Business Unit classification for the agreement.");
        entity.Property(e => e.Break2Id).HasComment("Secondary operational classification for the agreement.");
        entity.Property(e => e.JobTypeId).HasComment("Job type associated with the agreement.");
        entity.Property(e => e.StartDate).HasComment("Agreement coverage start date.");
        entity.Property(e => e.EndDate).HasComment("Agreement coverage end date.");
        entity.Property(e => e.ServiceAgreementStatusId).HasComment("Agreement lifecycle status. Values: 1=Draft, 2=Active, 3=Expired, 4=Cancelled.");
        entity.Property(e => e.VisitFrequencyId).HasComment("Frequency used to generate service agreement visit schedules.");
        entity.Property(e => e.BillingFrequencyId).HasComment("Frequency used to generate billing schedules.");
        entity.Property(e => e.ContractAmount).HasColumnType("numeric(18,2)").HasComment("Total contract value. Billing schedule amounts are calculated from contract amount and billing frequency.");
        entity.Property(e => e.LaborDiscountPercent).HasColumnType("numeric(5,2)").HasDefaultValue(0m)
            .HasComment("Labor discount percentage available under the agreement.");
        entity.Property(e => e.MaterialDiscountPercent).HasColumnType("numeric(5,2)").HasDefaultValue(0m)
            .HasComment("Material discount percentage available under the agreement.");
        entity.Property(e => e.AutoRenew).HasDefaultValue(false).HasComment("Indicates whether the agreement should automatically renew at expiration.");
        entity.Property(e => e.RenewedByServiceAgreementId)
            .HasComment("Identifier of the agreement created when this agreement was renewed. Null indicates the agreement has not yet been renewed.");
        entity.Property(e => e.SoldDate).HasComment("Date the agreement was sold.");
        entity.Property(e => e.SoldByEmployeeId).HasComment("Employee that sold the agreement.");
        entity.Property(e => e.ActivatedOn).HasColumnType("timestamptz").HasComment("Date and time the agreement became active.");
        entity.Property(e => e.CancelledOn).HasColumnType("timestamptz").HasComment("Date and time the agreement was cancelled.");
        entity.Property(e => e.ExternalEntityId).HasMaxLength(200).HasComment("External system identifier.");
        entity.Property(e => e.ExternalVersion).HasMaxLength(100).HasComment("External synchronization token or version.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AgreementNumber })
            .IsUnique()
            .HasDatabaseName("UQ_FgsServiceAgreement_AgreementNumber");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreement_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId }).HasDatabaseName("IX_FgsServiceAgreement_CustomerId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerLocationId }).HasDatabaseName("IX_FgsServiceAgreement_CustomerLocationId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementStatusId }).HasDatabaseName("IX_FgsServiceAgreement_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EndDate }).HasDatabaseName("IX_FgsServiceAgreement_EndDate");
    }
}
