using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.PatchFgsSetupZone;

public sealed class PatchFgsSetupZoneCommandHandler(
    IFgsSetupZoneWriteService writeService,
    ILogger<PatchFgsSetupZoneCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupZoneCommand, ApiResponse<FgsSetupZoneDetailDto>>
{
    public async Task<ApiResponse<FgsSetupZoneDetailDto>> Handle(
        PatchFgsSetupZoneCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd zone {Id}", result.Id);
            return ApiResponse<FgsSetupZoneDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch zone {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupZoneDetailDto>(ex);
        }
    }
}
