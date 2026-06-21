using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;

namespace Fgs.Setup.Infrastructure.SetupPaymentTerms;

internal sealed class FgsSetupPaymentTermSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Name { get; set; }
    public string DueDateMethod { get; set; }
    public int? NumberOfDays { get; set; }
    public bool IsAccountsReceivable { get; set; }
    public bool IsAccountsPayable { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupPaymentTermSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Name,
            DueDateMethod,
            NumberOfDays,
            IsAccountsReceivable,
            IsAccountsPayable,
            IsMobileVisible,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupPaymentTermDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Name { get; set; }
    public string DueDateMethod { get; set; }
    public int? NumberOfDays { get; set; }
    public bool IsAccountsReceivable { get; set; }
    public bool IsAccountsPayable { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupPaymentTermDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Name,
            DueDateMethod,
            NumberOfDays,
            IsAccountsReceivable,
            IsAccountsPayable,
            IsMobileVisible,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupPaymentTermLookupRow
{
    public long Id { get; set; }
    public string Name { get; set; }

    public FgsSetupPaymentTermLookupDto ToDto() => new(Id,
            Name);
}
