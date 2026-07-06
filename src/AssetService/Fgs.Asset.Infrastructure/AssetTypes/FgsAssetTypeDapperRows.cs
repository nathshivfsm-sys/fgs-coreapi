using Fgs.Asset.Application.Features.AssetTypes.Dtos;
namespace Fgs.Asset.Infrastructure.AssetTypes;
internal sealed class FgsAssetTypeSummaryRow { public long Id { get; set; } public string Code { get; set; } = null!; public string Name { get; set; } = null!; public string? Description { get; set; } public bool IsActive { get; set; } public FgsAssetTypeSummaryDto ToDto() => new(Id, Code, Name, Description, IsActive); }
internal sealed class FgsAssetTypeDetailRow { public long Id { get; set; } public string Code { get; set; } = null!; public string Name { get; set; } = null!; public string? Description { get; set; } public bool IsActive { get; set; } public FgsAssetTypeDetailDto ToDto() => new(Id, Code, Name, Description, IsActive); }
internal sealed class FgsAssetTypeLookupRow { public long Id { get; set; } public string Code { get; set; } = null!; public string Name { get; set; } = null!; public FgsAssetTypeLookupDto ToDto() => new(Id, Code, Name); }
