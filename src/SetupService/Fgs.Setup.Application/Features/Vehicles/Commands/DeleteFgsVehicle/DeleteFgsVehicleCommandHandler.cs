using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.DeleteFgsVehicle;

public sealed class DeleteFgsVehicleCommandHandler(
    IFgsVehicleWriteService writeService,
    ILogger<DeleteFgsVehicleCommandHandler> logger)
    : IRequestHandler<DeleteFgsVehicleCommand, ApiResponse<FgsVehicleDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleDetailDto>> Handle(
        DeleteFgsVehicleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted vehicle {Id}", result.Id);
            return ApiResponse<FgsVehicleDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete vehicle {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVehicleDetailDto>(ex);
        }
    }
}
