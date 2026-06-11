namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPaymentTerm</summary>
public sealed record FgsSetupPaymentTermSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>DueDateMethod</summary>
    string? DueDateMethod,
    /// <summary>NumberOfDays</summary>
    int? NumberOfDays,
    /// <summary>IsAccountsReceivable</summary>
    bool IsAccountsReceivable,
    /// <summary>IsAccountsPayable</summary>
    bool IsAccountsPayable,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPaymentTermDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>DueDateMethod</summary>
    string? DueDateMethod,
    /// <summary>NumberOfDays</summary>
    int? NumberOfDays,
    /// <summary>IsAccountsReceivable</summary>
    bool IsAccountsReceivable,
    /// <summary>IsAccountsPayable</summary>
    bool IsAccountsPayable,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPaymentTermCreateDto(
    /// <summary>Name</summary>
    string? Name,
    /// <summary>DueDateMethod</summary>
    string? DueDateMethod,
    /// <summary>NumberOfDays</summary>
    int? NumberOfDays,
    /// <summary>IsAccountsReceivable</summary>
    bool IsAccountsReceivable,
    /// <summary>IsAccountsPayable</summary>
    bool IsAccountsPayable,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsSetupPaymentTermUpdateDto(
    /// <summary>Name</summary>
    string? Name,
    /// <summary>DueDateMethod</summary>
    string? DueDateMethod,
    /// <summary>NumberOfDays</summary>
    int? NumberOfDays,
    /// <summary>IsAccountsReceivable</summary>
    bool IsAccountsReceivable,
    /// <summary>IsAccountsPayable</summary>
    bool IsAccountsPayable,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible)
;

public sealed record FgsSetupPaymentTermPatchDto(
    /// <summary>Name</summary>
    string? Name,
    /// <summary>DueDateMethod</summary>
    string? DueDateMethod,
    /// <summary>NumberOfDays</summary>
    int? NumberOfDays,
    /// <summary>IsAccountsReceivable</summary>
    bool? IsAccountsReceivable,
    /// <summary>IsAccountsPayable</summary>
    bool? IsAccountsPayable,
    /// <summary>IsMobileVisible</summary>
    bool? IsMobileVisible)
;

