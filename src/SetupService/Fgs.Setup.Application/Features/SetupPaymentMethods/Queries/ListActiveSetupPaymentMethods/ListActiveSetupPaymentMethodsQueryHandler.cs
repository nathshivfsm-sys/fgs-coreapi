using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.ListActiveSetupPaymentMethods;

public sealed class ListActiveSetupPaymentMethodsQueryHandler(IFgsSetupPaymentMethodReadRepository readRepository)
    : IRequestHandler<ListActiveSetupPaymentMethodsQuery, ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>> Handle(
        ListActiveSetupPaymentMethodsQuery request,
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
                request.Filters ?? new FgsSetupPaymentMethodListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupPaymentMethodSummaryDto>>(ex);
        }
    }
}
