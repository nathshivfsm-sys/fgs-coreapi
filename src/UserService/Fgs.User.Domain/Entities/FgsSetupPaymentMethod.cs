namespace Fgs.User.Domain.Entities;

public class FgsSetupPaymentMethod : FgsTenantCompanySetupEntityBase
{
    public string PaymentMethodType { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool IsMobileVisible { get; set; } = true;

    public bool IsCustomerPortalVisible { get; set; } = true;
}
