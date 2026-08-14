using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Queries.GetFgsInventoryItemDependencyById;

public sealed class GetFgsInventoryItemDependencyByIdQueryHandler(
    IFgsInventoryItemDependencyReadRepository readRepository)
    : IRequestHandler<GetFgsInventoryItemDependencyByIdQuery, ApiResponse<FgsInventoryItemDependencyDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemDependencyDetailDto>> Handle(
        GetFgsInventoryItemDependencyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        return result is null
            ? ApiResponse<FgsInventoryItemDependencyDetailDto>.Fail(
                [$"Inventory item dependency '{request.Id}' was not found."],
                ApiStatusCodes.NotFound)
            : ApiResponse<FgsInventoryItemDependencyDetailDto>.Ok(result);
    }
}
