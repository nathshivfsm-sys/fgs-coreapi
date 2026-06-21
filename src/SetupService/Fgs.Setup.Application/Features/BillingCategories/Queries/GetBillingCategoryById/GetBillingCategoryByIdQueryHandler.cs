using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.GetBillingCategoryById;

public sealed class GetBillingCategoryByIdQueryHandler(IBillingCategoryReadRepository readRepository)
    : IRequestHandler<GetBillingCategoryByIdQuery, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        GetBillingCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<BillingCategoryDetailDto>.Fail(
                    [$"Billing Category '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<BillingCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<BillingCategoryDetailDto>(ex);
        }
    }
}
