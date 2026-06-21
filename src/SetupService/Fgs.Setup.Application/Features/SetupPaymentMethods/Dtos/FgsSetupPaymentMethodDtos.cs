namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;

public sealed record FgsSetupPaymentMethodSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string DisplayName,
    int SortOrder,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupPaymentMethodDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string DisplayName,
    int SortOrder,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupPaymentMethodLookupDto(
    long Id,
    string DisplayName,
    int SortOrder);

public sealed record FgsSetupPaymentMethodCreateDto(
    string DisplayName,
    int SortOrder,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible);

public sealed record FgsSetupPaymentMethodUpdateDto(
    string DisplayName,
    int SortOrder,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible);

public sealed record FgsSetupPaymentMethodPatchDto(
    string? DisplayName,
    int? SortOrder,
    bool? IsMobileVisible,
    bool? IsCustomerPortalVisible,
    bool? IsActive);

public sealed record FgsSetupPaymentMethodListFilters(
    string? DisplayName = null);
