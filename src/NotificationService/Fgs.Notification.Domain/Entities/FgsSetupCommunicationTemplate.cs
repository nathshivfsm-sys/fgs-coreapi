namespace Fgs.Notification.Domain.Entities;

using Fgs.Kernel.Entities;

/// <summary>
/// Communication template (email, SMS, push) per platform setup model.
/// <see cref="TenantId"/> and <see cref="CompanyId"/> are null for global defaults.
/// </summary>
public sealed class FgsSetupCommunicationTemplate : INullableTenantCompanyScoped
{
    public long Id { get; set; }

    public long? TenantId { get; set; }

    public long? CompanyId { get; set; }

    /// <summary>EMAIL, SMS, or PUSH.</summary>
    public string TemplateType { get; set; } = string.Empty;

    /// <summary>Business event code (e.g. COMPANY_ADMIN_INVITATION).</summary>
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Subject { get; set; }

    public string Body { get; set; } = string.Empty;

    public bool IsMobileVisible { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }
}
