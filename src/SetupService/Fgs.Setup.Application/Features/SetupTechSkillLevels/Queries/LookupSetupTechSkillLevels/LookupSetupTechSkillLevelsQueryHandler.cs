using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.LookupSetupTechSkillLevels;

public sealed class LookupSetupTechSkillLevelsQueryHandler(IFgsSetupTechSkillLevelReadRepository readRepository)
    : IRequestHandler<LookupSetupTechSkillLevelsQuery, ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>> Handle(
        LookupSetupTechSkillLevelsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>(ex);
        }
    }
}
