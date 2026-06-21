using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.ListActiveTags;

public sealed class ListActiveTagsQueryHandler(IFgsTagReadRepository readRepository)
    : IRequestHandler<ListActiveTagsQuery, ApiResponse<PagedResult<FgsTagSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTagSummaryDto>>> Handle(
        ListActiveTagsQuery request,
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
                request.Filters ?? new FgsTagListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsTagSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsTagSummaryDto>>(ex);
        }
    }
}
