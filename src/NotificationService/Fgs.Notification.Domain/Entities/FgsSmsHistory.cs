using Fgs.Kernel.Entities;
using Fgs.Notification.Domain.Enums;

namespace Fgs.Notification.Domain.Entities;

/// <summary>
/// Stores outbound SMS history for business entities and provides a permanent audit trail of SMS communications.
/// </summary>
public class FgsSmsHistory : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string RecordType { get; set; } = null!;

    public long RecordId { get; set; }

    public long? TemplateId { get; set; }

    public NotificationStatus Status { get; set; } = NotificationStatus.Queued;

    public NotificationSourceApplication SourceApplication { get; set; }

    public string FromPhoneNumber { get; set; } = null!;

    public string ToPhoneNumber { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? ProviderName { get; set; }

    public string? ProviderMessageId { get; set; }

    public short SegmentCount { get; set; } = 1;

    public DateTimeOffset? SentOn { get; set; }

    public DateTimeOffset? DeliveredOn { get; set; }

    public DateTimeOffset? FailedOn { get; set; }

    public string? FailureReason { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public long? CreatedBy { get; set; }
}
