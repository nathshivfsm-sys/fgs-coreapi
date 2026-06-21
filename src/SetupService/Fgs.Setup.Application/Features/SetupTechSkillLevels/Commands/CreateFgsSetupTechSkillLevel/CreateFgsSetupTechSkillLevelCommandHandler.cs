using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.CreateFgsSetupTechSkillLevel;

public sealed class CreateFgsSetupTechSkillLevelCommandHandler(
    IFgsSetupTechSkillLevelWriteService writeService,
    ILogger<CreateFgsSetupTechSkillLevelCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupTechSkillLevelCommand, ApiResponse<FgsSetupTechSkillLevelDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTechSkillLevelDetailDto>> Handle(
        CreateFgsSetupTechSkillLevelCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created tech skill level {Id} with code {Code}", result.Id, result.Code);
            return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create tech skill level");
            return CatalogCrudExceptionMapper.MapException<FgsSetupTechSkillLevelDetailDto>(ex);
        }
    }
}
