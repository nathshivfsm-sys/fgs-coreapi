using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.DeleteFgsWarehouse;

public sealed class DeleteFgsWarehouseCommandHandler(
    IFgsWarehouseWriteService writeService,
    ILogger<DeleteFgsWarehouseCommandHandler> logger)
    : IRequestHandler<DeleteFgsWarehouseCommand, ApiResponse<FgsWarehouseDetailDto>>
{
    public async Task<ApiResponse<FgsWarehouseDetailDto>> Handle(
        DeleteFgsWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted warehouse {Id}", result.Id);
            return ApiResponse<FgsWarehouseDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete warehouse {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsWarehouseDetailDto>(ex);
        }
    }
}
