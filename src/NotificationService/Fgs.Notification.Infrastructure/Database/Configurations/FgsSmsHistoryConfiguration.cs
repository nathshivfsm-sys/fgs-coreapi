using Fgs.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Notification.Infrastructure.Database.Configurations;

internal sealed class FgsSmsHistoryConfiguration : IEntityTypeConfiguration<FgsSmsHistory>
{
    public void Configure(EntityTypeBuilder<FgsSmsHistory> entity)
    {
        entity.ToTable(
            "FgsSmsHistory",
            t => t.HasComment(
                "Stores outbound SMS history for business entities and provides a permanent audit trail of SMS communications."));

        entity.HasKey(e => e.Id).HasName("PK_FgsSmsHistory");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");

        entity.Property(e => e.TenantId).IsRequired().HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).IsRequired().HasComment("Company identifier.");
        entity.Property(e => e.RecordType).HasMaxLength(50).IsRequired()
            .HasComment("Business record associated with the SMS such as Estimate, Invoice, WorkOrder, Opportunity, Customer, or User.");
        entity.Property(e => e.RecordId).IsRequired()
            .HasComment("Identifier of the associated business record.");
        entity.Property(e => e.TemplateId)
            .HasComment("SMS template used to generate the message.");
        entity.Property(e => e.Status).IsRequired().HasDefaultValue(Domain.Enums.NotificationStatus.Queued)
            .HasComment("Current notification status.");
        entity.Property(e => e.SourceApplication).IsRequired()
            .HasComment("Application or component that originated the SMS.");
        entity.Property(e => e.FromPhoneNumber).HasMaxLength(30).IsRequired()
            .HasComment("Phone number or short code used to send the SMS.");
        entity.Property(e => e.ToPhoneNumber).HasMaxLength(30).IsRequired()
            .HasComment("Recipient mobile phone number.");
        entity.Property(e => e.Message).HasColumnType("text").IsRequired()
            .HasComment("Final SMS message that was sent to the recipient.");
        entity.Property(e => e.ProviderName).HasMaxLength(100)
            .HasComment("SMS provider used to send the message.");
        entity.Property(e => e.ProviderMessageId).HasMaxLength(500)
            .HasComment("Provider-specific message identifier used for troubleshooting and webhook tracking.");
        entity.Property(e => e.SegmentCount).HasDefaultValue((short)1).IsRequired()
            .HasComment("Number of SMS segments billed by the provider.");
        entity.Property(e => e.SentOn).HasColumnType("timestamptz")
            .HasComment("Date and time the SMS was sent.");
        entity.Property(e => e.DeliveredOn).HasColumnType("timestamptz")
            .HasComment("Date and time the SMS was confirmed as delivered by the provider.");
        entity.Property(e => e.FailedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the SMS failed to send or deliver.");
        entity.Property(e => e.FailureReason).HasColumnType("text")
            .HasComment("Failure reason returned by the SMS provider.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSmsHistory_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RecordType, e.RecordId })
            .HasDatabaseName("IX_FgsSmsHistory_Record");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Status })
            .HasDatabaseName("IX_FgsSmsHistory_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SentOn })
            .HasDatabaseName("IX_FgsSmsHistory_SentOn");
        entity.HasIndex(e => e.ProviderMessageId)
            .HasDatabaseName("IX_FgsSmsHistory_ProviderMessageId");
    }
}
