using Fgs.Setup.Application.Features.Warehouses.Dtos;

namespace Fgs.Setup.Infrastructure.Warehouses;

internal sealed class FgsWarehouseSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string WarehouseCode { get; set; }
    public string Name { get; set; }
    public string WarehouseType { get; set; }
    public Guid? AddressId { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsWarehouseSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            WarehouseCode,
            Name,
            WarehouseType,
            AddressId,
            Description,
            IsDefault,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsWarehouseDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string WarehouseCode { get; set; }
    public string Name { get; set; }
    public string WarehouseType { get; set; }
    public Guid? AddressId { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsWarehouseDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            WarehouseCode,
            Name,
            WarehouseType,
            AddressId,
            Description,
            IsDefault,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsWarehouseLookupRow
{
    public long Id { get; set; }
    public string WarehouseCode { get; set; }
    public string Name { get; set; }

    public FgsWarehouseLookupDto ToDto() => new(Id,
            WarehouseCode,
            Name);
}
