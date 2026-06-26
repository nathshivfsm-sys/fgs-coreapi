using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.ListBillingCategories;

public sealed class ListBillingCategoriesQueryHandler(IBillingCategoryReadRepository readRepository)
    : IRequestHandler<ListBillingCategoriesQuery, ApiResponse<PagedResult<BillingCategorySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<BillingCategorySummaryDto>>> Handle(
        ListBillingCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<BillingCategorySummaryDto>>.Ok(result);
    }
}
