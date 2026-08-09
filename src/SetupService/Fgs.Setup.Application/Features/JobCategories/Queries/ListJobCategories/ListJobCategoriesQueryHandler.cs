using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.ListJobCategories;

public sealed class ListJobCategoriesQueryHandler(IJobCategoryReadRepository readRepository)
    : IRequestHandler<ListJobCategoriesQuery, ApiResponse<PagedResult<JobCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobCategorySummaryDto>>> Handle(
        ListJobCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<JobCategorySummaryDto>>.Ok(result);
    }
}
