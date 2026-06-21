using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.ListActiveSetupTechSkillLevels;

public sealed class ListActiveSetupTechSkillLevelsQueryHandler(IFgsSetupTechSkillLevelReadRepository readRepository)
    : IRequestHandler<ListActiveSetupTechSkillLevelsQuery, ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>> Handle(
        ListActiveSetupTechSkillLevelsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupTechSkillLevelListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTechSkillLevelSummaryDto>>(ex);
        }
    }
}
