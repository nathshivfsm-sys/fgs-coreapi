using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.LookupBillingCategories;

public sealed class LookupBillingCategoriesQueryHandler(IBillingCategoryReadRepository readRepository)
    : IRequestHandler<LookupBillingCategoriesQuery, ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>> Handle(
        LookupBillingCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<BillingCategoryLookupDto>>(ex);
        }
    }
}
