using Fgs.Setup.Application.Features.Warehouses.Dtos;

namespace Fgs.Setup.Infrastructure.Warehouses;

internal sealed class FgsWarehouseSummaryRow
{
    public long Id { get; set; }
    public string WarehouseCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string WarehouseType { get; set; } = null!;
    public Guid? AddressId { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public FgsWarehouseSummaryDto ToDto() =>
        new(
            Id,
            WarehouseCode,
            Name,
            WarehouseType,
            AddressId,
            Description,
            IsDefault,
            IsActive);
}

internal sealed class FgsWarehouseDetailRow
{
    public long Id { get; set; }
    public string WarehouseCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string WarehouseType { get; set; } = null!;
    public Guid? AddressId { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public FgsWarehouseDetailDto ToDto() =>
        new(
            Id,
            WarehouseCode,
            Name,
            WarehouseType,
            AddressId,
            Description,
            IsDefault,
            IsActive);
}

internal sealed class FgsWarehouseLookupRow
{
    public long Id { get; set; }
    public string WarehouseCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsWarehouseLookupDto ToDto() => new(Id,
            WarehouseCode,
            Name);
}
