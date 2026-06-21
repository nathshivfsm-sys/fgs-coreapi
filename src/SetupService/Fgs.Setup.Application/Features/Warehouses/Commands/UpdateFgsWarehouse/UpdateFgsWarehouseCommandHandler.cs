using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Warehouses.Commands.UpdateFgsWarehouse;

public sealed class UpdateFgsWarehouseCommandHandler(
    IFgsWarehouseWriteService writeService,
    ILogger<UpdateFgsWarehouseCommandHandler> logger)
    : IRequestHandler<UpdateFgsWarehouseCommand, ApiResponse<FgsWarehouseDetailDto>>
{
    public async Task<ApiResponse<FgsWarehouseDetailDto>> Handle(
        UpdateFgsWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated warehouse {Id}", result.Id);
            return ApiResponse<FgsWarehouseDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update warehouse {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsWarehouseDetailDto>(ex);
        }
    }
}
