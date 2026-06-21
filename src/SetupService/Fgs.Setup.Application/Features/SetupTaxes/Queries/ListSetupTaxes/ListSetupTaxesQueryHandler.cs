using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.ListSetupTaxes;

public sealed class ListSetupTaxesQueryHandler(IFgsSetupTaxReadRepository readRepository)
    : IRequestHandler<ListSetupTaxesQuery, ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>> Handle(
        ListSetupTaxesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTaxSummaryDto>>(ex);
        }
    }
}
