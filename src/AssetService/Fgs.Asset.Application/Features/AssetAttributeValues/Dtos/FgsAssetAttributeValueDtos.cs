namespace Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
public sealed record FgsAssetAttributeValueSummaryDto(long Id, long AssetId, long AssetAttributeId, long? OptionId, string? ValueText, int? ValueInteger, decimal? ValueDecimal, DateOnly? ValueDate, bool? ValueBoolean);
public sealed record FgsAssetAttributeValueDetailDto(long Id, long AssetId, long AssetAttributeId, long? OptionId, string? ValueText, int? ValueInteger, decimal? ValueDecimal, DateOnly? ValueDate, bool? ValueBoolean);
public sealed record FgsAssetAttributeValueLookupDto(long Id, long AssetId, long AssetAttributeId, long? OptionId, string? ValueText, int? ValueInteger, decimal? ValueDecimal, DateOnly? ValueDate, bool? ValueBoolean);
public sealed record FgsAssetAttributeValueCreateDto(long AssetId, long AssetAttributeId, long? OptionId, string? ValueText, int? ValueInteger, decimal? ValueDecimal, DateOnly? ValueDate, bool? ValueBoolean);
public sealed record FgsAssetAttributeValueUpdateDto(long AssetId, long AssetAttributeId, long? OptionId, string? ValueText, int? ValueInteger, decimal? ValueDecimal, DateOnly? ValueDate, bool? ValueBoolean);
public sealed record FgsAssetAttributeValuePatchDto(long? AssetId, long? AssetAttributeId, long? OptionId, string? ValueText, int? ValueInteger, decimal? ValueDecimal, DateOnly? ValueDate, bool? ValueBoolean);
public sealed record FgsAssetAttributeValueListFilters(long? AssetId = null, long? AssetAttributeId = null);
