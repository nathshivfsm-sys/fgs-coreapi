namespace Fgs.Asset.Application.Features.AssetManufacturers.Dtos;

public sealed record FgsAssetManufacturerSummaryDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record FgsAssetManufacturerDetailDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record FgsAssetManufacturerLookupDto(long Id, string Code, string Name);
public sealed record FgsAssetManufacturerCreateDto(string Code, string Name, string? Description);
public sealed record FgsAssetManufacturerUpdateDto(string Code, string Name, string? Description);
public sealed record FgsAssetManufacturerPatchDto(string? Code, string? Name, string? Description, bool? IsActive);
public sealed record FgsAssetManufacturerListFilters(string? Code = null, string? Name = null);
