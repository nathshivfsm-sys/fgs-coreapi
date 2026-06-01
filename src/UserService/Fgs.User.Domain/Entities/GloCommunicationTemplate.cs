namespace Fgs.User.Domain.Entities;

/// <summary>
/// FSM-provided communication template available for system use or tenant customization.
/// </summary>
public class GloCommunicationTemplate
{
    public long Id { get; set; }

    /// <summary>System or Tenant scope.</summary>
    public string TemplateScope { get; set; } = "Tenant";

    /// <summary>Email, SMS, PushNotification, or SystemNotification.</summary>
    public string CommunicationChannel { get; set; } = null!;

    /// <summary>Business event code such as INVOICE_SENT or PASSWORD_RESET.</summary>
    public string TemplateCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Subject { get; set; }

    public string Body { get; set; } = null!;

    public bool IsMobileVisible { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public ICollection<GloCommunicationTemplateToken> TemplateTokens { get; set; } = [];
}
