using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Queries.ListFgsInventoryItemAlternates;

public sealed class ListFgsInventoryItemAlternatesQueryHandler(
    IFgsInventoryItemAlternateReadRepository readRepository)
    : IRequestHandler<ListFgsInventoryItemAlternatesQuery, ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>> Handle(
        ListFgsInventoryItemAlternatesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByInventoryItemIdAsync(request.InventoryItemId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>.Ok(result);
    }
}
