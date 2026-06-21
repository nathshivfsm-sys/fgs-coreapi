using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.ListSetupTaxDetails;

public sealed class ListSetupTaxDetailsQueryHandler(IFgsSetupTaxDetailReadRepository readRepository)
    : IRequestHandler<ListSetupTaxDetailsQuery, ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>> Handle(
        ListSetupTaxDetailsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTaxDetailSummaryDto>>(ex);
        }
    }
}
