using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Queries.ListFgsDataAccesses;

public sealed record ListFgsDataAccessesQuery(
    IdentityListQuery Query,
    FgsDataAccessListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsDataAccessSummaryDto>>>;
