namespace Fgs.Bff.Application.Features.Lookups.Dtos;

public sealed record LookupRequestDto(
    LookupKey Key,
    bool ActiveOnly = true,
    string? CountryCode = null,
    string? UnitType = null,
    int? SourceTypeId = null,
    long? JobTypeId = null,
    long? PriceBookId = null,
    long? PricingMatrixId = null,
    long? PricingMatrixLaborId = null,
    long? UniversalPricingServiceId = null,
    long? InventoryItemId = null,
    long? FgsRoleId = null,
    long? RoleId = null,
    Guid? UserId = null,
    bool? ShowToFieldTech = null,
    bool? AllowToPick = null,
    bool? IsMobileVisible = null,
    bool? IsCustomerPortalVisible = null,
    string? StateProvinceCode = null);

public sealed record LookupItemDto(
    string? Id,
    string? Code,
    string? Name,
    int? DisplayOrder,
    IReadOnlyDictionary<string, object?>? Extra);

public sealed record LookupResultDto(
    LookupKey Key,
    IReadOnlyList<LookupItemDto> Items,
    string? Error = null);

public sealed record LookupKeyInfoDto(
    LookupKey Key,
    string Service,
    string Path,
    bool RequiresTenant,
    IReadOnlyList<string> RequiredFilters,
    IReadOnlyList<string> OptionalFilters,
    string Description);
