using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.ListActiveJobTypeCategories;

public sealed class ListActiveJobTypeCategoriesQueryHandler(IJobTypeCategoryReadRepository readRepository)
    : IRequestHandler<ListActiveJobTypeCategoriesQuery, ApiResponse<PagedResult<JobTypeCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeCategorySummaryDto>>> Handle(
        ListActiveJobTypeCategoriesQuery request,
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
                request.Filters ?? new JobTypeCategoryListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<JobTypeCategorySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<JobTypeCategorySummaryDto>>(ex);
        }
    }
}
