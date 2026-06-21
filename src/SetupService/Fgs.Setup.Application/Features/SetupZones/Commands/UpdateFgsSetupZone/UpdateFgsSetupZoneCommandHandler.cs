using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.UpdateFgsSetupZone;

public sealed class UpdateFgsSetupZoneCommandHandler(
    IFgsSetupZoneWriteService writeService,
    ILogger<UpdateFgsSetupZoneCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupZoneCommand, ApiResponse<FgsSetupZoneDetailDto>>
{
    public async Task<ApiResponse<FgsSetupZoneDetailDto>> Handle(
        UpdateFgsSetupZoneCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated zone {Id}", result.Id);
            return ApiResponse<FgsSetupZoneDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update zone {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupZoneDetailDto>(ex);
        }
    }
}
