using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.ListSetupPaymentMethods;

public sealed class ListSetupPaymentMethodsQueryHandler(IFgsSetupPaymentMethodReadRepository readRepository)
    : IRequestHandler<ListSetupPaymentMethodsQuery, ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>> Handle(
        ListSetupPaymentMethodsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupPaymentMethodSummaryDto>>(ex);
        }
    }
}
