using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.PatchFgsSetupDescription;

public sealed class PatchFgsSetupDescriptionCommandHandler(
    IFgsSetupDescriptionWriteService writeService,
    ILogger<PatchFgsSetupDescriptionCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupDescriptionCommand, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        PatchFgsSetupDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd setup description {Id}", result.Id);
            return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch setup description {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupDescriptionDetailDto>(ex);
        }
    }
}
