using Fgs.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Notification.Infrastructure.Database.Configurations;

internal sealed class FgsEmailHistoryConfiguration : IEntityTypeConfiguration<FgsEmailHistory>
{
    public void Configure(EntityTypeBuilder<FgsEmailHistory> entity)
    {
        entity.ToTable(
            "FgsEmailHistory",
            t => t.HasComment(
                "Stores outbound email history for business entities and provides a permanent audit trail of email communications."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");

        entity.Property(e => e.TenantId).HasColumnOrder(1).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasColumnOrder(2).HasComment("Company identifier.");
        entity.Property(e => e.EntityType).HasMaxLength(50).IsRequired()
            .HasComment("Entity associated with the email such as Estimate, Invoice, WorkOrder, Opportunity, or Customer.");
        entity.Property(e => e.EntityId).HasComment("Identifier of the associated business entity.");
        entity.Property(e => e.EmailTemplateId).HasComment("Email template used to generate the email.");
        entity.Property(e => e.Subject).HasMaxLength(500).IsRequired().HasComment("Email subject line.");
        entity.Property(e => e.FromEmailAddress).HasMaxLength(500).IsRequired().HasComment("Sender email address.");
        entity.Property(e => e.FromDisplayName).HasMaxLength(255).HasComment("Sender display name.");
        entity.Property(e => e.ToEmailAddresses).HasColumnType("jsonb").IsRequired()
            .HasComment("JSON array containing recipient email addresses.");
        entity.Property(e => e.CcEmailAddresses).HasColumnType("jsonb")
            .HasComment("JSON array containing carbon copy recipient email addresses.");
        entity.Property(e => e.BccEmailAddresses).HasColumnType("jsonb")
            .HasComment("JSON array containing blind carbon copy recipient email addresses.");
        entity.Property(e => e.BodyHtml).HasColumnType("text").HasComment("Email body in HTML format.");
        entity.Property(e => e.BodyText).HasColumnType("text").HasComment("Email body in plain text format.");
        entity.Property(e => e.HasAttachments).HasDefaultValue(false)
            .HasComment("Indicates whether one or more attachments were included in the email.");
        entity.Property(e => e.Status).HasMaxLength(50).IsRequired()
            .HasComment("Email delivery status such as Queued, Sent, Delivered, Opened, Failed, or Bounced.");
        entity.Property(e => e.SentOn).HasColumnType("timestamptz")
            .HasComment("Date and time the email was sent.");
        entity.Property(e => e.FailureReason).HasColumnType("text")
            .HasComment("Failure reason returned by the email provider when send fails.");
        entity.Property(e => e.ProviderMessageId).HasMaxLength(500)
            .HasComment("Provider-specific message identifier used for troubleshooting and webhook tracking.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEmailHistory_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EntityType, e.EntityId })
            .HasDatabaseName("IX_FgsEmailHistory_TenantId_CompanyId_EntityType_EntityId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Status })
            .HasDatabaseName("IX_FgsEmailHistory_TenantId_CompanyId_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SentOn })
            .HasDatabaseName("IX_FgsEmailHistory_TenantId_CompanyId_SentOn");
        entity.HasIndex(e => e.ProviderMessageId)
            .HasDatabaseName("IX_FgsEmailHistory_ProviderMessageId");
    }
}
