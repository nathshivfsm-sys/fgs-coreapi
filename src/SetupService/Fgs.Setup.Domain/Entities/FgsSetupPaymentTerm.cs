namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPaymentTerm : FgsTenantCompanySetupEntityBase<long>
{
    public string Name { get; set; } = null!;

    public string DueDateMethod { get; set; } = null!;

    public int? NumberOfDays { get; set; }

    public bool IsAccountsReceivable { get; set; } = true;

    public bool IsAccountsPayable { get; set; } = true;

    public bool IsMobileVisible { get; set; } = true;
}
