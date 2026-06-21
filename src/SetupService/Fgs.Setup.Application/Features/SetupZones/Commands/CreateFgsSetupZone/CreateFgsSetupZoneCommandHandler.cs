using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.CreateFgsSetupZone;

public sealed class CreateFgsSetupZoneCommandHandler(
    IFgsSetupZoneWriteService writeService,
    ILogger<CreateFgsSetupZoneCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupZoneCommand, ApiResponse<FgsSetupZoneDetailDto>>
{
    public async Task<ApiResponse<FgsSetupZoneDetailDto>> Handle(
        CreateFgsSetupZoneCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created zone {Id} with code {Code}", result.Id, result.Code);
            return ApiResponse<FgsSetupZoneDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create zone");
            return CatalogCrudExceptionMapper.MapException<FgsSetupZoneDetailDto>(ex);
        }
    }
}
