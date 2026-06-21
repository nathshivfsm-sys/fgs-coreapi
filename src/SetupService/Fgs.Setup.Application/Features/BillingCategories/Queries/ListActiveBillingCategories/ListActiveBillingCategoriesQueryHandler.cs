using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.ListActiveBillingCategories;

public sealed class ListActiveBillingCategoriesQueryHandler(IBillingCategoryReadRepository readRepository)
    : IRequestHandler<ListActiveBillingCategoriesQuery, ApiResponse<PagedResult<BillingCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<BillingCategorySummaryDto>>> Handle(
        ListActiveBillingCategoriesQuery request,
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
                request.Filters ?? new BillingCategoryListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<BillingCategorySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<BillingCategorySummaryDto>>(ex);
        }
    }
}
