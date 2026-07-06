using Fgs.User.Application.Common.Locations;

namespace Fgs.User.Application.Features.Tenants.Dtos;

public sealed record TenantDetailSectionDto(
    long Id,
    Guid TenantGuid,
    string Code,
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? DefaultCurrency,
    int? DefaultLanguageId,
    short FgsTenantStatusId,
    bool IsActive);

public sealed record CompanyDetailSectionDto(
    long Id,
    long CompanyNumber,
    Guid CompanyGuid,
    string Code,
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? TaxId,
    string? CompanySize,
    string? TimeZone,
    bool IsActive,
    LocationDetailDto? PhysicalAddress,
    LocationDetailDto? BillingAddress);

public sealed record TenantCompanyDetailDto(
    TenantDetailSectionDto Tenant,
    CompanyDetailSectionDto Company);

public sealed record UpdateTenantSectionRequest(
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? DefaultCurrency,
    int? DefaultLanguageId);

public sealed record UpdateCompanySectionRequest(
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? TaxId,
    string? CompanySize,
    string? TimeZone,
    LocationWriteDto? PhysicalAddress,
    LocationWriteDto? BillingAddress);

public sealed record UpdateTenantCompanyDetailsRequest(
    UpdateTenantSectionRequest Tenant,
    UpdateCompanySectionRequest Company);
