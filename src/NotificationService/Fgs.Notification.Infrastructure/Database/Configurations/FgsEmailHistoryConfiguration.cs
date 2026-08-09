using Fgs.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Notification.Infrastructure.Database.Configurations;

internal sealed class FgsEmailHistoryConfiguration : IEntityTypeConfiguration<FgsEmailHistory>
{
    public void Configure(EntityTypeBuilder<FgsEmailHistory> entity)
    {
        entity.ToTable(
            "FgsEmailHistory",
            t => t.HasComment(
                "Stores outbound email history for business entities and provides a permanent audit trail of email communications."));

        entity.HasKey(e => e.Id).HasName("PK_FgsEmailHistory");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");

        entity.Property(e => e.TenantId).IsRequired().HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).IsRequired().HasComment("Company identifier.");
        entity.Property(e => e.RecordType).HasMaxLength(50).IsRequired()
            .HasComment("Business record associated with the email such as Estimate, Invoice, WorkOrder, Opportunity, Customer, or User.");
        entity.Property(e => e.RecordId).IsRequired()
            .HasComment("Identifier of the associated business record.");
        entity.Property(e => e.EmailTemplateId)
            .HasComment("Email template used to generate the email.");
        entity.Property(e => e.Status).IsRequired().HasDefaultValue(Domain.Enums.NotificationStatus.Queued)
            .HasComment("Current notification status.");
        entity.Property(e => e.SourceApplication).IsRequired()
            .HasComment("Application or component that originated the email.");
        entity.Property(e => e.Subject).HasMaxLength(500).IsRequired().HasComment("Email subject line.");
        entity.Property(e => e.FromEmailAddress).HasMaxLength(500).IsRequired().HasComment("Sender email address.");
        entity.Property(e => e.FromDisplayName).HasMaxLength(255).HasComment("Sender display name.");
        entity.Property(e => e.ToEmailAddresses).HasColumnType("jsonb").IsRequired()
            .HasComment("JSON array containing recipient email addresses.");
        entity.Property(e => e.CcEmailAddresses).HasColumnType("jsonb")
            .HasComment("JSON array containing carbon copy recipient email addresses.");
        entity.Property(e => e.BccEmailAddresses).HasColumnType("jsonb")
            .HasComment("JSON array containing blind carbon copy recipient email addresses.");
        entity.Property(e => e.Body).HasColumnType("text").IsRequired()
            .HasComment("Final email body that was sent to the recipient.");
        entity.Property(e => e.ProviderName).HasMaxLength(100)
            .HasComment("Email provider used to send the message.");
        entity.Property(e => e.ProviderMessageId).HasMaxLength(500)
            .HasComment("Provider-specific message identifier used for troubleshooting and webhook tracking.");
        entity.Property(e => e.SentOn).HasColumnType("timestamptz")
            .HasComment("Date and time the email was sent.");
        entity.Property(e => e.DeliveredOn).HasColumnType("timestamptz")
            .HasComment("Date and time the email was confirmed as delivered by the provider.");
        entity.Property(e => e.OpenedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the email was opened.");
        entity.Property(e => e.FailedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the email failed to send or deliver.");
        entity.Property(e => e.FailureReason).HasColumnType("text")
            .HasComment("Failure reason returned by the email provider when send fails.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEmailHistory_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RecordType, e.RecordId })
            .HasDatabaseName("IX_FgsEmailHistory_Record");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Status })
            .HasDatabaseName("IX_FgsEmailHistory_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SentOn })
            .HasDatabaseName("IX_FgsEmailHistory_SentOn");
        entity.HasIndex(e => e.ProviderMessageId)
            .HasDatabaseName("IX_FgsEmailHistory_ProviderMessageId");
    }
}
