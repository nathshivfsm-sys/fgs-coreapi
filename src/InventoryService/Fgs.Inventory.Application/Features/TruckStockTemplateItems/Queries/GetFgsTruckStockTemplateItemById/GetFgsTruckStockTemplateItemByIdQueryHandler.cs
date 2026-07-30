using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.GetFgsTruckStockTemplateItemById;

public sealed class GetFgsTruckStockTemplateItemByIdQueryHandler(
    IFgsTruckStockTemplateItemReadRepository readRepository)
    : IRequestHandler<GetFgsTruckStockTemplateItemByIdQuery, ApiResponse<FgsTruckStockTemplateItemDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateItemDetailDto>> Handle(
        GetFgsTruckStockTemplateItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.TemplateId, request.ItemId, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTruckStockTemplateItemDetailDto>.Fail(
                [$"Truck stock template item '{request.ItemId}' was not found on template '{request.TemplateId}'."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsTruckStockTemplateItemDetailDto>.Ok(result);
    }
}
