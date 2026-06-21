using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.ListActiveJobTypeSubCategories;

public sealed class ListActiveJobTypeSubCategoriesQueryHandler(IJobTypeSubCategoryReadRepository readRepository)
    : IRequestHandler<ListActiveJobTypeSubCategoriesQuery, ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>> Handle(
        ListActiveJobTypeSubCategoriesQuery request,
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
                request.Filters ?? new JobTypeSubCategoryListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<JobTypeSubCategorySummaryDto>>(ex);
        }
    }
}
