namespace Fgs.User.Domain.Entities;

public class FgsSetupCommunicationTemplate : FgsTenantCompanySetupEntityBase<long>
{
    public string TemplateType { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Subject { get; set; }

    public string Body { get; set; } = null!;

    public bool IsMobileVisible { get; set; } = true;
}
