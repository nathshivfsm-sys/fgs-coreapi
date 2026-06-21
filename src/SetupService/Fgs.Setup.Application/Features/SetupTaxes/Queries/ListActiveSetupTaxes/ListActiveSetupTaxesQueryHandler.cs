using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.ListActiveSetupTaxes;

public sealed class ListActiveSetupTaxesQueryHandler(IFgsSetupTaxReadRepository readRepository)
    : IRequestHandler<ListActiveSetupTaxesQuery, ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>> Handle(
        ListActiveSetupTaxesQuery request,
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
                request.Filters ?? new FgsSetupTaxListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTaxSummaryDto>>(ex);
        }
    }
}
