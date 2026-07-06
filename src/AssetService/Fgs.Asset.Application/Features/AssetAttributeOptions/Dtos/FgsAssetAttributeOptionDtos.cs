namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
public sealed record FgsAssetAttributeOptionSummaryDto(long Id, long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder, bool IsActive);
public sealed record FgsAssetAttributeOptionDetailDto(long Id, long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder, bool IsActive);
public sealed record FgsAssetAttributeOptionLookupDto(long Id, string OptionCode, string OptionName);
public sealed record FgsAssetAttributeOptionCreateDto(long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder);
public sealed record FgsAssetAttributeOptionUpdateDto(long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder);
public sealed record FgsAssetAttributeOptionPatchDto(long? AssetAttributeId, string? OptionCode, string? OptionName, int? DisplayOrder, bool? IsActive);
public sealed record FgsAssetAttributeOptionListFilters(string? OptionCode = null, string? OptionName = null, long? AssetAttributeId = null);
