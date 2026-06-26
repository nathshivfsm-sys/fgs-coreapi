using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.ListGLBreaks;

public sealed record ListGLBreaksQuery(SetupListQuery Query, GLBreakListFilters Filters)
    : IRequest<ApiResponse<PagedResult<GLBreakSummaryDto>>>;
