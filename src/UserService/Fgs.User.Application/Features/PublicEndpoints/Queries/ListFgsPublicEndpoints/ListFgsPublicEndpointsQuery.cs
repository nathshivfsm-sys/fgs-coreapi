using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Queries.ListFgsPublicEndpoints;

public sealed record ListFgsPublicEndpointsQuery(
    IdentityListQuery Query,
    FgsPublicEndpointListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsPublicEndpointSummaryDto>>>;
