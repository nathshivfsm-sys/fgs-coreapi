using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.PatchFgsSetupTechSkillLevel;

public sealed class PatchFgsSetupTechSkillLevelCommandHandler(
    IFgsSetupTechSkillLevelWriteService writeService,
    ILogger<PatchFgsSetupTechSkillLevelCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupTechSkillLevelCommand, ApiResponse<FgsSetupTechSkillLevelDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTechSkillLevelDetailDto>> Handle(
        PatchFgsSetupTechSkillLevelCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd tech skill level {Id}", result.Id);
            return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch tech skill level {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTechSkillLevelDetailDto>(ex);
        }
    }
}
