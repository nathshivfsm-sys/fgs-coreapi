using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.CreateFgsVehicle;

public sealed class CreateFgsVehicleCommandHandler(
    IFgsVehicleWriteService writeService,
    ILogger<CreateFgsVehicleCommandHandler> logger)
    : IRequestHandler<CreateFgsVehicleCommand, ApiResponse<FgsVehicleDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleDetailDto>> Handle(
        CreateFgsVehicleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created vehicle {Id} with code {VIN}", result.Id, result.VIN);
            return ApiResponse<FgsVehicleDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create vehicle");
            return CatalogCrudExceptionMapper.MapException<FgsVehicleDetailDto>(ex);
        }
    }
}
