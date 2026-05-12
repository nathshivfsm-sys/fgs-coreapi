namespace Fgs.User.Domain.Entities;

public class FgsSetupPaymentMethod : FgsTenantCompanySetupEntityBase
{
    public int GloPaymentMethodTypeId { get; set; }

    public string DisplayName { get; set; } = null!;

    public bool IsMobileVisible { get; set; } = true;

    public bool IsCustomerPortalVisible { get; set; } = true;
}
