namespace Fgs.User.Domain.Entities;

public class FgsSetupTaxDetail : FgsTenantCompanySetupEntityBase
{
    public long FgsSetupTaxId { get; set; }

    public long FgsSetupTaxAuthorityId { get; set; }

    public DateOnly EffectiveFromDate { get; set; }

    public DateOnly? EffectiveToDate { get; set; }

    public decimal TaxPercent { get; set; }
}
