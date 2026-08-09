using Fgs.Kernel.Entities;
using Fgs.Notification.Domain.Enums;

namespace Fgs.Notification.Domain.Entities;

/// <summary>
/// Stores outbound email history for business entities and provides a permanent audit trail of email communications.
/// </summary>
public class FgsEmailHistory : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string RecordType { get; set; } = null!;

    public long RecordId { get; set; }

    public long? EmailTemplateId { get; set; }

    public NotificationStatus Status { get; set; } = NotificationStatus.Queued;

    public NotificationSourceApplication SourceApplication { get; set; }

    public string Subject { get; set; } = null!;

    public string FromEmailAddress { get; set; } = null!;

    public string? FromDisplayName { get; set; }

    public string ToEmailAddresses { get; set; } = null!;

    public string? CcEmailAddresses { get; set; }

    public string? BccEmailAddresses { get; set; }

    public string Body { get; set; } = null!;

    public string? ProviderName { get; set; }

    public string? ProviderMessageId { get; set; }

    public DateTimeOffset? SentOn { get; set; }

    public DateTimeOffset? DeliveredOn { get; set; }

    public DateTimeOffset? OpenedOn { get; set; }

    public DateTimeOffset? FailedOn { get; set; }

    public string? FailureReason { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public long? CreatedBy { get; set; }
}
