using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.GetFgsSetupTechSkillLevelById;

public sealed class GetFgsSetupTechSkillLevelByIdQueryHandler(IFgsSetupTechSkillLevelReadRepository readRepository)
    : IRequestHandler<GetFgsSetupTechSkillLevelByIdQuery, ApiResponse<FgsSetupTechSkillLevelDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTechSkillLevelDetailDto>> Handle(
        GetFgsSetupTechSkillLevelByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Fail(
                    [$"Tech Skill Level '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupTechSkillLevelDetailDto>(ex);
        }
    }
}
