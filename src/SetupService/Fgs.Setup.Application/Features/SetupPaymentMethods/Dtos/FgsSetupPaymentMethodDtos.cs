namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;

public sealed record FgsSetupPaymentMethodSummaryDto(
    long Id,
    string DisplayName,
    int SortOrder,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IsActive);

public sealed record FgsSetupPaymentMethodDetailDto(
    long Id,
    string DisplayName,
    int SortOrder,
    bool IsMobileVisible,
    bool IsCustomerPortalVisible,
    bool IsActive);

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
