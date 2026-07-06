using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
namespace Fgs.Asset.Infrastructure.AssetStatuses;
internal sealed class FgsAssetStatusSummaryRow { public long Id { get; set; } public string Code { get; set; } = null!; public string Name { get; set; } = null!; public string? Description { get; set; } public bool IsActive { get; set; } public FgsAssetStatusSummaryDto ToDto() => new(Id, Code, Name, Description, IsActive); }
internal sealed class FgsAssetStatusDetailRow { public long Id { get; set; } public string Code { get; set; } = null!; public string Name { get; set; } = null!; public string? Description { get; set; } public bool IsActive { get; set; } public FgsAssetStatusDetailDto ToDto() => new(Id, Code, Name, Description, IsActive); }
internal sealed class FgsAssetStatusLookupRow { public long Id { get; set; } public string Code { get; set; } = null!; public string Name { get; set; } = null!; public FgsAssetStatusLookupDto ToDto() => new(Id, Code, Name); }
