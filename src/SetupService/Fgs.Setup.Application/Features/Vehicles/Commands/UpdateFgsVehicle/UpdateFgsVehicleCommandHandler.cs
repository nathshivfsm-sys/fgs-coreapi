using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.UpdateFgsVehicle;

public sealed class UpdateFgsVehicleCommandHandler(
    IFgsVehicleWriteService writeService,
    ILogger<UpdateFgsVehicleCommandHandler> logger)
    : IRequestHandler<UpdateFgsVehicleCommand, ApiResponse<FgsVehicleDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleDetailDto>> Handle(
        UpdateFgsVehicleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated vehicle {Id}", result.Id);
            return ApiResponse<FgsVehicleDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update vehicle {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsVehicleDetailDto>(ex);
        }
    }
}
