namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global payment term master used to seed tenant/company payment terms.
/// </summary>
public class GloSetupPaymentTerm
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string DueDateMethod { get; set; } = null!;

    public int? NumberOfDays { get; set; }

    public bool IsAccountsReceivable { get; set; } = true;

    public bool IsAccountsPayable { get; set; }

    public bool IsMobileVisible { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; } = true;
}
