using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;

namespace Fgs.Inventory.Application.Abstractions.PurchaseOrders;

public interface IFgsPurchaseOrderWriteService
{
    Task<FgsPurchaseOrderDetailDto> CreateAsync(FgsPurchaseOrderCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsPurchaseOrderDetailDto> UpdateAsync(long id, FgsPurchaseOrderUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsPurchaseOrderDetailDto> PatchAsync(long id, FgsPurchaseOrderPatchDto dto, CancellationToken cancellationToken = default);
}
