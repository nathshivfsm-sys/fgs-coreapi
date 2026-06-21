using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListSalesActivityTypes;

public sealed class ListSalesActivityTypesQueryHandler(IFgsSalesActivityTypeReadRepository readRepository)
    : IRequestHandler<ListSalesActivityTypesQuery, ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>> Handle(
        ListSalesActivityTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSalesActivityTypeSummaryDto>>(ex);
        }
    }
}
