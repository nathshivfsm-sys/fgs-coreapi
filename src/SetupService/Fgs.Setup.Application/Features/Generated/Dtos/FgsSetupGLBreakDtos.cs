namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupGLBreak</summary>
public sealed record FgsSetupGLBreakSummaryDto(
    /// <summary>Surrogate primary key.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Unique GL break code within tenant, company, and break level scope.</summary>
    string? Code,
    /// <summary>Display name of the GL break.</summary>
    string? Name,
    /// <summary>Optional label displayed in UI and financial documents.</summary>
    string? BreakLabel,
    /// <summary>Break hierarchy level. Allowed values: 1 or 2.</summary>
    short BreakLevel,
    /// <summary>Optional reference to uploaded logo file in FgsFile.</summary>
    long? LogoFileId,
    /// <summary>Optional reference to branch or break address in FgsLocation.</summary>
    Guid? AddressId,
    /// <summary>UTC timestamp when the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UTC timestamp when the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the GL break is active.</summary>
    bool IsActive)
;

public sealed record FgsSetupGLBreakDetailDto(
    /// <summary>Surrogate primary key.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>Unique GL break code within tenant, company, and break level scope.</summary>
    string? Code,
    /// <summary>Display name of the GL break.</summary>
    string? Name,
    /// <summary>Optional label displayed in UI and financial documents.</summary>
    string? BreakLabel,
    /// <summary>Break hierarchy level. Allowed values: 1 or 2.</summary>
    short BreakLevel,
    /// <summary>Optional reference to uploaded logo file in FgsFile.</summary>
    long? LogoFileId,
    /// <summary>Optional reference to branch or break address in FgsLocation.</summary>
    Guid? AddressId,
    /// <summary>UTC timestamp when the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User or process that created the record.</summary>
    string? CreatedBy,
    /// <summary>UTC timestamp when the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User or process that last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the GL break is active.</summary>
    bool IsActive)
;

public sealed record FgsSetupGLBreakCreateDto(
    /// <summary>Unique GL break code within tenant, company, and break level scope.</summary>
    string? Code,
    /// <summary>Display name of the GL break.</summary>
    string? Name,
    /// <summary>Optional label displayed in UI and financial documents.</summary>
    string? BreakLabel,
    /// <summary>Break hierarchy level. Allowed values: 1 or 2.</summary>
    short BreakLevel,
    /// <summary>Optional reference to uploaded logo file in FgsFile.</summary>
    long? LogoFileId,
    /// <summary>Optional reference to branch or break address in FgsLocation.</summary>
    Guid? AddressId)
;

public sealed record FgsSetupGLBreakUpdateDto(
    /// <summary>Unique GL break code within tenant, company, and break level scope.</summary>
    string? Code,
    /// <summary>Display name of the GL break.</summary>
    string? Name,
    /// <summary>Optional label displayed in UI and financial documents.</summary>
    string? BreakLabel,
    /// <summary>Break hierarchy level. Allowed values: 1 or 2.</summary>
    short BreakLevel,
    /// <summary>Optional reference to uploaded logo file in FgsFile.</summary>
    long? LogoFileId,
    /// <summary>Optional reference to branch or break address in FgsLocation.</summary>
    Guid? AddressId)
;

public sealed record FgsSetupGLBreakPatchDto(
    /// <summary>Unique GL break code within tenant, company, and break level scope.</summary>
    string? Code,
    /// <summary>Display name of the GL break.</summary>
    string? Name,
    /// <summary>Optional label displayed in UI and financial documents.</summary>
    string? BreakLabel,
    /// <summary>Break hierarchy level. Allowed values: 1 or 2.</summary>
    short? BreakLevel,
    /// <summary>Optional reference to uploaded logo file in FgsFile.</summary>
    long? LogoFileId,
    /// <summary>Optional reference to branch or break address in FgsLocation.</summary>
    Guid? AddressId)
;

