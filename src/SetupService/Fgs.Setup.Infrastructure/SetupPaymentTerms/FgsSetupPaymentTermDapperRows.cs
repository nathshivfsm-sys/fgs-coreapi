using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;

namespace Fgs.Setup.Infrastructure.SetupPaymentTerms;

internal sealed class FgsSetupPaymentTermSummaryRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string DueDateMethod { get; set; } = null!;
    public int? NumberOfDays { get; set; }
    public bool IsAccountsReceivable { get; set; }
    public bool IsAccountsPayable { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupPaymentTermSummaryDto ToDto() =>
        new(
            Id,
            Name,
            DueDateMethod,
            NumberOfDays,
            IsAccountsReceivable,
            IsAccountsPayable,
            IsMobileVisible,
            IsActive);
}

internal sealed class FgsSetupPaymentTermDetailRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string DueDateMethod { get; set; } = null!;
    public int? NumberOfDays { get; set; }
    public bool IsAccountsReceivable { get; set; }
    public bool IsAccountsPayable { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupPaymentTermDetailDto ToDto() =>
        new(
            Id,
            Name,
            DueDateMethod,
            NumberOfDays,
            IsAccountsReceivable,
            IsAccountsPayable,
            IsMobileVisible,
            IsActive);
}

internal sealed class FgsSetupPaymentTermLookupRow
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public FgsSetupPaymentTermLookupDto ToDto() => new(Id,
            Name);
}
