namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPaymentMethod : FgsTenantCompanySetupEntityBase<long>
{
    public string DisplayName { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsMobileVisible { get; set; } = true;

    public bool IsCustomerPortalVisible { get; set; } = true;
}
