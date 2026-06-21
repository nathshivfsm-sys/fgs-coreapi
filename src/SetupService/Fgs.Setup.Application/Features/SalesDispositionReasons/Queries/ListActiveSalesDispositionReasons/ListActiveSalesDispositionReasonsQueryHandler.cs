using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.ListActiveSalesDispositionReasons;

public sealed class ListActiveSalesDispositionReasonsQueryHandler(IFgsSalesDispositionReasonReadRepository readRepository)
    : IRequestHandler<ListActiveSalesDispositionReasonsQuery, ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>> Handle(
        ListActiveSalesDispositionReasonsQuery request,
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
                request.Filters ?? new FgsSalesDispositionReasonListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSalesDispositionReasonSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSalesDispositionReasonSummaryDto>>(ex);
        }
    }
}
