using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementNoteConfiguration : IEntityTypeConfiguration<FgsServiceAgreementNote>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreementNote> entity)
    {
        entity.ToTable(
            "FgsServiceAgreementNote",
            t =>
            {
                t.HasComment(
                    "Stores notes related to service agreements, service agreement visits, and service agreement billing schedules. A note belongs to exactly one parent entity.");
                t.HasCheckConstraint(
                    "CK_FgsServiceAgreementNote_Parent",
                    """
                    ("ServiceAgreementId" IS NOT NULL AND "ServiceAgreementVisitId" IS NULL AND "ServiceAgreementBillingScheduleId" IS NULL)
                    OR ("ServiceAgreementId" IS NULL AND "ServiceAgreementVisitId" IS NOT NULL AND "ServiceAgreementBillingScheduleId" IS NULL)
                    OR ("ServiceAgreementId" IS NULL AND "ServiceAgreementVisitId" IS NULL AND "ServiceAgreementBillingScheduleId" IS NOT NULL)
                    """);
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ServiceAgreementId).HasComment("Parent service agreement identifier.");
        entity.Property(e => e.ServiceAgreementVisitId).HasComment("Parent service agreement visit identifier.");
        entity.Property(e => e.ServiceAgreementBillingScheduleId).HasComment("Parent service agreement billing schedule identifier.");
        entity.Property(e => e.NoteTypeId).HasComment("Optional note classification identifier.");
        entity.Property(e => e.Note).HasColumnType("text").IsRequired().HasComment("Note text.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasOne(e => e.ServiceAgreement)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementId)
            .HasConstraintName("FK_FgsServiceAgreementNote_ServiceAgreement")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.ServiceAgreementVisit)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementVisitId)
            .HasConstraintName("FK_FgsServiceAgreementNote_ServiceAgreementVisit")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.ServiceAgreementBillingSchedule)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementBillingScheduleId)
            .HasConstraintName("FK_FgsServiceAgreementNote_BillingSchedule")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreementNote_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsServiceAgreementNote_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementVisitId })
            .HasDatabaseName("IX_FgsServiceAgreementNote_ServiceAgreementVisitId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementBillingScheduleId })
            .HasDatabaseName("IX_FgsServiceAgreementNote_BillingScheduleId");
    }
}
