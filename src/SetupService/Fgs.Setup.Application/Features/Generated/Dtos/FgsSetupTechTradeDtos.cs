namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupTechTrade</summary>
public sealed record FgsSetupTechTradeSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TradeCode</summary>
    string? TradeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int? SortOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTechTradeDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TradeCode</summary>
    string? TradeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int? SortOrder,
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

public sealed record FgsSetupTechTradeCreateDto(
    /// <summary>TradeCode</summary>
    string? TradeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

public sealed record FgsSetupTechTradeUpdateDto(
    /// <summary>TradeCode</summary>
    string? TradeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

public sealed record FgsSetupTechTradePatchDto(
    /// <summary>TradeCode</summary>
    string? TradeCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>SortOrder</summary>
    int? SortOrder)
;

