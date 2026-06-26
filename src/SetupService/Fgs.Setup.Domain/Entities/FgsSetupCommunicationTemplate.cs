using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant/company communication template (email, SMS, push). Null tenant/company = global default.
/// </summary>
public class FgsSetupCommunicationTemplate : FgsEntityBase, INullableTenantCompanyScoped
{
    public long Id { get; set; }

    public long? TenantId { get; set; }

    public long? CompanyId { get; set; }

    /// <summary>Email, SMS, PushNotification, or SystemNotification.</summary>
    public string CommunicationChannel { get; set; } = null!;

    public string TemplateType { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Subject { get; set; }

    public string Body { get; set; } = null!;

    public bool IsMobileVisible { get; set; } = true;

    public bool IsActive { get; set; } = true;
}
