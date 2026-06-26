namespace Fgs.Setup.Domain.Entities;

public class FgsSetupTaxDetail : FgsTenantCompanySetupEntityBase<long>
{
    public long FgsSetupTaxId { get; set; }

    public long FgsSetupTaxAuthorityId { get; set; }

    public DateOnly EffectiveFromDate { get; set; }

    public DateOnly? EffectiveToDate { get; set; }

    public bool IsExternalSystemRecord { get; set; }
}
