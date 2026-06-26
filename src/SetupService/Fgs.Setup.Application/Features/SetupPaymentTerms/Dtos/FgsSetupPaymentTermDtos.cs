namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;

public sealed record FgsSetupPaymentTermSummaryDto(
    long Id,
    string Name,
    string DueDateMethod,
    int? NumberOfDays,
    bool IsAccountsReceivable,
    bool IsAccountsPayable,
    bool IsMobileVisible,
    bool IsActive);

public sealed record FgsSetupPaymentTermDetailDto(
    long Id,
    string Name,
    string DueDateMethod,
    int? NumberOfDays,
    bool IsAccountsReceivable,
    bool IsAccountsPayable,
    bool IsMobileVisible,
    bool IsActive);

public sealed record FgsSetupPaymentTermLookupDto(
    long Id,
    string Name);

public sealed record FgsSetupPaymentTermCreateDto(
    string Name,
    string DueDateMethod,
    int? NumberOfDays,
    bool IsAccountsReceivable,
    bool IsAccountsPayable,
    bool IsMobileVisible);

public sealed record FgsSetupPaymentTermUpdateDto(
    string Name,
    string DueDateMethod,
    int? NumberOfDays,
    bool IsAccountsReceivable,
    bool IsAccountsPayable,
    bool IsMobileVisible);

public sealed record FgsSetupPaymentTermPatchDto(
    string? Name,
    string? DueDateMethod,
    int? NumberOfDays,
    bool? IsAccountsReceivable,
    bool? IsAccountsPayable,
    bool? IsMobileVisible,
    bool? IsActive);

public sealed record FgsSetupPaymentTermListFilters(
    string? Name = null);
