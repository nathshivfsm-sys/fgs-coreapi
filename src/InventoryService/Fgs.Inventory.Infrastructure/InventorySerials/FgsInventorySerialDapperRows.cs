using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.Inventory.Domain.Enums;

namespace Fgs.Inventory.Infrastructure.InventorySerials;

internal sealed class FgsInventorySerialSummaryRow
{
    public long Id { get; set; }
    public long InventoryItemId { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string InventorySerialStatus { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsInventorySerialSummaryDto ToDto() =>
        new(
            Id,
            InventoryItemId,
            SerialNumber,
            Enum.Parse<FgsInventorySerialStatus>(InventorySerialStatus),
            Notes,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsInventorySerialDetailRow
{
    public long Id { get; set; }
    public long InventoryItemId { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string InventorySerialStatus { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsInventorySerialDetailDto ToDto() =>
        new(
            Id,
            InventoryItemId,
            SerialNumber,
            Enum.Parse<FgsInventorySerialStatus>(InventorySerialStatus),
            Notes,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsInventorySerialLookupRow
{
    public long Id { get; set; }
    public string SerialNumber { get; set; } = null!;
    public long InventoryItemId { get; set; }
    public string InventorySerialStatus { get; set; } = null!;

    public FgsInventorySerialLookupDto ToDto() =>
        new(Id, SerialNumber, InventoryItemId, Enum.Parse<FgsInventorySerialStatus>(InventorySerialStatus));
}
