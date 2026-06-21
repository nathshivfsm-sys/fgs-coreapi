using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.ListSetupTechSkillLevels;

public sealed class ListSetupTechSkillLevelsQueryHandler(IFgsSetupTechSkillLevelReadRepository readRepository)
    : IRequestHandler<ListSetupTechSkillLevelsQuery, ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>> Handle(
        ListSetupTechSkillLevelsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTechSkillLevelSummaryDto>>(ex);
        }
    }
}
