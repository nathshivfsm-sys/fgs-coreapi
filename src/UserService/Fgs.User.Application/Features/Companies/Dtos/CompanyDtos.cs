using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Application.Features.Tenants.Dtos;

namespace Fgs.User.Application.Features.Companies.Dtos;

public sealed record CompanyAggregateDto(
    TenantDetailDto Tenant,
    CompanyDetailDto Company,
    FgsTenantServiceSetupDetailDto? ServiceSetup,
    FgsTenantServiceAccountsSetupDetailDto? ServiceAccountsSetup);

public sealed record CompanyDetailDto(
    long Id,
    long TenantId,
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

public sealed record CompanyCreateDto(
    string Code,
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

public sealed record CompanyUpdateDto(
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? TaxId,
    string? CompanySize,
    string? TimeZone,
    bool IsActive,
    LocationWriteDto? PhysicalAddress,
    LocationWriteDto? BillingAddress);

public sealed record CompanyPatchDto(
    string? Name = null,
    string? LegalName = null,
    string? Email = null,
    string? PhoneNumber = null,
    string? Website = null,
    string? TaxId = null,
    string? CompanySize = null,
    string? TimeZone = null,
    bool? IsActive = null,
    LocationWriteDto? PhysicalAddress = null,
    LocationWriteDto? BillingAddress = null);
