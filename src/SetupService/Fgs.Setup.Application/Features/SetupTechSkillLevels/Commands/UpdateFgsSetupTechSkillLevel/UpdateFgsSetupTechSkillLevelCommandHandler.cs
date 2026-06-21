using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.UpdateFgsSetupTechSkillLevel;

public sealed class UpdateFgsSetupTechSkillLevelCommandHandler(
    IFgsSetupTechSkillLevelWriteService writeService,
    ILogger<UpdateFgsSetupTechSkillLevelCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupTechSkillLevelCommand, ApiResponse<FgsSetupTechSkillLevelDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTechSkillLevelDetailDto>> Handle(
        UpdateFgsSetupTechSkillLevelCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated tech skill level {Id}", result.Id);
            return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update tech skill level {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTechSkillLevelDetailDto>(ex);
        }
    }
}
