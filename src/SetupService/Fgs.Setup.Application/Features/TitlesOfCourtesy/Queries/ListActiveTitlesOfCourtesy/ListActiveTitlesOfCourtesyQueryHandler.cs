using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListActiveTitlesOfCourtesy;

public sealed class ListActiveTitlesOfCourtesyQueryHandler(ITitleOfCourtesyReadRepository readRepository)
    : IRequestHandler<ListActiveTitlesOfCourtesyQuery, ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>> Handle(
        ListActiveTitlesOfCourtesyQuery request,
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
                request.Filters ?? new TitleOfCourtesyListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<TitleOfCourtesySummaryDto>>(ex);
        }
    }
}
