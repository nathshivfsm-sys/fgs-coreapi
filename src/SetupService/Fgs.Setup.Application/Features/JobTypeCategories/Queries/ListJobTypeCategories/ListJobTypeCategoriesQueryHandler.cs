using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.ListJobTypeCategories;

public sealed class ListJobTypeCategoriesQueryHandler(IJobTypeCategoryReadRepository readRepository)
    : IRequestHandler<ListJobTypeCategoriesQuery, ApiResponse<PagedResult<JobTypeCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeCategorySummaryDto>>> Handle(
        ListJobTypeCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<JobTypeCategorySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<JobTypeCategorySummaryDto>>(ex);
        }
    }
}
