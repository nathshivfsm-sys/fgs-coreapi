using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.ListActiveGLBreaks;

public sealed record ListActiveGLBreaksQuery(
    int Page,
    int PageSize,
    string? SortBy,
    SortDirection SortDirection,
    string? Search,
    GLBreakListFilters Filters)
    : IRequest<ApiResponse<PagedResult<GLBreakSummaryDto>>>;
