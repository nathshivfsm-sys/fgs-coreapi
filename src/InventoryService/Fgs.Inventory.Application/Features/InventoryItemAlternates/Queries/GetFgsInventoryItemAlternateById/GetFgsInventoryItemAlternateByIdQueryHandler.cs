using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Abstractions.InventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Queries.GetFgsInventoryItemAlternateById;

public sealed class GetFgsInventoryItemAlternateByIdQueryHandler(
    IFgsInventoryItemAlternateReadRepository readRepository)
    : IRequestHandler<GetFgsInventoryItemAlternateByIdQuery, ApiResponse<FgsInventoryItemAlternateDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemAlternateDetailDto>> Handle(
        GetFgsInventoryItemAlternateByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        return result is null
            ? ApiResponse<FgsInventoryItemAlternateDetailDto>.Fail(
                [$"Inventory item alternate '{request.Id}' was not found."],
                ApiStatusCodes.NotFound)
            : ApiResponse<FgsInventoryItemAlternateDetailDto>.Ok(result);
    }
}
