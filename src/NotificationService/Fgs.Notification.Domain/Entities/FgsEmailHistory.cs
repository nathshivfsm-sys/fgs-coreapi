using Fgs.Kernel.Entities;

namespace Fgs.Notification.Domain.Entities;

public class FgsEmailHistory : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string EntityType { get; set; } = null!;

    public long EntityId { get; set; }

    public long? EmailTemplateId { get; set; }

    public string Subject { get; set; } = null!;

    public string FromEmailAddress { get; set; } = null!;

    public string? FromDisplayName { get; set; }

    public string ToEmailAddresses { get; set; } = null!;

    public string? CcEmailAddresses { get; set; }

    public string? BccEmailAddresses { get; set; }

    public string? BodyHtml { get; set; }

    public string? BodyText { get; set; }

    public bool HasAttachments { get; set; }

    public string Status { get; set; } = null!;

    public DateTimeOffset? SentOn { get; set; }

    public string? FailureReason { get; set; }

    public string? ProviderMessageId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
