using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Queries.ListFgsApiEvents;

public sealed record ListFgsApiEventsQuery(
    IdentityListQuery Query,
    FgsApiEventListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsApiEventSummaryDto>>>;
