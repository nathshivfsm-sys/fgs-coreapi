namespace Fgs.Asset.Application.Features.AssetTypes.Dtos;

public sealed record FgsAssetTypeSummaryDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record FgsAssetTypeDetailDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record FgsAssetTypeLookupDto(long Id, string Code, string Name);
public sealed record FgsAssetTypeCreateDto(string Code, string Name, string? Description);
public sealed record FgsAssetTypeUpdateDto(string Code, string Name, string? Description);
public sealed record FgsAssetTypePatchDto(string? Code, string? Name, string? Description, bool? IsActive);
public sealed record FgsAssetTypeListFilters(string? Code = null, string? Name = null);
