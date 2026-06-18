using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using Fgs.Setup.Application.Features.GLBreaks.Queries.ListGLBreaks;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.ListActiveGLBreaks;

public sealed class ListActiveGLBreaksQueryHandler(IMediator mediator)
    : IRequestHandler<ListActiveGLBreaksQuery, ApiResponse<PagedResult<GLBreakSummaryDto>>>
{
    public Task<ApiResponse<PagedResult<GLBreakSummaryDto>>> Handle(
        ListActiveGLBreaksQuery request,
        CancellationToken cancellationToken) =>
        mediator.Send(
            new ListGLBreaksQuery(
                new SetupListQuery(
                    request.Page,
                    request.PageSize,
                    request.SortBy,
                    request.SortDirection,
                    request.Search,
                    true),
                request.Filters),
            cancellationToken);
}
