using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.PatchFgsVehicle;

public sealed class PatchFgsVehicleCommandHandler(
    IFgsVehicleWriteService writeService,
    ILogger<PatchFgsVehicleCommandHandler> logger)
    : IRequestHandler<PatchFgsVehicleCommand, ApiResponse<FgsVehicleDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleDetailDto>> Handle(
        PatchFgsVehicleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd vehicle {Id}", result.Id);
            return ApiResponse<FgsVehicleDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch vehicle {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVehicleDetailDto>(ex);
        }
    }
}
