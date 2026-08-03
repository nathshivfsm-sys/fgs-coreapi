using Fgs.Asset.Application.Features.Assets.Dtos;

namespace Fgs.Asset.Infrastructure.Assets;

internal class FgsAssetSummaryRow
{
    public long Id { get; set; }
    public Guid AssetGuid { get; set; }
    public string AssetNumber { get; set; } = null!;
    public long? ServiceLocationId { get; set; }
    public string? UnitNumber { get; set; }
    public long? AssetTypeId { get; set; }
    public long? AssetManufacturerId { get; set; }
    public long? AssetModelId { get; set; }
    public string? AssetDescription { get; set; }
    public string? CustomerAssetNumber { get; set; }
    public string? CustomerAssetName { get; set; }
    public string? ManufacturerName { get; set; }
    public string? ModelNumber { get; set; }
    public string? SerialNumber { get; set; }
    public DateOnly? ManufactureDate { get; set; }
    public DateOnly? InstallDate { get; set; }
    public long? InstalledWorkOrderId { get; set; }
    public bool IsInstalledByCompany { get; set; }
    public bool IsOurInstallation { get; set; }
    public long AssetStatusId { get; set; }
    public bool IsActive { get; set; }

    public FgsAssetSummaryDto ToDto() =>
        new(
            Id,
            AssetGuid,
            AssetNumber,
            ServiceLocationId,
            UnitNumber,
            AssetTypeId,
            AssetManufacturerId,
            AssetModelId,
            AssetDescription,
            CustomerAssetNumber,
            CustomerAssetName,
            ManufacturerName,
            ModelNumber,
            SerialNumber,
            ManufactureDate,
            InstallDate,
            InstalledWorkOrderId,
            IsInstalledByCompany,
            IsOurInstallation,
            AssetStatusId,
            IsActive);
}

internal sealed class FgsAssetDetailRow : FgsAssetSummaryRow
{
    public new FgsAssetDetailDto ToDto() =>
        new(
            Id,
            AssetGuid,
            AssetNumber,
            ServiceLocationId,
            UnitNumber,
            AssetTypeId,
            AssetManufacturerId,
            AssetModelId,
            AssetDescription,
            CustomerAssetNumber,
            CustomerAssetName,
            ManufacturerName,
            ModelNumber,
            SerialNumber,
            ManufactureDate,
            InstallDate,
            InstalledWorkOrderId,
            IsInstalledByCompany,
            IsOurInstallation,
            AssetStatusId,
            IsActive);
}

internal sealed class FgsAssetLookupRow
{
    public long Id { get; set; }
    public string AssetNumber { get; set; } = null!;
    public string? CustomerAssetName { get; set; }

    public FgsAssetLookupDto ToDto() => new(Id, AssetNumber, CustomerAssetName);
}
