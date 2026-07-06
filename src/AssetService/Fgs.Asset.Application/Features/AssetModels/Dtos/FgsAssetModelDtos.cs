namespace Fgs.Asset.Application.Features.AssetModels.Dtos;
public sealed record FgsAssetModelSummaryDto(long Id, long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription, bool IsActive);
public sealed record FgsAssetModelDetailDto(long Id, long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription, bool IsActive);
public sealed record FgsAssetModelLookupDto(long Id, string ModelNumber, string ModelDescription);
public sealed record FgsAssetModelCreateDto(long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription);
public sealed record FgsAssetModelUpdateDto(long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription);
public sealed record FgsAssetModelPatchDto(long? AssetTypeId, long? AssetManufacturerId, string? ModelNumber, string? ModelDescription, bool? IsActive);
public sealed record FgsAssetModelListFilters(string? ModelNumber = null, long? AssetTypeId = null, long? AssetManufacturerId = null);
