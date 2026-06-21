using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.DeleteFgsSetupTechSkillLevel;

public sealed class DeleteFgsSetupTechSkillLevelCommandHandler(
    IFgsSetupTechSkillLevelWriteService writeService,
    ILogger<DeleteFgsSetupTechSkillLevelCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupTechSkillLevelCommand, ApiResponse<FgsSetupTechSkillLevelDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTechSkillLevelDetailDto>> Handle(
        DeleteFgsSetupTechSkillLevelCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted tech skill level {Id}", result.Id);
            return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete tech skill level {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTechSkillLevelDetailDto>(ex);
        }
    }
}
