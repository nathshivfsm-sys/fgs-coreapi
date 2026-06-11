using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Included or excluded coverage item for a service agreement template.
/// </summary>
public class FgsSetupServiceAgreementTemplateCoverage : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long ServiceAgreementTemplateId { get; set; }

    public string CoverageTypeCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;
}
