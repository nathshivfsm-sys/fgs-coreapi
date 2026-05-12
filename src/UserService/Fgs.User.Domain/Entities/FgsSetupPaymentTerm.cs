namespace Fgs.User.Domain.Entities;

public class FgsSetupPaymentTerm : FgsTenantCompanySetupEntityBase
{
    public string Name { get; set; } = null!;

    public string DueDateMethod { get; set; } = null!;

    public int? NumberOfDays { get; set; }

    public bool IsAccountsReceivable { get; set; } = true;

    public bool IsAccountsPayable { get; set; }

    public bool IsMobileVisible { get; set; } = true;
}
