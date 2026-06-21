using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.ListActiveSetupTaxDetails;

public sealed class ListActiveSetupTaxDetailsQueryHandler(IFgsSetupTaxDetailReadRepository readRepository)
    : IRequestHandler<ListActiveSetupTaxDetailsQuery, ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>> Handle(
        ListActiveSetupTaxDetailsQuery request,
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
                request.Filters ?? new FgsSetupTaxDetailListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTaxDetailSummaryDto>>(ex);
        }
    }
}
