using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.ListJobTypeSubCategories;

public sealed class ListJobTypeSubCategoriesQueryHandler(IJobTypeSubCategoryReadRepository readRepository)
    : IRequestHandler<ListJobTypeSubCategoriesQuery, ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>> Handle(
        ListJobTypeSubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<JobTypeSubCategorySummaryDto>>(ex);
        }
    }
}
