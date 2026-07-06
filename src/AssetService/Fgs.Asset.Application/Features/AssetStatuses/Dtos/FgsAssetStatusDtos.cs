namespace Fgs.Asset.Application.Features.AssetStatuses.Dtos;

public sealed record FgsAssetStatusSummaryDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record FgsAssetStatusDetailDto(long Id, string Code, string Name, string? Description, bool IsActive);
public sealed record FgsAssetStatusLookupDto(long Id, string Code, string Name);
public sealed record FgsAssetStatusCreateDto(string Code, string Name, string? Description);
public sealed record FgsAssetStatusUpdateDto(string Code, string Name, string? Description);
public sealed record FgsAssetStatusPatchDto(string? Code, string? Name, string? Description, bool? IsActive);
public sealed record FgsAssetStatusListFilters(string? Code = null, string? Name = null);
