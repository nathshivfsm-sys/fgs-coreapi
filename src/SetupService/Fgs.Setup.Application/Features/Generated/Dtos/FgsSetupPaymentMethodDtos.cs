namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupPaymentMethod</summary>
public sealed record FgsSetupPaymentMethodSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupPaymentMethodDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible,
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

public sealed record FgsSetupPaymentMethodCreateDto(
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible)
;

public sealed record FgsSetupPaymentMethodUpdateDto(
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int SortOrder,
    /// <summary>IsMobileVisible</summary>
    bool IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool IsCustomerPortalVisible)
;

public sealed record FgsSetupPaymentMethodPatchDto(
    /// <summary>DisplayName</summary>
    string? DisplayName,
    /// <summary>SortOrder</summary>
    int? SortOrder,
    /// <summary>IsMobileVisible</summary>
    bool? IsMobileVisible,
    /// <summary>IsCustomerPortalVisible</summary>
    bool? IsCustomerPortalVisible)
;

