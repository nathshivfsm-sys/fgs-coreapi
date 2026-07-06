using Fgs.Asset.Application.Features.AssetModels.Dtos;
namespace Fgs.Asset.Infrastructure.AssetModels;
internal class FgsAssetModelSummaryRow { public long Id { get; set; } public long AssetTypeId { get; set; } public long AssetManufacturerId { get; set; } public string ModelNumber { get; set; } = null!; public string ModelDescription { get; set; } = null!; public bool IsActive { get; set; } public FgsAssetModelSummaryDto ToDto() => new(Id, AssetTypeId, AssetManufacturerId, ModelNumber, ModelDescription, IsActive); }
internal sealed class FgsAssetModelDetailRow : FgsAssetModelSummaryRow { public new FgsAssetModelDetailDto ToDto() => new(Id, AssetTypeId, AssetManufacturerId, ModelNumber, ModelDescription, IsActive); }
internal sealed class FgsAssetModelLookupRow { public long Id { get; set; } public string ModelNumber { get; set; } = null!; public string ModelDescription { get; set; } = null!; public FgsAssetModelLookupDto ToDto() => new(Id, ModelNumber, ModelDescription); }
