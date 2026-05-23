namespace Fgs.Platform.Domain.Entities;

/// <summary>
/// Communication template (email, SMS, push) per platform setup model.
/// <see cref="TenantId"/> and <see cref="CompanyId"/> are null for global defaults.
/// </summary>
public sealed class FgsSetupCommunicationTemplate
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

    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}
